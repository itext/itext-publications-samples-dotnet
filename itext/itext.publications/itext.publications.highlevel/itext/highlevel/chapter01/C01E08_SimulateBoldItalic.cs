using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C01E08_SimulateBoldItalic {
        public const String DEST = "../../../results/chapter01/simulate_bold_italic.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E08_SimulateBoldItalic().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            // Add content
            // We don't suggest usage of SimulateItalic() method to reach text obliquity since the result is written with the usual
            // rather than the italic font: we only emulate "obliquity". It's recommended to use an actual italic font instead.
            Text title1 = new Text("The Strange Case of ").SimulateItalic();
            // We don't suggest usage of SimulateBold() method to reach text thickness since the result is written with the usual
            // rather than the bold font: we only emulate "thickness". It's recommended to use an actual bold font instead.
            Text title2 = new Text("Dr. Jekyll and Mr. Hyde").SimulateBold();
            Text author = new Text("Robert Louis Stevenson").SimulateItalic().SimulateBold();
            Paragraph p = new Paragraph().Add(title1).Add(title2).Add(" by ").Add(author);
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
