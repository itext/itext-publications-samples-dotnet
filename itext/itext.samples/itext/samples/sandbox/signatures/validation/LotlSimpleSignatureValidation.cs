using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Signatures.Validation;
using iText.Signatures.Validation.Lotl;
using iText.Signatures.Validation.Report;

namespace iText.Samples.Sandbox.Signatures.Validation {
    public class LotlSimpleSignatureValidation {
        public static readonly string SRC = "../../../resources/pdfs"
                                            + "/super_official_document_signed.pdf";

        public static readonly string DEST = "results/sandbox/signatures/validation/somepdf.pdf";
        public static readonly string DUMMY_PDF = "../../../resources/validation/pdf/dummy.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            File.Copy(DUMMY_PDF, DEST, true);
            new LotlSimpleSignatureValidation().showCaseCacheInitializationAndSimpleUsage();
        }

        public void showCaseCacheInitializationAndSimpleUsage() {
            ValidatorChainBuilder builder = new ValidatorChainBuilder();
            // We want to use LOTL as a source of trusted certificates
            builder.WithOcspClient(() => new DummyOcspClient());
            builder.TrustEuropeanLotl(true);
            //We need to configure some additional properties for LOTL fetching
            // First of all we want to remove country data if something goes wrong during fetching,
            // here we choose to just not use those certificates, but other strategies are possible like if you
            //really need the certificates you can use new ThrowExceptionOnFailingCountryData() and handle the exception
            // in your code. (maybe try again later etc.)
            LotlFetchingProperties fetchingProperties = new LotlFetchingProperties(new RemoveOnFailingCountryData());
            //Our pdf is signed by a portuguese certificate, so we need to fetch the portuguese country lotl data.
            //If this is not set, the default behaviour is to fetch all countries in the lotl list (all european
            // countries + uk).
            fetchingProperties.SetCountryNames(LotlCountryCodeConstants.PORTUGAL);
            LotlService.InitializeGlobalCache(fetchingProperties);
            //If we want all countries except from a few we can use following api:
            //fetchingProperties
            // .setCountryNamesToIgnore(LotlCountryCodeConstants.ITALY, LotlCountryCodeConstants.UNITED_KINGDOM);

            //By default the cache is considered valid for 24 hours, if you want to change this you can use method
            //fetchingProperties.setCacheStalenessInMilliseconds
            //We highly recommend to not set this value too low as fetching the lotl data is network intensive,
            //and we want to avoid fetching it too often if not really needed.
            fetchingProperties.SetCacheStalenessInMilliseconds(24 * 60 * 60 * 1000 * 2); //2 day

            // Behind the scenes we will refresh the certificates based on the
            fetchingProperties.GetRefreshIntervalCalculator();
            // By default,  we will try 4 times per cache staleness period, So if the cache is valid for 24 hours
            // we will try to refresh every 6 hours, if you want to change this you can use
            fetchingProperties.SetRefreshIntervalCalculator(
                (cacheStalenessInMilliseconds) => cacheStalenessInMilliseconds / 8); //try every 3 hours
            //If you want to disable the automatic refresh you can use INT.MAX_VALUE as the refresh interval
            // fetchingProperties.setRefreshIntervalCalculator((cacheStalenessInMilliseconds) => Integer.MAX_VALUE);

            //If you ran it without adding to the dependencies:
            // eu-trusted-lists-resources
            // you would get an exception here as the resources are not found. Something along the lines of:
            //Exception in thread "main" com.itextpdf.kernel.exceptions.PdfException: European Trusted List resources are
            // not available. Please ensure that the itextpdf-eutrustedlistsresources module is included in your project.
            // Alternatively,  you can use the EuropeanTrustedListConfigurationFactory to load the resources from a
            // custom location.
            using (PdfDocument document = new PdfDocument(new PdfReader(SRC))) {
                SignatureValidator validator = builder.BuildSignatureValidator(document);
                ValidationReport r = validator.ValidateSignatures();
                // Here you have the validation report and can use it as you need
                Console.WriteLine(r);
            }
        }
    }
}