using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tutorial.Chapter04 {
    /// <summary>Simple text markup annotation example.</summary>
    public class C04E01_04_TextMarkupAnnotation {
        public const String DEST = "../../../results/chapter04/textmarkup_annotation.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E01_04_TextMarkupAnnotation().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Initialize document
            Document document = new Document(pdf);
            Paragraph p = new Paragraph("The example of text markup annotation.");
            document.ShowTextAligned(p, 20, 795, 1, TextAlignment.LEFT, VerticalAlignment.MIDDLE, 0);
            //Create text markup annotation
            PdfAnnotation ann = PdfTextMarkupAnnotation.CreateHighLight(new Rectangle(105, 790, 64, 10), new float[] { 
                169, 790, 105, 790, 169, 800, 105, 800 }).SetColor(ColorConstants.YELLOW).SetTitle(new PdfString("Hello!")).SetContents
                (new PdfString("I'm a popup.")).SetTitle(new PdfString("iText")).SetRectangle(new PdfArray
                (new float[] { 100, 600, 200, 100 }));
            pdf.GetFirstPage().AddAnnotation(ann);
            //Close document
            document.Close();
        }
    }
}
