using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class PageLabels
    {
        public static readonly string DEST = "results/sandbox/objects/page_labels.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PageLabels().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfViewerPreferences viewerPreferences = new PdfViewerPreferences();
            viewerPreferences.SetPrintScaling(PdfViewerPreferences.PdfViewerPreferencesConstants.NONE);
            pdfDoc.GetCatalog().SetPageMode(PdfName.UseThumbs);
            pdfDoc.GetCatalog().SetPageLayout(PdfName.TwoPageLeft);
            pdfDoc.GetCatalog().SetViewerPreferences(viewerPreferences);

            doc.Add(new Paragraph("Hello World"));
            doc.Add(new Paragraph("Hello People"));

            doc.Add(new AreaBreak());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            
            // Add the text to the direct content, but not in the right order
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(2));
            canvas.BeginText();
            canvas.SetFontAndSize(font, 12);
            canvas.MoveText(88.66f, 788);
            canvas.ShowText("ld");
            canvas.MoveText(-22f, 0);
            canvas.ShowText("Wor");
            canvas.MoveText(-15.33f, 0);
            canvas.ShowText("llo");
            canvas.MoveText(-15.33f, 0);
            canvas.ShowText("He");
            canvas.EndText();
            PdfFormXObject formXObject = new PdfFormXObject(new Rectangle(250, 25));
            new PdfCanvas(formXObject, pdfDoc)
                .BeginText()
                .SetFontAndSize(font, 12)
                .MoveText(0, 7)
                .ShowText("Hello People")
                .EndText();
            canvas.AddXObjectAt(formXObject, 36, 763);

            pdfDoc.SetDefaultPageSize(new PageSize(PageSize.A4).Rotate());
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World"));

            pdfDoc.SetDefaultPageSize(new PageSize(842, 595));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World"));

            pdfDoc.SetDefaultPageSize(PageSize.A4);
            doc.Add(new AreaBreak());
            pdfDoc.GetLastPage().SetCropBox(new Rectangle(10, 70, 525, 755));
            doc.Add(new Paragraph("Hello World"));

            doc.Add(new AreaBreak());
            pdfDoc.GetLastPage().GetPdfObject().Put(PdfName.UserUnit, new PdfNumber(5));
            doc.Add(new Paragraph("Hello World"));

            doc.Add(new AreaBreak());
            pdfDoc.GetLastPage().SetArtBox(new Rectangle(36, 36, 523, 770));
            Paragraph p = new Paragraph("Hello ")
                .Add(new Link("World", PdfAction.CreateURI("http://maps.google.com")));
            doc.Add(p);
            PdfAnnotation a = new PdfTextAnnotation(
                    new Rectangle(36, 755, 30, 30))
                .SetTitle(new PdfString("Example"))
                .SetContents("This is a post-it annotation");
            pdfDoc.GetLastPage().AddAnnotation(a);


            pdfDoc.GetPage(1).SetPageLabel(PageLabelNumberingStyle.UPPERCASE_LETTERS, null);
            pdfDoc.GetPage(3).SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, null);
            pdfDoc.GetPage(4).SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, "Custom-", 2);

            doc.Close();
        }
    }
}