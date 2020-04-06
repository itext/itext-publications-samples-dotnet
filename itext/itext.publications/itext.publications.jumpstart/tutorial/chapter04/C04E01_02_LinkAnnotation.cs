/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter04 {
    /// <summary>Simple link annotation example.</summary>
    public class C04E01_02_LinkAnnotation {
        public const String DEST = "../../../results/chapter04/link_annotation.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E01_02_LinkAnnotation().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Initialize document
            Document document = new Document(pdf);
            //Create link annotation
            PdfLinkAnnotation annotation = ((PdfLinkAnnotation)new PdfLinkAnnotation(new Rectangle(0, 0)).SetAction(PdfAction
                .CreateURI("http://itextpdf.com/")));
            Link link = new Link("here", annotation);
            Paragraph p = new Paragraph("The example of link annotation. Click ").Add(link.SetUnderline()).Add(" to learn more..."
                );
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
