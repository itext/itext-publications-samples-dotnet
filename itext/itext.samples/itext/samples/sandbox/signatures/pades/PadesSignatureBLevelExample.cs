using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Pades
{
    /// <summary>Basic example of document signing with PaDES Baseline-B Profile.</summary>
    public class PadesSignatureBLevelExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String DEST = "results/sandbox/signatures/pades/padesSignatureLevelBTest.pdf";

        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PadesSignatureBLevelExample().SignSignatureWithBaselineBProfile(SRC, DEST);
        }

        /// <summary>Basic example of document signing with PaDES Baseline-B Profile.</summary>
        /// <param name="src">source file</param>
        /// <param name="dest">destination file</param>
        public void SignSignatureWithBaselineBProfile(String src, String dest)
        {
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(
                FileUtil.GetInputStreamForFile(src)), FileUtil.GetFileOutputStream(dest));
            // You could also set:
            // padesSigner.SetTemporaryDirectoryPath(destinationFolder);
            // padesSigner.SetExternalDigest()
            // padesSigner.SetIssuingCertificateRetriever()
            // padesSigner.SetTrustedCertificates()
            // padesSigner.SetEstimatedSize()
            // padesSigner.SetStampingProperties()

            SignerProperties signerProperties = CreateSignerProperties();

            // You could also pass IExternalSignature instance instead of privateKey.
            padesSigner.SignWithBaselineBProfile(signerProperties, GetCertificateChain(), GetPrivateKey());
        }

        /// <summary>Creates signing chain for the sample. This chain shouldn't be used for the real signing.</summary>
        /// <returns>The chain of certificates to be used for the signing operation.</returns>
        protected IX509Certificate[] GetCertificateChain()
        {
            try
            {
                return PemFileHelper.ReadFirstChain(CHAIN);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates private key for the sample. This key shouldn't be used for the real signing.</summary>
        /// <returns>IPrivateKey instance to be used for the main signing operation.</returns>
        protected IPrivateKey GetPrivateKey()
        {
            try
            {
                return PemFileHelper.ReadFirstKey(SIGN, PASSWORD);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates properties to be used in signing operations.</summary>
        /// <returns>Signer properties to be used for main signing operation.</returns>
        protected SignerProperties CreateSignerProperties()
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName("Signature1");
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID)
                .SetContent("Approval test signature.\nCreated by iText.");
            signerProperties
                .SetPageNumber(1)
                .SetPageRect(new Rectangle(50, 650, 200, 100))
                .SetSignatureAppearance(appearance)
                .SetReason("Reason")
                .SetLocation("Location");
            return signerProperties;
        }
    }
}