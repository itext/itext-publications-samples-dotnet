using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Kernel.Crypto;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    /// <summary>Basic example of the signature appearance customizing during the document signing
    /// using the SignatureFieldAppearance class and layers.</summary>
    public class SignatureAppearanceLayersExample
    {
        private static char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly String DEST =
            "results/sandbox/signatures/appearance/signatureAppearanceLayersExample.pdf";

        public static readonly String SRC = "../../../resources/pdfs/signExample.pdf";

        private static readonly String CERT_PATH = "../../../resources/cert/sign.pem";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SignatureAppearanceLayersExample().SignDocumentSignature(DEST);
        }

        protected void SignDocumentSignature(String filePath)
        {
            var privateKey = GetPrivateKey();
            var chain = GetCertificateChain();

            PdfSigner signer = new PdfSigner(new PdfReader(SRC), FileUtil.GetFileOutputStream(filePath),
                new StampingProperties());

            SignerProperties signerProperties = new SignerProperties()
                .SetFieldName("signature")
                .SetPageRect(new Rectangle(250, 500, 100, 100))
                .SetReason("Test 1")
                .SetLocation("TestCity");
            signer.SetSignerProperties(signerProperties);

            Rectangle rectangle = new Rectangle(0, 0, 100, 100);

            PdfFormXObject layer0 = new PdfFormXObject(rectangle);
            // Draw pink rectangle with blue border
            new PdfCanvas(layer0, signer.GetDocument())
                .SaveState()
                .SetFillColor(ColorConstants.PINK)
                .SetStrokeColor(ColorConstants.BLUE)
                .Rectangle(0, 0, 100, 100)
                .FillStroke()
                .RestoreState();

            PdfFormXObject layer2 = new PdfFormXObject(rectangle);
            // Draw yellow circle with gray border
            new PdfCanvas(layer2, signer.GetDocument())
                .SaveState()
                .SetFillColor(ColorConstants.YELLOW)
                .SetStrokeColor(ColorConstants.DARK_GRAY)
                .Circle(50, 50, 50)
                .FillStroke()
                .RestoreState();

            signer.GetSignatureField().SetBackgroundLayer(layer0).SetSignatureAppearanceLayer(layer2);

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