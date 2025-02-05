using System;
using System.IO;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Kernel.Crypto;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    /// <summary>Basic example of the signature appearance customizing during the document signing
    /// using the SignatureFieldAppearance class.</summary>
    public class SignatureAppearanceExample
    {
        public static readonly String DEST = "results/sandbox/signatures/appearance/signatureAppearanceExample.pdf";
        public static readonly String SRC = "../../../resources/pdfs/signExample.pdf";

        private static readonly string CERT_PATH = "../../../resources/cert/sign.pem";
        private static readonly string IMAGE_PATH = "../../../resources/img/itext.png";
        private static readonly string BACKGROUND_PATH = "../../../resources/img/sign.jpg";
        
        private static char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SignatureAppearanceExample().SignDocument();
        }

        protected void SignDocument()
        {
            var privateKey = PemFileHelper.ReadFirstKey(CERT_PATH, PASSWORD);
            var chain = PemFileHelper.ReadFirstChain(CERT_PATH);

            PdfSigner signer = new PdfSigner(new PdfReader(SRC),
                FileUtil.GetFileOutputStream(SignatureAppearanceExample.DEST),
                new StampingProperties());
            
            ImageData clientSignatureImage = ImageDataFactory.Create(IMAGE_PATH);
            ImageData backgroundImage = ImageDataFactory.Create(BACKGROUND_PATH);
            
            // Set up the appearance
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID)
                // Use any of the setContent overloads to specify the content. If new SignedAppearanceText() is used,
                // it will be filled in automatically during the signing, but could be also filled in manually using
                // setters, e.g. new SignedAppearanceText().SetReasonLine("[Reason]: TestReason").SetLocationLine(null)
                .SetContent("Signature appearance description", clientSignatureImage)
                .SetBackgroundImage(ApplyBackgroundImage(backgroundImage, -1))
                .SetBorder(new SolidBorder(new DeviceRgb(1, 92, 135), 3))
                .SetFontColor(new DeviceRgb(252, 173, 47))
                .SetFontSize(30);
            
            // Use setFont in order to specify the font, e.g. appearance.setFont(PdfFontFactory.createFont());
            appearance.SetFontFamily(StandardFontFamilies.HELVETICA)
                .SetProperty(Property.FONT_PROVIDER, new BasicFontProvider());
            
            // Set signer properties
            SignerProperties signerProperties = new SignerProperties()
                .SetFieldName("signature")
                .SetCertificationLevel(AccessPermissions.NO_CHANGES_PERMITTED)
                .SetClaimedSignDate(DateTimeUtil.GetCurrentTime())
                .SetPageNumber(1)
                .SetPageRect(new Rectangle(50, 450, 200, 200))
                .SetContact("Contact")
                .SetSignatureCreator("Creator")
                .SetLocation("TestCity")
                .SetReason("TestReason")
                .SetSignatureAppearance(appearance);
            
            signer.SetSignerProperties(signerProperties);

            IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);
            signer.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 
                0, PdfSigner.CryptoStandard.CMS);
        }

        private BackgroundImage ApplyBackgroundImage(ImageData image, float imageScale)
        {
            BackgroundRepeat repeat = new BackgroundRepeat(BackgroundRepeat.BackgroundRepeatValue.NO_REPEAT);
            BackgroundPosition position = new BackgroundPosition()
                .SetPositionX(BackgroundPosition.PositionX.CENTER)
                .SetPositionY(BackgroundPosition.PositionY.CENTER);
            BackgroundSize size = new BackgroundSize();
            float EPS = 1e-5f;
            if (Math.Abs(imageScale) < EPS)
            {
                size.SetBackgroundSizeToValues(UnitValue.CreatePercentValue(100),
                    UnitValue.CreatePercentValue(100));
            }
            else
            {
                if (imageScale < 0)
                {
                    size.SetBackgroundSizeToContain();
                }
                else
                {
                    size.SetBackgroundSizeToValues(
                        UnitValue.CreatePointValue(imageScale * image.GetWidth()),
                        UnitValue.CreatePointValue(imageScale * image.GetHeight()));
                }
            }

            return new BackgroundImage.Builder()
                .SetImage(new PdfImageXObject(image))
                .SetBackgroundSize(size)
                .SetBackgroundRepeat(repeat)
                .SetBackgroundPosition(position)
                .Build();
        }
    }
}