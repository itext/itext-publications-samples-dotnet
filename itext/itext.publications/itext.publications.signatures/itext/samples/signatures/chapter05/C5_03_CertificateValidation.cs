using System;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Pdf;
using iText.Signatures;
using iText.Signatures.Validation.V1;
using iText.Signatures.Validation.V1.Report;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Chapter05
{
    public class C5_03_CertificateValidation
    {
        public static readonly string DEST = "signatures/chapter05/";

        public static readonly string ROOT = "../../../resources/encryption/rootRsa.cer";

        public static readonly string EXAMPLE = "../../../resources/pdfs/signedPAdES-LT.pdf";

        public const String EXPECTED_OUTPUT =
            "../../../resources/pdfs/signedPAdES-LT.pdf\n"
            + "ValidationReport{validationResult=INDETERMINATE\n" +
            "reportItems=\n" +
            "ReportItem{checkName='Signature verification check.', message='Validating signature Signature1', " +
            "cause=, status=INFO}, \n" +
            "CertificateReportItem{baseclass=\n" +
            "ReportItem{checkName='Revocation data check.', message='Certificate revocation status " +
            "cannot be checked: no revocation data available or the status cannot be determined.', " +
            "cause=, status=INDETERMINATE}\n" +
            "certificate=C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestTsCert}, \n" +
            "CertificateReportItem{baseclass=\n" +
            "ReportItem{checkName='Certificate check.', message='Certificate C=BY,L=Minsk,O=iText,OU=test," +
            "CN=iTextTestRoot is trusted, revocation data checks are not required.', cause=, status=INFO}\n" +
            "certificate=C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRoot}, \n" +
            "CertificateReportItem{baseclass=\n" +
            "ReportItem{checkName='Revocation data check.', message='Certificate revocation status " +
            "cannot be checked: no revocation data available or the status cannot be determined.', " +
            "cause=, status=INDETERMINATE}\n" +
            "certificate=C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRsaCert01}, \n" +
            "CertificateReportItem{baseclass=\n" +
            "ReportItem{checkName='Certificate check.', message='Certificate C=BY,L=Minsk,O=iText,OU=test," +
            "CN=iTextTestRoot is trusted, revocation data checks are not required.', cause=, status=INFO}\n" +
            "certificate=C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRoot}, }";

        public static TextWriter OUT_STREAM = Console.Out;

        public void VerifySignatures(String path)
        {
            // Set up the validator.
            SignatureValidationProperties properties = new SignatureValidationProperties();
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            var parser = new X509CertificateParser();
            IX509Certificate rootCert;
            using (FileStream stream = new FileStream(ROOT, FileMode.Open, FileAccess.Read))
            {
                rootCert = new X509CertificateBC(parser.ReadCertificate(stream));
            }

            certificateRetriever.AddTrustedCertificates(new[] { rootCert });
            ValidatorChainBuilder validatorChainBuilder = new ValidatorChainBuilder()
                .WithIssuingCertificateRetriever(certificateRetriever)
                .WithSignatureValidationProperties(properties);
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(path)))
            {
                OUT_STREAM.WriteLine(path);
                SignatureValidator validator = validatorChainBuilder.BuildSignatureValidator(document);
                // Validate all signatures in the document.
                report = validator.ValidateSignatures();
                OUT_STREAM.WriteLine(report);
            }
        }

        public static void Main(String[] args)
        {
            C5_03_CertificateValidation app = new C5_03_CertificateValidation();

            app.VerifySignatures(EXAMPLE);
        }
    }
}