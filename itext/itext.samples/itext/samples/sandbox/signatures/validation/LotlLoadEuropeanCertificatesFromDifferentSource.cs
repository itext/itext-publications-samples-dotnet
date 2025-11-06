using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures.Validation;
using iText.Signatures.Validation.Lotl;
using iText.Signatures.Validation.Report;
using NVelocity.Runtime;

namespace iText.Samples.Sandbox.Signatures.Validation {
    public class LotlLoadEuropeanCertificatesFromDifferentSource {
        public static readonly String SRC = "../../../resources/pdfs"
                                            + "/super_official_document_signed.pdf";

        public static readonly String CERTS = "../../../resources/cert/european_certs/";

        public static readonly String DUMMY_PDF = "../../../resources/validation/pdf/dummy.pdf";
        public static readonly String DEST = "results/sandbox/signatures/validation/somepdf.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            File.Copy(DUMMY_PDF, DEST, true);
            new LotlLoadEuropeanCertificatesFromDifferentSource().loadEuropeanCertificatesFromPemFiles();
        }

        public void loadEuropeanCertificatesFromPemFiles() {
            ValidatorChainBuilder builder = new ValidatorChainBuilder();

            builder.WithOcspClient(() => new DummyOcspClient());
            builder.TrustEuropeanLotl(true);
            LotlFetchingProperties fetchingProperties = new LotlFetchingProperties(new RemoveOnFailingCountryData());
            fetchingProperties.SetCountryNames(LotlCountryCodeConstants.PORTUGAL);
            using (LotlService lotlService = new LotlService(fetchingProperties)) {
                //You might not want to rely on our provided resources module and want to load the european trusted list
                // from
                // the pem files you have downloaded and verified yourself. In this case you can implement your own
                // EuropeanResourceFetcher
                // and provide it to the LotlService like shown below.
                lotlService.WithEuropeanResourceFetcher(
                    new LoadEuropeanCertificatesFromPemFiles(CERTS));


                //Don't forget to initialize the custom cache before using the LotlService or you will get an exception
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

        class LoadEuropeanCertificatesFromPemFiles : EuropeanResourceFetcher {
            private readonly String pathToPemFolderDirectory;

            private readonly List<String> pemFileName = new List<string>() {
                "1.pem",
                "2.pem",
                "3.pem",
                "4.pem",
                "5.pem",
                "6.pem",
                "7.pem",
                "8.pem"
            };

            public LoadEuropeanCertificatesFromPemFiles(String pathToPemFolderDirectory) {
                this.pathToPemFolderDirectory = pathToPemFolderDirectory;
            }


            public override Result GetEUJournalCertificates() {
                var r = new Result(); 
                List<IX509Certificate> certs = new List<IX509Certificate>();
                try {
                    foreach (String pemFile in pemFileName) {
                        String fullPath = pathToPemFolderDirectory + "/" + pemFile;
                        IX509Certificate c = PemFileHelper.ReadFirstChain(fullPath)[0];
                        certs.Add(c);
                    }
                }
                catch (Exception) {
                    throw new RuntimeException("");
                }

                r.SetCertificates(certs);
                return r;
            }
        }
    }
}