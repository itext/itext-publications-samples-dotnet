using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_04_CreateEmptyField
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/ks";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly char[] PASSWORD = "password".ToCharArray();
        public const String SIGNAME = "Signature1";

        public static readonly String[] RESULT_FILES =
        {
            "hello_empty.pdf",
            "hello_empty2.pdf",
            "field_signed.pdf"
        };

        public void CreatePdf(String filename)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Hello World!"));

            // Create a signature form field
            PdfFormField field = PdfFormField.CreateSignature(pdfDoc,
                new Rectangle(72, 632, 200, 100));
            field.SetFieldName(SIGNAME);
            field.SetPage(1);

            // Set the widget properties
            field.GetWidgets()[0].SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT).SetFlags(PdfAnnotation.PRINT);

            PdfDictionary mkDictionary = field.GetWidgets()[0].GetAppearanceCharacteristics();
            if (null == mkDictionary)
            {
                mkDictionary = new PdfDictionary();
            }

            PdfArray black = new PdfArray();
            black.Add(new PdfNumber(ColorConstants.BLACK.GetColorValue()[0]));
            black.Add(new PdfNumber(ColorConstants.BLACK.GetColorValue()[1]));
            black.Add(new PdfNumber(ColorConstants.BLACK.GetColorValue()[2]));
            mkDictionary.Put(PdfName.BC, black);

            PdfArray white = new PdfArray();
            white.Add(new PdfNumber(ColorConstants.WHITE.GetColorValue()[0]));
            white.Add(new PdfNumber(ColorConstants.WHITE.GetColorValue()[1]));
            white.Add(new PdfNumber(ColorConstants.WHITE.GetColorValue()[2]));
            mkDictionary.Put(PdfName.BG, white);

            field.GetWidgets()[0].SetAppearanceCharacteristics(mkDictionary);

            PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(field);

            Rectangle rect = new Rectangle(0, 0, 200, 100);
            PdfFormXObject xObject = new PdfFormXObject(rect);
            PdfCanvas canvas = new PdfCanvas(xObject, pdfDoc);
            canvas
                .SetStrokeColor(ColorConstants.BLUE)
                .SetFillColor(ColorConstants.LIGHT_GRAY)
                .Rectangle(0 + 0.5, 0 + 0.5, 200 - 0.5, 100 - 0.5)
                .FillStroke()
                .SetFillColor(ColorConstants.BLUE);
            new Canvas(canvas, rect).ShowTextAligned("SIGN HERE", 100, 50,
                TextAlignment.CENTER, (float) (Math.PI / 180) * 25);

            // Note that Acrobat doesn't show normal appearance in the highlight mode.
            field.GetWidgets()[0].SetNormalAppearance(xObject.GetPdfObject());

            doc.Close();
        }

        public void AddField(String src, String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));

            // Create a signature form field
            PdfSignatureFormField field = PdfFormField.CreateSignature(pdfDoc,
                new Rectangle(72, 632, 200, 100));
            field.SetFieldName(SIGNAME);

            field.GetWidgets()[0].SetHighlightMode(PdfAnnotation.HIGHLIGHT_OUTLINE).SetFlags(PdfAnnotation.PRINT);

            PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(field);

            pdfDoc.Close();
        }

        public void Sign(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            signer.GetSignatureAppearance()
                .SetReason(reason)
                .SetLocation(location);
            signer.SetFieldName(name);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            C2_04_CreateEmptyField appCreate = new C2_04_CreateEmptyField();
            appCreate.CreatePdf(DEST + RESULT_FILES[0]);
            appCreate.AddField(SRC, DEST + RESULT_FILES[1]);

            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            new C2_04_CreateEmptyField().Sign(DEST + RESULT_FILES[0], SIGNAME, DEST + RESULT_FILES[2],
                chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Test", "Ghent");
        }
    }
}