/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace Tutorial.Chapter04 {
    /// <summary>Simple line annotation example.</summary>
    public class C04E01_03_LineAnnotation {
        public const String DEST = "../../results/chapter04/line_annotation.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E01_03_LineAnnotation().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfPage page = pdf.AddNewPage();
            PdfArray lineEndings = new PdfArray();
            lineEndings.Add(new PdfName("Diamond"));
            lineEndings.Add(new PdfName("Diamond"));
            //Create line annotation with inside caption
            PdfAnnotation annotation = new PdfLineAnnotation(new Rectangle(0, 0), new float[] { 20, 790, page.GetPageSize
                ().GetWidth() - 20, 790 }).SetLineEndingStyles((lineEndings)).SetContentsAsCaption(true).SetTitle(new 
                PdfString("iText")).SetContents("The example of line annotation").SetColor(ColorConstants.BLUE);
            page.AddAnnotation(annotation);
            //Close document
            pdf.Close();
        }
    }
}
