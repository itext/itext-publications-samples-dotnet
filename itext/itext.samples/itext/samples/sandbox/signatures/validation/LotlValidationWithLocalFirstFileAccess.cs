using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using iText.IO.Resolver.Resource;
using iText.Kernel.Pdf;
using iText.Signatures.Validation;
using iText.Signatures.Validation.Lotl;
using iText.Signatures.Validation.Report;
using NVelocity.Runtime;

namespace iText.Samples.Sandbox.Signatures.Validation {
    public class LotlValidationWithLocalFirstFileAccess {
        public static readonly string SRC = "../../../resources/pdfs"
                                            + "/super_official_document_signed.pdf";

        public static readonly string XMLS = "../../../resources/validation/xml/";
        public static readonly string DUMMY_PDF = "../../../resources/validation/pdf/dummy.pdf";
        public static readonly string DEST = "results/sandbox/signatures/validation/somepdf.pdf";

        public static void Main(string[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            File.Copy(DUMMY_PDF, DEST, true);
            new LotlValidationWithLocalFirstFileAccess().useLocalFirstAccess();
        }

        public void useLocalFirstAccess() {
            ValidatorChainBuilder builder = new ValidatorChainBuilder();
            builder.WithOcspClient(() => new DummyOcspClient());
            // We want to use LOTL as a source of trusted certificates
            builder.TrustEuropeanLotl(true);

            LotlFetchingProperties fetchingProperties = new LotlFetchingProperties(new RemoveOnFailingCountryData());
            fetchingProperties.SetCountryNames(LotlCountryCodeConstants.PORTUGAL);

            using (LotlService lotlService = new LotlService(fetchingProperties)) {
                lotlService.WithCustomResourceRetriever(new FromFileAccess(XMLS));

                lotlService.InitializeCache();
                builder.WithLotlService(() => lotlService);

                using (PdfDocument document = new PdfDocument(new PdfReader(SRC))) {
                    SignatureValidator validator = builder.BuildSignatureValidator(document);
                    ValidationReport r = validator.ValidateSignatures();
                    //Here you have the validation report and can use it as you need
                    Console.WriteLine(r);
                }
            }
        }

        class FromFileAccess : DefaultResourceRetriever {
            private readonly string resourcePath;

            public FromFileAccess(string resourcePath) {
                this.resourcePath = resourcePath;
            }


            public override byte[] GetByteArrayByUrl(Uri url) {
                string fileName = Regex.Replace( url.ToString(),"[^a-zA-Z0-9]", "_");
                string path = Path.GetFullPath(resourcePath + fileName);

                Console.WriteLine(path);
                Console.WriteLine("Trying to access file: " + new FileInfo(path).FullName);
                if (File.Exists(path)) {
                    //here we can implement timers to force refreshing the files if needed
                    // for example force refresh if the file is older than 1 day etc.
                    //This can be done by checking the file attributes for the last modified time
                    //This is left as an exercise to the reader :)

                    return File.ReadAllBytes(path);
                }

                byte[] data = null;
                try {
                    data = base.GetByteArrayByUrl(url);
                }
                catch (Exception e) {
                    //Super naive reusingmechanism in case of network issues
                    WaitABit();
                    try {
                        data = base.GetByteArrayByUrl(url);
                    }
                    catch (Exception ex) {
                        //nothing we can do if it fails again
                    }
                }

                if (data != null) {
                    File.WriteAllBytes(path, data);
                }

                return data;
            }

            private void WaitABit() {
                Thread.Sleep(2000);
            }
        }
    }
}