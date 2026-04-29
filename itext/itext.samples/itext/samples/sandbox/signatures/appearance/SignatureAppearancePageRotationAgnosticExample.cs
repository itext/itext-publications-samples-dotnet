using System;
using System.IO;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Crypto;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Appearance
{
    // SignatureAppearancePageRotationAgnosticExample.cs
    //
    // Creates PDF signatures using custom appearance layers and rotated input page.
    // Graphics are drawn as if page is not rotated and real rectangle is recalculated based on the rotation.
    public class SignatureAppearancePageRotationAgnosticExample
    {
        public static readonly String DEST =
            "results/sandbox/signatures/appearance/signatureAppearancePageRotationAgnosticExample.pdf";

        public static readonly String ROTATED_DOC = "results/sandbox/signatures/appearance/rotatedSource.pdf";
        public static readonly String CERT_PATH = "../../../resources/cert/sign.pem";

        private static char[] PASSWORD = "testpassphrase".ToCharArray();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(ROTATED_DOC)))
            {
                pdfDocument.AddNewPage().SetRotation(90);
                using (Document document = new Document(pdfDocument))
                {
                    document.Add(new Paragraph("Rotated 90 degrees"));
                }
            }

            new SignatureAppearancePageRotationAgnosticExample().SignDocumentSignature(DEST);
        }

        /// <summary>
        /// Signs the rotated document using custom appearance layers and
        /// additional logic to recalculate field rectangle and rotation based on page rotation.
        /// </summary>
        /// <param name="filePath">output path as String</param>
        protected void SignDocumentSignature(String filePath)
        {
            IPrivateKey privateKey = GetPrivateKey();
            IX509Certificate[] chain = GetCertificateChain();

            PdfSigner signer = new PdfSigner(new PdfReader(ROTATED_DOC), FileUtil.GetFileOutputStream(filePath),
                new StampingProperties());
            // Specify page number, at which signature is supposed to be drawn.
            int pageNumber = 1;
            PdfPage page = signer.GetDocument().GetPage(pageNumber);
            // We need to get page rotation to modify signature rectangle and set proper rotation.
            int pageRotation = page.GetRotation();

            // Here you want to specify the rectangle as if page is not rotated. So coordinates begin in left bottom corner.
            Rectangle originalSignatureRectangle = new Rectangle(250, 100, 100, 50);

            // With following transformations signature rectangle will look the same agnostic to page rotation.
            Rectangle transformedRectangle = GetTransformedRectangle(originalSignatureRectangle, page, pageRotation);

            SignerProperties signerProperties = new SignerProperties()
                .SetFieldName("signature")
                .SetPageRect(transformedRectangle)
                .SetReason("Test 1")
                .SetLocation("TestCity")
                .SetPageNumber(pageNumber);
            signer.SetSignerProperties(signerProperties);

            Rectangle rectangle = new Rectangle(0, 0, 100, 50);

            PdfFormXObject layer0 = new PdfFormXObject(rectangle);
            new PdfCanvas(layer0, signer.GetDocument())
                .SaveState()
                .SetFillColor(ColorConstants.PINK)
                .Rectangle(0, 0, 100, 50)
                .FillStroke()
                .RestoreState();

            PdfFormXObject layer2 = new PdfFormXObject(rectangle);
            new PdfCanvas(layer2, signer.GetDocument())
                .SaveState()
                .BeginText()
                .MoveText(10, 30)
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 8)
                .ShowText("Signature Appearance")
                .EndText()
                .RestoreState();

            signer.GetSignatureField().SetBackgroundLayer(layer0).SetSignatureAppearanceLayer(layer2);
            signer.GetSignatureField().GetFirstFormAnnotation().SetBorderWidth(5);
            // We need to set ignore page rotation to false first to draw signature as is on a page.
            signer.GetSignatureField().SetIgnorePageRotation(false);
            // Depending on page rotation, we need to rotate signature form field so that it always looks the same.
            // Field is rotated to the opposite direction, so we just need to set page rotation to field as is.
            signer.GetSignatureField().GetFirstFormAnnotation().SetRotation(pageRotation);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            // This method closes the underlying pdf document, so the instance
            // of PdfSigner cannot be used after this method call
            IExternalSignature pks = new PrivateKeySignature(privateKey, DigestAlgorithms.SHA256);
            signer.SignDetached(new BouncyCastleDigest(), pks, chain, null, null, null, 0,
                PdfSigner.CryptoStandard.CMS);
        }

        /// <summary>
        /// Calculates transformed signature rectangle based on page rotation.
        /// </summary>
        /// <param name="originalSignatureRectangle">original signature rectangle specified by a user</param>
        /// <param name="page">PdfPage on which signature is supposed to be drawn</param>
        /// <param name="pageRotation">rotation of the page</param>
        /// <returns>transformed signature rectangle</returns>
        protected Rectangle GetTransformedRectangle(Rectangle originalSignatureRectangle, PdfPage page,
            int pageRotation)
        {
            switch (pageRotation)
            {
                case 90:
                    return new Rectangle(
                        page.GetPageSize().GetWidth() - originalSignatureRectangle.GetY() -
                        originalSignatureRectangle.GetHeight(),
                        originalSignatureRectangle.GetX(),
                        originalSignatureRectangle.GetHeight(),
                        originalSignatureRectangle.GetWidth());
                case 180:
                    return new Rectangle(
                        page.GetPageSize().GetWidth() - originalSignatureRectangle.GetX() -
                        originalSignatureRectangle.GetWidth(),
                        page.GetPageSize().GetHeight() - originalSignatureRectangle.GetY() -
                        originalSignatureRectangle.GetHeight(),
                        originalSignatureRectangle.GetWidth(),
                        originalSignatureRectangle.GetHeight());
                case 270:
                    return new Rectangle(
                        originalSignatureRectangle.GetY(),
                        page.GetPageSize().GetHeight() - originalSignatureRectangle.GetX() -
                        originalSignatureRectangle.GetWidth(),
                        originalSignatureRectangle.GetHeight(),
                        originalSignatureRectangle.GetWidth());
                default:
                    return originalSignatureRectangle;
            }
        }

        /// <summary>
        /// Creates signing chain for the sample. This chain shouldn't be used for the real signing.
        /// </summary>
        /// <returns>the chain of certificates to be used for the signing operation.</returns>
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

        /// <summary>
        /// Creates private key for the sample. This key shouldn't be used for the real signing.
        /// </summary>
        /// <returns>PrivateKey instance to be used for the main signing operation.</returns>
        /// <exception cref="PdfException"></exception>
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