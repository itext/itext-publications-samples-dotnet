using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Fields.Properties;
using iText.Forms.Form.Element;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    /// <summary>Basic example of the signature appearance customizing during the document signing.</summary>
    public class PadesSignatureAppearanceExample
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String DEST = "results/sandbox/signatures/appearance/padesSignatureAppearanceExample.pdf";

        private static readonly String CHAIN = "../../../resources/cert/chain.pem";
        private static readonly String SIGN = "../../../resources/cert/sign.pem";
        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();
        
        public static readonly String IMAGE_PATH = "../../../resources/img/sign.jpg";
        public static readonly String BOLD = "../../../resources/font/OpenSans-Bold.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PadesSignatureAppearanceExample().SignSignature(SRC, DEST);
        }

        /// <summary>Basic example of the signature appearance customizing during the document signing.</summary>
        /// <param name="src">source file</param>
        /// <param name="dest">destination file</param>
        public void SignSignature(String src, String dest)
        {
            PdfPadesSigner padesSigner = new PdfPadesSigner(new PdfReader(
                FileUtil.GetInputStreamForFile(src)), FileUtil.GetFileOutputStream(dest));
            // We can pass the appearance through the signer properties.
            SignerProperties signerProperties = CreateSignerProperties();
            padesSigner.SignWithBaselineBProfile(signerProperties, GetCertificateChain(), GetPrivateKey());
        }

        /// <summary>
        /// Creates properties to be used in signing operations. Also creates the appearance that will be passed to the
        /// PDF signer through the signer properties.
        /// </summary>
        /// <returns>Signer properties to be used for main signing operation.</returns>
        protected SignerProperties CreateSignerProperties()
        {
            SignerProperties signerProperties = new SignerProperties().SetFieldName("Signature1");

            // Create the appearance instance and set the signature content to be shown and different appearance properties.
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(signerProperties.GetFieldName())
                .SetContent(new SignedAppearanceText().SetReasonLine("Customized reason: Reason")
                    .SetLocationLine("Customized location: Location"), ImageDataFactory.Create(IMAGE_PATH))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 2))
                .SetFont(PdfFontFactory.CreateFont(BOLD, PdfEncodings.IDENTITY_H));

            // Note that if SignedAppearanceText is set as a content, description text will be generated automatically, but
            // any `visual` values can be changed by using the appropriate setters. This won't affect the signature dictionary.

            // Set created signature appearance and other signer properties.
            signerProperties
                .SetSignatureAppearance(appearance)
                .SetPageNumber(1)
                .SetPageRect(new Rectangle(50, 650, 200, 100))
                .SetReason("Reason")
                .SetLocation("Location");
            return signerProperties;
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
    }
}