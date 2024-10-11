using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Clients;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;
using iText.Signatures.Validation;
using iText.Signatures.Validation.Context;
using iText.Signatures.Validation.Report;

namespace iText.Samples.Sandbox.Signatures.Validation
{
    /// <summary>Basic example of the certificate chain validation before the document signing.</summary>
    public class ValidateChainBeforeSigningExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly String DEST =
            "results/sandbox/signatures/validation/validateChainBeforeSigningExample.txt";

        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String ROOT = "../../../resources/cert/root.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        private IX509Certificate[] certificateChain;
        private IPrivateKey privateKey;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ValidateChainBeforeSigningExample().ValidateChainAndSignSignature(SRC, DEST);
        }

        /// <summary>Basic example of the certificate chain validation before the document signing.</summary>
        /// <param name="src">pdf source file</param>
        /// <param name="dest">validation report destination file</param>
        public virtual void ValidateChainAndSignSignature(String src, String dest)
        {
            IX509Certificate[] certificateChain = GetCertificateChain();
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[1];
            // Set up the validator.
            SignatureValidationProperties properties = new SignatureValidationProperties()
                .AddOcspClient(GetOcspClient());
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            ValidatorChainBuilder validatorChainBuilder = new ValidatorChainBuilder().WithIssuingCertificateRetrieverFactory(
                () => certificateRetriever).WithSignatureValidationProperties(properties);
            CertificateChainValidator validator = validatorChainBuilder.BuildCertificateChainValidator();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidationContext baseContext = new ValidationContext(ValidatorContext.CERTIFICATE_CHAIN_VALIDATOR,
                CertificateSource.SIGNER_CERT, TimeBasedContext.PRESENT);
            // Validate the chain. ValidationReport will contain all the validation report messages.
            ValidationReport report = validator.ValidateCertificate(baseContext, signingCert,
                DateTimeUtil.GetCurrentUtcTime
                    ());
            if (ValidationReport.ValidationResult.VALID == report.GetValidationResult())
            {
                SignDocument(src);
            }

            // Write validation report to the file.
            using (FileStream fos = new FileStream(dest, FileMode.Create))
            {
                byte[] reportBytes = GetBytes(report.ToString());
                fos.Write(reportBytes, 0, reportBytes.Length);
            }
        }

        /// <summary>Sign document with PaDES Baseline-B Profile.</summary>
        /// <param name="src">source file</param>
        protected internal virtual void SignDocument(String src)
        {
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(FileUtil.GetInputStreamForFile(src)),
                new MemoryStream
                    ());
            SignerProperties signerProperties = CreateSignerProperties();
            padesSigner.SignWithBaselineBProfile(signerProperties, GetCertificateChain(), GetPrivateKey(SIGN));
        }

        /// <summary>Creates signing chain for the sample.</summary>
        /// <remarks>Creates signing chain for the sample. This chain shouldn't be used for the real signing.</remarks>
        /// <returns>the chain of certificates to be used for the signing operation.</returns>
        protected internal virtual IX509Certificate[] GetCertificateChain()
        {
            if (certificateChain == null)
            {
                try
                {
                    certificateChain = PemFileHelper.ReadFirstChain(CHAIN);
                }
                catch (Exception)
                {
                    // Ignore.
                }
            }

            return certificateChain;
        }

        /// <summary>Creates private key for the sample.</summary>
        /// <remarks>Creates private key for the sample. This key shouldn't be used for the real signing.</remarks>
        /// <param name="path">path to the private key</param>
        /// <returns>
        /// 
        /// <see cref="iText.Commons.Bouncycastle.Crypto.IPrivateKey"/>
        /// instance to be used for the main signing operation.
        /// </returns>
        protected internal virtual IPrivateKey GetPrivateKey(String path)
        {
            if (privateKey == null)
            {
                try
                {
                    privateKey = PemFileHelper.ReadFirstKey(path, PASSWORD);
                }
                catch (Exception)
                {
                    // Ignore.
                }
            }

            return privateKey;
        }

        /// <summary>Creates an OCSP client for the sample.</summary>
        /// <remarks>
        /// Creates an OCSP client for the sample.
        /// <para />
        /// NOTE: for the real signing you should use real revocation data clients
        /// (such as
        /// <see cref="iText.Signatures.OcspClientBouncyCastle"/>
        /// ).
        /// </remarks>
        /// <returns>the OCSP client to be used for the signing operation.</returns>
        protected internal virtual IOcspClient GetOcspClient()
        {
            try
            {
                IX509Certificate[] certificates = GetCertificateChain();
                TestOcspResponseBuilder builder = new TestOcspResponseBuilder((IX509Certificate)certificates[1],
                    GetPrivateKey
                        (ROOT));
                DateTime currentDate = DateTimeUtil.GetCurrentUtcTime();
                builder.SetProducedAt(currentDate);
                builder.SetThisUpdate(DateTimeUtil.GetCalendar(currentDate));
                builder.SetNextUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(10)));
                return new TestOcspClient().AddBuilderForCertificate(certificates[0], builder);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates properties to be used in signing operations.</summary>
        /// <returns>
        /// 
        /// <see cref="iText.Signatures.SignerProperties"/>
        /// properties to be used for main signing operation.
        /// </returns>
        protected internal virtual SignerProperties CreateSignerProperties()
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName("Signature1");
            SignatureFieldAppearance appearance =
                new SignatureFieldAppearance(SignerProperties.IGNORED_ID).SetContent
                    ("Approval test signature.\nCreated by iText.");
            signerProperties.SetPageNumber(1).SetPageRect(new Rectangle(50, 650, 200, 100)).SetSignatureAppearance(
                appearance
            ).SetReason("Reason").SetLocation("Location");
            return signerProperties;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = System.Buffer.GetByte(str.ToCharArray(), 2 * i);
            }

            return bytes;
        }
    }
}
