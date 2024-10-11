using System;
using System.IO;
using System.Linq;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms.Fields.Properties;
using iText.Forms.Form.Element;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Crypto;
using Org.BouncyCastle.Crypto;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Signatures;
using NUnit.Framework;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class CreateSignatureAppearance
    {
        private static string RESOURCE_FOLDER = "../../../resources";
        private static string RESULT_FOLDER = "results/signatures/test/";
        private static string PATH = "/johndoe.p12";
        private static char[] PASS = "johndoe".ToCharArray();
        private static ICipherParameters pk;
        private static IX509Certificate[] chain;


        [OneTimeSetUp]
        public static void SetUpBeforeClass()
        {
            Directory.CreateDirectory(RESULT_FOLDER);
            var pkcs12Store = new Pkcs12StoreBuilder().Build();
            pkcs12Store.Load(new FileStream(RESOURCE_FOLDER + PATH, FileMode.Open, FileAccess.Read), PASS);
            var alias = "";
            var aliases = pkcs12Store.Aliases;
            while (alias.Equals("johndoe") == false && aliases.GetEnumerator().MoveNext())
            {
                alias = aliases.First();
            }

            pk = pkcs12Store.GetKey(alias).Key;

            var ce = pkcs12Store.GetCertificateChain(alias);
            chain = new IX509Certificate[ce.Length];

            for (var i = 0; i < ce.Length; i++)
            {
                chain[i] = new X509CertificateBC(ce[i].Certificate);
            }
        }

        [Test]
        public void TestModeDescription()
        {
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/test-DESCRIPTION.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());
                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestModeGraphic()
        {
            using (var imageResource =
                   new FileStream(RESOURCE_FOLDER + "/iText badge.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/test-GRAPHIC.pdf", FileMode.Create))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(data);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestModeGraphicAndDescription()
        {
            using (var imageResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-GRAPHIC_AND_DESCRIPTION.pdf", FileMode.Create))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(new SignedAppearanceText(),
                    data); // SignedAppearanceText will be filled in automatically
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestModeNameAndDescription()
        {
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/test-NAME_AND_DESCRIPTION.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent("",
                    new SignedAppearanceText()); // "" and SignedAppearanceText will be filled in automatically
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestReuseAppearance()
        {
            var emptySignatureFile = new FileStream(CreateEmptySignatureField(), FileMode.Open, FileAccess.Read);

            using (var reader = new PdfReader(emptySignatureFile))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/emptySignatureField-signed.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());
                SignerProperties signerProps = new SignerProperties()
                    .SetFieldName("Signature")
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);
                pdfSigner.GetSignatureField().SetReuseAppearance(true);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent("",
                    new SignedAppearanceText()); // "" and SignedAppearanceText will be filled in automatically
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        string CreateEmptySignatureField()
        {
            var outputPath = RESULT_FOLDER + "emptySignatureField.pdf";
            var emptySignatureFile = new FileStream(outputPath, FileMode.Create);
            using (var pdfDocument = new PdfDocument(new PdfWriter(emptySignatureFile)))
            {
                var field = new SignatureFormFieldBuilder(pdfDocument, "Signature")
                    .SetWidgetRectangle(new Rectangle(100, 600, 300, 100)).CreateSignature();

                CreateAppearance(field, pdfDocument);
                PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(field, pdfDocument.AddNewPage());
            }

            return outputPath;
        }

        void CreateAppearance(PdfSignatureFormField field, PdfDocument pdfDocument)
        {
            var widget = field.GetWidgets()[0];
            var rectangle = field.GetWidgets()[0].GetRectangle().ToRectangle();
            rectangle = new Rectangle(rectangle.GetWidth(), rectangle.GetHeight()); // necessary because of iText bug
            var xObject = new PdfFormXObject(rectangle);
            xObject.MakeIndirect(pdfDocument);
            var canvas = new PdfCanvas(xObject, pdfDocument);
            canvas.SetExtGState(new PdfExtGState().SetFillOpacity(.5f));
            using (var imageResource = new FileStream(RESOURCE_FOLDER + "/Binary - Light Gray.png", FileMode.Open,
                       FileAccess.Read))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));
                canvas.AddImageFittedIntoRectangle(data, rectangle, false);
            }

            widget.SetNormalAppearance(xObject.GetPdfObject());
        }

        [Test]
        public void TestSetImage()
        {
            using (var imageResource =
                   new FileStream(RESOURCE_FOLDER + "/Binary - Orange.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-setImage.pdf", FileMode.Create))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent("",
                    new SignedAppearanceText()); // "" and SignedAppearanceText will be filled in automatically
                var size = new BackgroundSize();
                size.SetBackgroundSizeToContain();
                appearance.SetBackgroundImage(new BackgroundImage.Builder()
                    .SetImage(new PdfImageXObject(data))
                    .SetBackgroundSize(size)
                    .Build());
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSetCaptions()
        {
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/test-setCaptions.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearanceText = new SignedAppearanceText();
                appearanceText.SetReasonLine("Objective: " + signerProps.GetReason());
                appearanceText.SetLocationLine("Whereabouts: " + signerProps.GetLocation());
                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(appearanceText);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSetFontStyle()
        {
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile = new FileStream(RESULT_FOLDER + "/test-SetFontStyle.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent("",
                    new SignedAppearanceText()); // "" and SignedAppearanceText will be filled in automatically
                appearance.SetFont(PdfFontFactory.CreateFont(StandardFonts.COURIER));
                appearance.SetFontColor(new DeviceRgb(0xF9, 0x9D, 0x25));
                appearance.SetFontSize(10);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSetDescriptionText()
        {
            using (var imageResource =
                   new FileStream(RESOURCE_FOLDER + "/iText logo.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-SetDescriptionText.pdf", FileMode.Create))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(new Rectangle(100, 500, 300, 100))
                    .SetPageNumber(1);

                var restriction =
                    "The qualified electronic signature at hand is restricted to present offers, invoices or credit notes to customers according to EU REGULATION No 910/2014 (23 July 2014) and German VAT law (§14 UStG).";
                signerProps.SetReason(restriction);
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(restriction, data);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestCustomLayer0()
        {
            using (var imageResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-CustomLayer0.pdf", FileMode.Create))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                var rectangle = new Rectangle(100, 500, 300, 100);
                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(rectangle)
                    .SetPageNumber(1)
                    .SetReason("Specimen")
                    .SetLocation("Boston");
                pdfSigner.SetSignerProperties(signerProps);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(new SignedAppearanceText(),
                    data); // SignedAppearanceText will be filled in automatically
                signerProps.SetSignatureAppearance(appearance);

                var backgroundLayer = new PdfFormXObject(rectangle);
                var canvas = new PdfCanvas(backgroundLayer, pdfSigner.GetDocument());
                canvas.SetStrokeColor(new DeviceRgb(0xF9, 0x9D, 0x25)).SetLineWidth(2);
                for (int i = (int)(rectangle.GetLeft() - rectangle.GetHeight()); i < rectangle.GetRight(); i += 5)
                    canvas.MoveTo(i, rectangle.GetBottom()).LineTo(i + rectangle.GetHeight(), rectangle.GetTop());
                canvas.Stroke();
                pdfSigner.GetSignatureField().SetBackgroundLayer(backgroundLayer);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestCustomLayer2()
        {
            using (var badgeResource =
                   new FileStream(RESOURCE_FOLDER + "/iText badge.png", FileMode.Open, FileAccess.Read))
            using (var signResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-CustomLayer2.pdf", FileMode.Create))
            {
                var badge = ImageDataFactory.Create(StreamUtil.InputStreamToArray(badgeResource));
                var sign = ImageDataFactory.Create(StreamUtil.InputStreamToArray(signResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                var rectangle = new Rectangle(100, 500, 300, 100);
                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(rectangle)
                    .SetPageNumber(1);
                pdfSigner.SetSignerProperties(signerProps);

                var foregroundLayer = new PdfFormXObject(rectangle);
                var canvas = new PdfCanvas(foregroundLayer, pdfSigner.GetDocument());

                var xCenter = rectangle.GetLeft() + rectangle.GetWidth() / 2;
                var yCenter = rectangle.GetBottom() + rectangle.GetHeight() / 2;

                var badgeWidth = rectangle.GetHeight() - 20;
                var badgeHeight = badgeWidth * badge.GetHeight() / badge.GetWidth();

                canvas.SetLineWidth(20)
                    .SetStrokeColorRgb(.9f, .1f, .1f)
                    .MoveTo(rectangle.GetLeft(), rectangle.GetBottom())
                    .LineTo(rectangle.GetRight(), rectangle.GetTop())
                    .MoveTo(xCenter + rectangle.GetHeight(), yCenter - rectangle.GetWidth())
                    .LineTo(xCenter - rectangle.GetHeight(), yCenter + rectangle.GetWidth())
                    .Stroke();

                sign.SetTransparency(new int[] { 0, 0 });
                canvas.AddImageFittedIntoRectangle(sign,
                    new Rectangle(0, yCenter, badgeWidth * sign.GetWidth() / sign.GetHeight() / 2, badgeWidth / 2),
                    false);

                canvas.ConcatMatrix(
                    AffineTransform.GetRotateInstance(Math.Atan2(rectangle.GetHeight(), rectangle.GetWidth()), xCenter,
                        yCenter));
                canvas.AddImageFittedIntoRectangle(badge,
                    new Rectangle(xCenter - badgeWidth / 2, yCenter - badgeHeight + badgeWidth / 2, badgeWidth,
                        badgeHeight), false);
                pdfSigner.GetSignatureField().SetSignatureAppearanceLayer(foregroundLayer);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestCustomLayers()
        {
            using (var badgeResource =
                   new FileStream(RESOURCE_FOLDER + "/iText badge.png", FileMode.Open, FileAccess.Read))
            using (var signResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-CustomLayers.pdf", FileMode.Create))
            {
                var badge = ImageDataFactory.Create(StreamUtil.InputStreamToArray(badgeResource));
                var sign = ImageDataFactory.Create(StreamUtil.InputStreamToArray(signResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                var rectangle = new Rectangle(100, 500, 300, 100);
                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(rectangle)
                    .SetPageNumber(1);
                pdfSigner.SetSignerProperties(signerProps);

                var backgroundLayer = new PdfFormXObject(rectangle);
                var canvas = new PdfCanvas(backgroundLayer, pdfSigner.GetDocument());
                canvas.SetStrokeColor(new DeviceRgb(0xF9, 0x9D, 0x25)).SetLineWidth(2);
                for (var i = (int)(rectangle.GetLeft() - rectangle.GetHeight()); i < rectangle.GetRight(); i += 5)
                    canvas.MoveTo(i, rectangle.GetBottom()).LineTo(i + rectangle.GetHeight(), rectangle.GetTop());
                canvas.Stroke();

                var foregroundLayer = new PdfFormXObject(rectangle);
                canvas = new PdfCanvas(foregroundLayer, pdfSigner.GetDocument());

                var xCenter = rectangle.GetLeft() + rectangle.GetWidth() / 2;
                var yCenter = rectangle.GetBottom() + rectangle.GetHeight() / 2;

                var badgeWidth = rectangle.GetHeight() - 20;
                var badgeHeight = badgeWidth * badge.GetHeight() / badge.GetWidth();

                canvas.SetLineWidth(20)
                    .SetStrokeColorRgb(.9f, .1f, .1f)
                    .MoveTo(rectangle.GetLeft(), rectangle.GetBottom())
                    .LineTo(rectangle.GetRight(), rectangle.GetTop())
                    .MoveTo(xCenter + rectangle.GetHeight(), yCenter - rectangle.GetWidth())
                    .LineTo(xCenter - rectangle.GetHeight(), yCenter + rectangle.GetWidth())
                    .Stroke();

                sign.SetTransparency(new int[] { 0, 0 });
                canvas.AddImageFittedIntoRectangle(sign,
                    new Rectangle(0, yCenter, badgeWidth * sign.GetWidth() / sign.GetHeight() / 2, badgeWidth / 2),
                    false);

                canvas.ConcatMatrix(
                    AffineTransform.GetRotateInstance(Math.Atan2(rectangle.GetHeight(), rectangle.GetWidth()), xCenter,
                        yCenter));
                canvas.AddImageFittedIntoRectangle(badge,
                    new Rectangle(xCenter - badgeWidth / 2, yCenter - badgeHeight + badgeWidth / 2, badgeWidth,
                        badgeHeight), false);

                pdfSigner.GetSignatureField().SetBackgroundLayer(backgroundLayer)
                    .SetSignatureAppearanceLayer(foregroundLayer);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestCustomLayer2OnReusedAppearance()
        {
            var emptySignatureFile = new FileStream(CreateEmptySignatureField(), FileMode.Open, FileAccess.Read);

            using (var badgeResource =
                   new FileStream(RESOURCE_FOLDER + "/iText badge.png", FileMode.Open, FileAccess.Read))
            using (var signResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(emptySignatureFile))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-CustomLayer2OnReusedAppearance.pdf", FileMode.Create))
            {
                var badge = ImageDataFactory.Create(StreamUtil.InputStreamToArray(badgeResource));
                var sign = ImageDataFactory.Create(StreamUtil.InputStreamToArray(signResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());
                pdfSigner.SetSignerProperties(new SignerProperties().SetFieldName("Signature"));

                var rectangle = pdfSigner.GetSignatureField().GetFirstFormAnnotation().GetWidget().GetRectangle()
                    .ToRectangle();

                var foregroundLayer = new PdfFormXObject(rectangle);
                var canvas = new PdfCanvas(foregroundLayer, pdfSigner.GetDocument());

                var xCenter = rectangle.GetLeft() + rectangle.GetWidth() / 2;
                var yCenter = rectangle.GetBottom() + rectangle.GetHeight() / 2;

                var badgeWidth = rectangle.GetHeight() - 20;
                var badgeHeight = badgeWidth * badge.GetHeight() / badge.GetWidth();

                canvas.SetLineWidth(20)
                    .SetStrokeColorRgb(.9f, .1f, .1f)
                    .MoveTo(rectangle.GetLeft(), rectangle.GetBottom())
                    .LineTo(rectangle.GetRight(), rectangle.GetTop())
                    .MoveTo(xCenter + rectangle.GetHeight(), yCenter - rectangle.GetWidth())
                    .LineTo(xCenter - rectangle.GetHeight(), yCenter + rectangle.GetWidth())
                    .Stroke();

                sign.SetTransparency(new int[] { 0, 0 });
                canvas.AddImageFittedIntoRectangle(sign,
                    new Rectangle(0, yCenter, badgeWidth * sign.GetWidth() / sign.GetHeight() / 2, badgeWidth / 2),
                    false);

                canvas.ConcatMatrix(
                    AffineTransform.GetRotateInstance(Math.Atan2(rectangle.GetHeight(), rectangle.GetWidth()), xCenter,
                        yCenter));
                canvas.AddImageFittedIntoRectangle(badge,
                    new Rectangle(xCenter - badgeWidth / 2, yCenter - badgeHeight + badgeWidth / 2, badgeWidth,
                        badgeHeight), false);

                pdfSigner.GetSignatureField().SetReuseAppearance(true).SetSignatureAppearanceLayer(foregroundLayer);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestMachineReadables()
        {
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-MachineReadables.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                SignerProperties signerProps = new SignerProperties()
                    .SetContact("Test content of Contact field")
                    .SetReason("Test content of Reason field")
                    .SetLocation("Test content of Location field")
                    .SetSignatureCreator("Test content of Signature Creator field");
                pdfSigner.SetSignerProperties(signerProps);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        /**
         * This test illustrates an issue in the {@link PdfSignatureAppearance#setReuseAppearance(boolean) ReuseAppearance}
         * feature of iText: Here the complete normal appearance of the unsigned field is re-used as n0 layer of the signed
         * field. Unfortunately it was forgotten that the original appearance was displayed so that its BBox transformed by
         * its matrix fits into the annotation rectangle. If BBox of the original appearance does not have its lower left
         * corner in the origin or its matrix is not the identity, therefore, the re-used appearance usually is displayed in
         * differently if at all.
         */
        [Test]
        public void TestReuseSpecialAppearance()
        {
            var emptySignatureFile = new FileStream(CreateSpecialEmptySignatureField(), FileMode.Open, FileAccess.Read);

            using (var reader = new PdfReader(emptySignatureFile))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/specialEmptySignatureField-signed.pdf", FileMode.Create))
            {
                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());
                SignerProperties signerProps = new SignerProperties().SetFieldName("Signature");
                pdfSigner.SetSignerProperties(signerProps);

                signerProps.SetReason("Specimen");
                signerProps.SetLocation("Boston");

                pdfSigner.GetSignatureField().SetReuseAppearance(true);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent("",
                    new SignedAppearanceText()); // "" and SignedAppearanceText will be filled in automatically
                appearance.SetFontColor(ColorConstants.LIGHT_GRAY);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }

        string CreateSpecialEmptySignatureField()
        {
            var outputPath = RESULT_FOLDER + "specialEmptySignatureField.pdf";
            var emptySignatureFile = new FileStream(outputPath, FileMode.Create);
            using (var pdfDocument = new PdfDocument(new PdfWriter(emptySignatureFile)))
            {
                var field = new SignatureFormFieldBuilder(pdfDocument, "Signature")
                    .SetWidgetRectangle(new Rectangle(100, 600, 300, 100)).CreateSignature();
                CreateSpecialAppearance(field, pdfDocument);
                PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(field, pdfDocument.AddNewPage());
            }

            return outputPath;
        }

        void CreateSpecialAppearance(PdfSignatureFormField field, PdfDocument pdfDocument)
        {
            var widget = field.GetWidgets()[0];
            var rectangle = field.GetWidgets()[0].GetRectangle().ToRectangle();
            rectangle = new Rectangle(-rectangle.GetWidth() / 4, -rectangle.GetHeight() / 4, rectangle.GetWidth(),
                rectangle.GetHeight());
            var xObject = new PdfFormXObject(rectangle);
            xObject.MakeIndirect(pdfDocument);
            var matrix = new float[6];
            AffineTransform.GetRotateInstance(Math.PI / 4).GetMatrix(matrix);
            xObject.GetPdfObject().Put(PdfName.Matrix, new PdfArray(matrix));
            var canvas = new PdfCanvas(xObject, pdfDocument);

            using (var imageResource =
                   new FileStream(RESOURCE_FOLDER + "/Binary - Light Gray.png", FileMode.Open, FileAccess.Read))
            {
                var data = ImageDataFactory.Create(StreamUtil.InputStreamToArray(imageResource));
                canvas.AddImageFittedIntoRectangle(data, rectangle, false);
            }

            canvas.SetFillColorGray(0);
            canvas.SetFontAndSize(PdfFontFactory.CreateFont(), rectangle.GetHeight() / 2);
            canvas.BeginText();
            canvas.ShowText("Test");
            canvas.EndText();
            widget.SetNormalAppearance(xObject.GetPdfObject());
        }

        [Test]
        public void TestSignInNewHierarchicalField()
        {
            Exception ex = NUnit.Framework.Assert.Catch(typeof(ArgumentException), () =>
            {
                using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
                using (var reader = new PdfReader(resource))
                using (var outputFile =
                       new FileStream(RESULT_FOLDER + "/test-SignInNewHierarchicalField.pdf", FileMode.Create))
                {
                    var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                    SignerProperties signerProps = new SignerProperties()
                        .SetFieldName("Form.Subform.Signature")
                        .SetPageRect(new Rectangle(100, 500, 300, 100))
                        .SetPageNumber(1)
                        .SetReason("Hierarchical Signature Field")
                        .SetLocation("Boston");
                    pdfSigner.SetSignerProperties(signerProps);

                    var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                    pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                        PdfSigner.CryptoStandard.CMS);
                }
            });
        }

        [Test]
        public void TestCustomAppearance()
        {
            using (var badgeResource =
                   new FileStream(RESOURCE_FOLDER + "/iText badge.png", FileMode.Open, FileAccess.Read))
            using (var signResource = new FileStream(RESOURCE_FOLDER + "/johnDoe.png", FileMode.Open, FileAccess.Read))
            using (var resource = new FileStream(RESOURCE_FOLDER + "/Blank.pdf", FileMode.Open, FileAccess.Read))
            using (var reader = new PdfReader(resource))
            using (var outputFile =
                   new FileStream(RESULT_FOLDER + "/test-CustomAppearance.pdf", FileMode.Create))
            {
                var badge = ImageDataFactory.Create(StreamUtil.InputStreamToArray(badgeResource));
                var sign = ImageDataFactory.Create(StreamUtil.InputStreamToArray(signResource));

                var pdfSigner = new PdfSigner(reader, outputFile, new StampingProperties());

                var rectangle = new Rectangle(100, 500, 300, 100);
                SignerProperties signerProps = new SignerProperties()
                    .SetPageRect(rectangle)
                    .SetPageNumber(1);
                pdfSigner.SetSignerProperties(signerProps);

                var paragraph = new Paragraph();

                var signImage = new Image(sign);
                signImage.SetAutoScale(true);
                paragraph.Add(signImage);
                var badgeImage = new Image(badge);
                badgeImage.SetRotationAngle(-Math.PI / 16);
                badgeImage.SetAutoScale(true);
                paragraph.Add(badgeImage);

                var div = new Div();
                div.Add(paragraph);

                var appearance = new SignatureFieldAppearance(SignerProperties.IGNORED_ID);
                appearance.SetContent(div);
                signerProps.SetSignatureAppearance(appearance);

                var pks = new PrivateKeySignature(new PrivateKeyBC(pk), DigestAlgorithms.SHA256);
                pdfSigner.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                    PdfSigner.CryptoStandard.CMS);
            }
        }
    }
}