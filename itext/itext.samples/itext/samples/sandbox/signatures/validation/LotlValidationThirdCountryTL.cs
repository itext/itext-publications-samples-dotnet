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
    /// <summary>
    /// Sample: validation of the List of Trusted Lists (LOTL) for third countries.
    /// </summary>
    /// <remarks>
    /// What this sample does:
    /// <para>- Points the <see cref="LotlValidator"/> to the European Commission third-country LOTL
    /// (also known as MRA/AdES LOTL) instead of the default EU LOTL URL.</para>
    /// <para>- Customizes fetching of Official Journal signing certificates used to sign the third-country LOTL
    /// by extracting X.509 certificates from the pointers section of a local TSL resource.</para>
    /// <para>- Overrides pivot handling because the third-country LOTL does not publish pivot files the same way
    /// as the EU LOTL. The custom pivot fetcher directly verifies the LOTL XML signature instead.</para>
    /// <para>- Initializes a <see cref="LotlService"/> with tailored <see cref="LotlFetchingProperties"/> that limit countries
    /// to those relevant for the scenario (e.g., UA, MD) and remove failed country data from use.</para>
    /// <para>- Produces a <see cref="ValidationReport"/> describing the LOTL validation outcome.</para>
    ///
    /// When to use this approach:
    /// <para>- When you need to validate the LOTL for third countries.</para>
    /// <para>- When you need to adapt resource/pivot fetching logic to the structure of the third-country LOTL.</para>
    ///
    /// Key customizations in this file:
    /// <para>- EuropeanTrustedListConfigurationFactoryForThirdCountries: supplies the third‑country LOTL URI
    /// while reusing the Official Journal signing certificates from the default configuration.</para>
    /// <para>- ThirdCountriesResourceFetcher: provides Official Journal certificates by parsing them from
    /// PointersToOtherTSL data of a local TSL resource.</para>
    /// <para>- ThirdCountriesDoesNotContainPivots: bypasses pivot downloads and validates the LOTL XML
    /// signature directly with trusted Official Journal certificates.</para>
    /// </remarks>
    public class LotlValidationThirdCountryTL {
        /// <summary>
        /// Path to a local TSL XML used only to extract Official Journal certificates from
        /// the PointersToOtherTSL section. This is a convenience source for the certificates
        /// that sign the third-country LOTL.
        /// </summary>
        private const String TSL = "../../../resources/validation/tsl/jgoigecgmelgnadppbgklkndmkdgcjpm";
        /// <summary>
        /// Path where the textual <see cref="ValidationReport"/> will be written.
        /// </summary>
        public static readonly string DEST = "results/sandbox/signatures/validation/third_country_tl_validation.txt";

        /// <summary>
        /// Entry point that ensures the output folder exists and triggers third-country LOTL validation.
        /// </summary>
        /// <param name="args">CLI arguments (not used)</param>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            ValidateThirdCountryTL();
        }

        /// <summary>
        /// Configures a <see cref="LotlService"/> and <see cref="LotlValidator"/> to work with the third-country LOTL:
        /// </summary>
        /// <remarks>
        /// <para>- Temporarily swaps the global <see cref="EuropeanTrustedListConfigurationFactory"/> with a variant that
        /// returns the third-country LOTL URI.</para>
        /// <para>- Sets <see cref="LotlFetchingProperties"/> to remove failing country data and to focus on specific
        /// country codes (e.g., UA, MD).</para>
        /// <para>- Installs <see cref="ThirdCountriesResourceFetcher"/> to supply Official Journal certificates and
        /// <see cref="ThirdCountriesDoesNotContainPivots"/> to validate without pivots.</para>
        /// <para>- Initializes cache, validates the LOTL, and writes the result to <see cref="DEST"/>.</para>
        /// <para>The original factory is restored in a finally block to avoid side effects.</para>
        /// </remarks>
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
        /// <summary>
        /// Custom <see cref="EuropeanTrustedListConfigurationFactory"/> that targets the third‑country LOTL.
        /// </summary>
        /// <remarks>
        /// Why it is needed:
        /// <para>- The third-country LOTL is published at a different URI than the main EU LOTL; this factory
        /// points the validator to that URI.</para>
        /// <para>- The third-country LOTL is signed with EU Official Journal certificates, so we reuse the
        /// certificates provided by the original factory.</para>
        /// </remarks>
        internal class EuropeanTrustedListConfigurationFactoryForThirdCountries : EuropeanTrustedListConfigurationFactory {
            private readonly EuropeanTrustedListConfigurationFactory originalFactory;

//\cond DO_NOT_DOCUMENT
            /// <summary>
            /// Wraps the original factory to delegate certificate retrieval while overriding the LOTL URI.
            /// </summary>
            /// <param name="originalFactory">The default factory used to obtain Official Journal certificates.</param>
            internal EuropeanTrustedListConfigurationFactoryForThirdCountries(EuropeanTrustedListConfigurationFactory 
                originalFactory) {
                this.originalFactory = originalFactory;
            }
//\endcond

            /// <summary>
            /// Returns the URI of the third‑country (MRA/AdES) LOTL.
            /// </summary>
            public override String GetTrustedListUri() {
                return "https://ec.europa.eu/tools/lotl/mra/ades-lotl.xml";
            }

            /// <summary>
            /// Third‑country LOTL does not rely on the same publication identifier; empty string is returned.
            /// </summary>
            public override String GetCurrentlySupportedPublication() {
                return "";
            }

            /// <summary>
            /// Reuses Official Journal signing certificates from the original factory so the XML signature
            /// of the third-country LOTL can be validated with the same trust anchors.
            /// </summary>
            /// <returns>Official Journal signing certificates.</returns>
            public override IList<IX509Certificate> GetCertificates() {
                return originalFactory.GetCertificates();
            }
        }
//\endcond

//\cond DO_NOT_DOCUMENT
        /// <summary>
        /// Resource fetcher tailored for the third‑country LOTL.
        /// </summary>
        /// <remarks>
        /// What it does:
        /// <para>- Provides EU Official Journal certificates by parsing them from the PointersToOtherTSL section
        /// of a local TSL XML resource.</para>
        /// <para>- Ensures the validator can verify the signature of the third‑country LOTL without depending
        /// on the default resource locations.</para>
        /// </remarks>
        internal class ThirdCountriesResourceFetcher : EuropeanResourceFetcher
        {
            /// <summary>
            /// Supplies Official Journal certificates for the third‑country LOTL by extracting
            /// X.509 values from a local TSL resource.
            /// </summary>
            /// <returns>A result whose local report records parsing issues if any.</returns>
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

            /// <summary>
            /// Loads Official Journal certificates from a local TSL XML file by reading the
            /// PointersToOtherTSL → ServiceDigitalIdentities → X509Certificate entries.
            /// </summary>
            /// <param name="euXmlPath">Path to a local TSL XML.</param>
            /// <returns>List of parsed certificates.</returns>
            private static IList<IX509Certificate> LoadCertificatesFromPointersToOtherTSL(String euXmlPath)
            {
                using (Stream @in = iText.Commons.Utils.FileUtil.GetInputStreamForFile(euXmlPath))
                {
                    return LoadCertificatesFromPointersToOtherTSL(@in);
                }
            }

            /// <summary>
            /// Parses X.509 certificates from the given TSL XML stream using a namespace‑agnostic XPath.
            /// </summary>
            /// <param name="xmlStream">Input stream containing TSL XML.</param>
            /// <returns>List of X.509 certificates extracted from the XML.</returns>
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
        /// <summary>
        /// Pivot handler for the third‑country LOTL which does not rely on pivot files.
        /// </summary>
        /// <remarks>
        /// Why it is needed:
        /// <para>- The third‑country LOTL does not publish or require pivot files in the same manner as the EU LOTL.</para>
        /// <para>- This fetcher short‑circuits pivot retrieval and directly validates the LOTL XML signature
        /// using trusted Official Journal certificates.</para>
        /// </remarks>
        internal class ThirdCountriesDoesNotContainPivots : PivotFetcher {
            /// <summary>
            /// Creates the pivot fetcher bound to the provided <see cref="LotlService"/>.
            /// </summary>
            /// <param name="service">Current LOTL service instance.</param>
            public ThirdCountriesDoesNotContainPivots(LotlService service)
                : base(service) {
            }

            /// <summary>
            /// Retains the current journal URI if set by upstream logic.
            /// </summary>
            /// <param name="currentJournalUri">Current journal URI.</param>
            public override void SetCurrentJournalUri(String currentJournalUri) {
                base.SetCurrentJournalUri(currentJournalUri);
            }

            /// <summary>
            /// Validates the LOTL XML directly instead of downloading pivot files.
            /// </summary>
            /// <remarks>
            /// Implementation details:
            /// <para>- Builds a <see cref="TrustedCertificatesStore"/> from the provided Official Journal certificates.</para>
            /// <para>- Uses a <see cref="CustomXmlSignatureValidator"/> to validate the LOTL XML signature.</para>
            /// <para>- On failure, merges the detailed validation report and marks the operation as invalid.</para>
            /// </remarks>
            /// <param name="lotlXml">The LOTL XML bytes to validate.</param>
            /// <param name="certificates">Trusted Official Journal certificates.</param>
            /// <returns>A result whose local report contains signature validation details.</returns>
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
            /// <summary>
            /// Small adapter around <see cref="XmlSignatureValidator"/> to expose its validate method
            /// for direct use in this sample.
            /// </summary>
            internal class CustomXmlSignatureValidator : XmlSignatureValidator {
                /// <summary>
                /// Creates a validator using the provided trust store containing Official Journal certificates.
                /// </summary>
                /// <remarks>
                /// This constructor shall not be used directly outside of this sample.
                /// </remarks>
                /// <param name="trustedCertificatesStore">
                /// <see cref="iText.Signatures.Validation.TrustedCertificatesStore"/> which contains trusted certificates.
                /// </param>
                protected internal CustomXmlSignatureValidator(TrustedCertificatesStore trustedCertificatesStore)
                    : base(trustedCertificatesStore) {
                }

                /// <summary>
                /// Exposes <see cref="XmlSignatureValidator.Validate(Stream)"/> for use by the enclosing pivot fetcher.
                /// </summary>
                /// <param name="xmlDocumentInputStream">Input stream of the LOTL XML.</param>
                /// <returns>A <see cref="ValidationReport"/> with the signature validation outcome.</returns>
                public virtual ValidationReport PublicValidate(Stream xmlDocumentInputStream) {
                    return base.Validate(xmlDocumentInputStream);
                }
            }
//\endcond
        }
//\endcond
    }
}