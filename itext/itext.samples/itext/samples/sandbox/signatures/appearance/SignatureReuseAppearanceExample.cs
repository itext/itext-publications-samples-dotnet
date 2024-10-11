using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Exceptions;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Kernel.Crypto;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    /// <summary>Basic example of the signature appearance customizing during the document signing
    /// using the SignatureFieldAppearance class and ReuseAppearances.</summary>
    public class SignatureReuseAppearanceExample
    {
        private static char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly String DEST =
            "results/sandbox/signatures/appearance/signatureReuseAppearanceExample.pdf";

        public static readonly String SRC = "../../../resources/pdfs/unsignedSignatureField.pdf";

        private static readonly String CERT_PATH = "../../../resources/cert/sign.pem";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SignatureReuseAppearanceExample().SignDocument(DEST);
        }

        protected void SignDocument(string dest)
        {
            var privateKey = GetPrivateKey();
            var chain = GetCertificateChain();

            PdfSigner signer = new PdfSigner(new PdfReader(SRC), FileUtil.GetFileOutputStream(dest), new StampingProperties());

            //      Set signer properties
            SignerProperties signerProperties = new SignerProperties()
                .SetCertificationLevel(AccessPermissions.UNSPECIFIED)
                .SetFieldName("Signature1")
                .SetReason("Test 1")
                .SetLocation("TestCity")
                .SetSignatureAppearance(new SignatureFieldAppearance(SignerProperties.IGNORED_ID)
                    .SetContent("New appearance").SetFontColor(ColorConstants.GREEN));

            signer.SetSignerProperties(signerProperties);

            signer.GetSignatureField().SetReuseAppearance(true);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            // This method closes the underlying pdf document, so the instance
            // of PdfSigner cannot be used after this method call
            IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);
            signer.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 
                0, PdfSigner.CryptoStandard.CMS);
        }
        /**
         * Creates signing chain for the sample. This chain shouldn't be used for the real signing.
         *
         * @return the chain of certificates to be used for the signing operation.
         */
        protected IX509Certificate[] GetCertificateChain()
        {
            try
            {
                return PemFileHelper.ReadFirstChain(CERT_PATH);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /**
         * Creates private key for the sample. This key shouldn't be used for the real signing.
         *
         * @return {@link PrivateKey} instance to be used for the main signing operation.
         */
        protected IPrivateKey GetPrivateKey()
        {
            try
            {
                return PemFileHelper.ReadFirstKey(CERT_PATH, PASSWORD);
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }
    }
}