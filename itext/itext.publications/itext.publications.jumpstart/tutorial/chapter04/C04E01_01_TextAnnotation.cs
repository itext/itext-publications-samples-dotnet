/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter04 {
    /// <summary>Simple text annotation example.</summary>
    public class C04E01_01_TextAnnotation {
        public const String DEST = "../../results/chapter04/text_annotation.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E01_01_TextAnnotation().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Initialize document
            Document document = new Document(pdf);
            document.Add(new Paragraph("The example of text annotation."));
            //Create text annotation
            PdfAnnotation ann = new PdfTextAnnotation(new Rectangle(20, 800, 0, 0))
                .SetOpen(true)
                .SetColor(ColorConstants.GREEN)
                .SetTitle(new PdfString("iText"))
                .SetContents("With iText, you can truly take your documentation needs to the next level.");
            pdf.GetFirstPage().AddAnnotation(ann);
            //Close document
            document.Close();
        }
    }
}
