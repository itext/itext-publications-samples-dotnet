using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Signatures.Validation;
using iText.Signatures.Validation.Lotl;
using iText.Signatures.Validation.Report;
using Org.BouncyCastle.X509;

namespace iText.Samples.Sandbox.Signatures.Validation {
    public class LotlValidationThirdCountryTL {
        private const String TSL = "../../../resources/validation/tsl/jgoigecgmelgnadppbgklkndmkdgcjpm";
        public static readonly string DEST = "results/sandbox/signatures/validation/third_country_tl_validation.txt";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            ValidateThirdCountryTL();
        }

        public static void ValidateThirdCountryTL()
        {
            EuropeanTrustedListConfigurationFactory ogFactory = EuropeanTrustedListConfigurationFactory.GetFactory()();
            try {
                EuropeanTrustedListConfigurationFactory.SetFactory(() => new EuropeanTrustedListConfigurationFactoryForThirdCountries
                    (ogFactory));
                LotlFetchingProperties lotlFetchingProperties = new LotlFetchingProperties(new RemoveOnFailingCountryData(
                ));
                lotlFetchingProperties.SetCountryNames("UA", "MD");
                LotlValidator validator;
                using (LotlService lotlService = new LotlService(lotlFetchingProperties)) {
                    lotlService.WithEuropeanResourceFetcher(new LotlValidationThirdCountryTL.ThirdCountriesResourceFetcher());
                    lotlService.WithPivotFetcher(new ThirdCountriesDoesNotContainPivots(lotlService));
                    lotlService.InitializeCache();
                    validator = new LotlValidator(lotlService);
                }
                ValidationReport report = validator.Validate();
                //Here you have the validation report and can use it as you need
                File.WriteAllText(DEST, report.ToString(), Encoding.UTF8);
                System.Console.Out.WriteLine(report);
            }
            finally {
                EuropeanTrustedListConfigurationFactory.SetFactory(() => ogFactory);
            }
        }

//\cond DO_NOT_DOCUMENT
        internal class EuropeanTrustedListConfigurationFactoryForThirdCountries : EuropeanTrustedListConfigurationFactory {
            private readonly EuropeanTrustedListConfigurationFactory originalFactory;

//\cond DO_NOT_DOCUMENT
            internal EuropeanTrustedListConfigurationFactoryForThirdCountries(EuropeanTrustedListConfigurationFactory 
                originalFactory) {
                this.originalFactory = originalFactory;
            }
//\endcond

            public override String GetTrustedListUri() {
                return "https://ec.europa.eu/tools/lotl/mra/ades-lotl.xml";
            }

            public override String GetCurrentlySupportedPublication() {
                return "";
            }

            public override IList<IX509Certificate> GetCertificates() {
                return originalFactory.GetCertificates();
            }
        }
//\endcond

//\cond DO_NOT_DOCUMENT
        internal class ThirdCountriesResourceFetcher : EuropeanResourceFetcher
        {
            public override EuropeanResourceFetcher.Result GetEUJournalCertificates()
            {
                EuropeanResourceFetcher.Result result = new EuropeanResourceFetcher.Result();
                SafeCalling.OnExceptionLog(() => result.SetCertificates(LoadCertificatesFromPointersToOtherTSL(
                    System.IO.Path.Combine
                        (TSL))), result.GetLocalReport(), (e) => new ReportItem(LotlValidator.LOTL_VALIDATION,
                    "JOURNAL_CERT_NOT_PARSABLE"
                    , e, ReportItem.ReportItemStatus.INFO));
                return result;
            }

            private static IList<IX509Certificate> LoadCertificatesFromPointersToOtherTSL(String euXmlPath)
            {
                using (Stream @in = iText.Commons.Utils.FileUtil.GetInputStreamForFile(euXmlPath))
                {
                    return LoadCertificatesFromPointersToOtherTSL(@in);
                }
            }

            private static IList<IX509Certificate> LoadCertificatesFromPointersToOtherTSL(Stream xmlStream)
            {
                // Parse XML with .NET XmlDocument and use XPath with local-name() to be namespace-agnostic
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.Load(xmlStream);

                string x509CertXPath =
                    "/*[local-name()='TrustServiceStatusList']" +
                    "/*[local-name()='SchemeInformation']" +
                    "/*[local-name()='PointersToOtherTSL']" +
                    "//*[local-name()='ServiceDigitalIdentities']" +
                    "/*[local-name()='ServiceDigitalIdentity']" +
                    "/*[local-name()='DigitalId']" +
                    "/*[local-name()='X509Certificate']";

                XmlNodeList nodes = doc.SelectNodes(x509CertXPath);

                X509CertificateParser parser = new X509CertificateParser();
                IList<IX509Certificate> result = new List<IX509Certificate>(nodes != null ? nodes.Count : 0);

                if (nodes != null)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        string b64 = nodes.Item(i)?.InnerText;
                        if (b64 == null)
                        {
                            continue;
                        }

                        b64 = b64.Trim();
                        if (string.IsNullOrEmpty(b64))
                        {
                            continue;
                        }

                        byte[] der = Convert.FromBase64String(b64);
                        using (MemoryStream bin = new MemoryStream(der))
                        {
                            Org.BouncyCastle.X509.X509Certificate cert = parser.ReadCertificate(bin);
                            result.Add(new X509CertificateBC(cert));
                        }
                    }
                }

                return result;
            }
        }
//\endcond

//\cond DO_NOT_DOCUMENT
        internal class ThirdCountriesDoesNotContainPivots : PivotFetcher {
            public ThirdCountriesDoesNotContainPivots(LotlService service)
                : base(service) {
            }

            public override void SetCurrentJournalUri(String currentJournalUri) {
                base.SetCurrentJournalUri(currentJournalUri);
            }

            public override PivotFetcher.Result DownloadAndValidatePivotFiles(byte[] lotlXml, IList<IX509Certificate> 
                certificates) {
                IList<IX509Certificate> trustedCertificates = certificates;
                PivotFetcher.Result result = new PivotFetcher.Result();
                TrustedCertificatesStore trustedCertificatesStore = new TrustedCertificatesStore();
                trustedCertificatesStore.AddGenerallyTrustedCertificates(trustedCertificates);
                LotlValidationThirdCountryTL.ThirdCountriesDoesNotContainPivots.CustomXmlSignatureValidator xmlSignatureValidator
                     = new LotlValidationThirdCountryTL.ThirdCountriesDoesNotContainPivots.CustomXmlSignatureValidator(trustedCertificatesStore
                    );
                ValidationReport localReport = xmlSignatureValidator.PublicValidate(new MemoryStream(lotlXml));
                if (localReport.GetValidationResult() != ValidationReport.ValidationResult.VALID) {
                    result.GetLocalReport().AddReportItem(new ReportItem(LotlValidator.LOTL_VALIDATION, "LOTL_VALIDATION_UNSUCCESSFUL"
                        , ReportItem.ReportItemStatus.INVALID));
                    result.GetLocalReport().Merge(localReport);
                    return result;
                }
                return result;
            }

//\cond DO_NOT_DOCUMENT
            internal class CustomXmlSignatureValidator : XmlSignatureValidator {
                /// <summary>
                /// Creates
                /// <see cref="XmlSignatureValidator"/>
                /// instance.
                /// </summary>
                /// <remarks>
                /// Creates
                /// <see cref="XmlSignatureValidator"/>
                /// instance. This constructor shall not be used directly.
                /// </remarks>
                /// <param name="trustedCertificatesStore">
                /// 
                /// <see cref="iText.Signatures.Validation.TrustedCertificatesStore"/>
                /// which contains trusted certificates
                /// </param>
                protected internal CustomXmlSignatureValidator(TrustedCertificatesStore trustedCertificatesStore)
                    : base(trustedCertificatesStore) {
                }

                public virtual ValidationReport PublicValidate(Stream xmlDocumentInputStream) {
                    return base.Validate(xmlDocumentInputStream);
                }
            }
//\endcond
        }
//\endcond
    }
}