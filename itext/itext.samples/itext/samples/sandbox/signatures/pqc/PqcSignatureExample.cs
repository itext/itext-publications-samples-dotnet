using System;
using System.IO;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using NUnit.Framework;

namespace iText.Signatures.Pqc {
    //import com.itextpdf.samples.sandbox.signatures.utils.PemFileHelper;
    //import org.junit.jupiter.api.Assertions;
    /// <summary>Example of document signing using specified post-quantum algorithms.</summary>
    /// <remarks>
    /// Example of document signing using specified post-quantum algorithms.
    /// <para />
    /// See inherited classed for specific PQC algorithms samples. This class uses abstract methods as placeholders.
    /// </remarks>
    public abstract class PqcSignatureExample {
        public const String SRC = "../../../resources/pdfs/signExample.pdf";

        private static readonly IBouncyCastleFactory BOUNCY_CASTLE_FACTORY = BouncyCastleFactoryCreator.GetFactory
            ();

        private const String SIGNATURE_FIELD = "Signature";

        private static readonly char[] KEY_PASSPHRASE = "testpassphrase".ToCharArray();

        protected internal virtual void ManipulatePdf() {
            String dest = GetDestination();
            FileInfo file = new FileInfo(dest);
            file.Directory.Create();
            DoSign(dest);
            DoVerify(dest, GetSignatureAlgoOid());
        }

        /// <summary>
        /// Performs PDF document signing using provided certificate
        /// <see cref="GetCertPath()"/>
        /// ,
        /// signature algorithm name
        /// <see cref="GetSignatureAlgo()"/>
        /// and
        /// digest algorithm name
        /// <see cref="GetDigestAlgo()"/>.
        /// </summary>
        /// <remarks>
        /// Performs PDF document signing using provided certificate
        /// <see cref="GetCertPath()"/>
        /// ,
        /// signature algorithm name
        /// <see cref="GetSignatureAlgo()"/>
        /// and
        /// digest algorithm name
        /// <see cref="GetDigestAlgo()"/>
        /// . Also
        /// <see cref="GetEstimatedSize()"/>
        /// should be specified.
        /// <para />
        /// Note: for experimental (not standardised) algorithms BCPQC provider is required, see
        /// <see cref="GetProvider()"/>.
        /// </remarks>
        /// <param name="outFile">output PDF path</param>
        private void DoSign(String outFile) {
            IX509Certificate[] signChain = PemFileHelper.ReadFirstChain(GetCertPath());
            IPrivateKey signPrivateKey = PemFileHelper.ReadFirstKey(GetCertPath(), KEY_PASSPHRASE);
            IExternalSignature pks = new PrivateKeySignature(signPrivateKey, GetDigestAlgo(), GetSignatureAlgo(), null);
            using (Stream @out = FileUtil.GetFileOutputStream(outFile)) {
                PdfSigner signer = new PdfSigner(new PdfReader(SRC), @out, new StampingProperties());
                SignerProperties signerProperties = GetSignerProperties();
                signer.SetSignerProperties(signerProperties);
                signer.SignDetached(new BouncyCastleDigest(), pks, signChain, null, null, null, GetEstimatedSize(),
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        private static void DoVerify(String fileName, String expectedSigAlgoIdentifier) {
            using (PdfReader r = new PdfReader(fileName)) {
                using (PdfDocument pdfDoc = new PdfDocument(r)) {
                    SignatureUtil u = new SignatureUtil(pdfDoc);
                    PdfPKCS7 data = u.ReadSignatureData(SIGNATURE_FIELD);
                    Assert.IsTrue(data.VerifySignatureIntegrityAndAuthenticity());
                    if (expectedSigAlgoIdentifier != null) {
                        Assert.AreEqual(expectedSigAlgoIdentifier, data.GetSignatureMechanismOid());
                    }
                }
            }
        }

        private static SignerProperties GetSignerProperties() {
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID).SetContent
                ("Approval test signature.\nCreated by iText.");
            return new SignerProperties().SetFieldName(SIGNATURE_FIELD).SetPageRect(new Rectangle(50, 650, 200, 100)).
                SetReason("Test").SetLocation("TestCity").SetSignatureAppearance(appearance);
        }

        public abstract String GetDestination();

        public abstract String GetSignatureAlgoOid();

        public abstract String GetCertPath();

        public abstract String GetSignatureAlgo();

        public abstract String GetDigestAlgo();

        public abstract int GetEstimatedSize();
    }
}
