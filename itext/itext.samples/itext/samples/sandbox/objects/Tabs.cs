using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
   
    // Tabs.cs
    //
    // Example showing different ways to use tabs in paragraphs.
    // Demonstrates leading tabs, inline tabs, and combined tab usage.
 
    public class Tabs
    {
        public static readonly string DEST = "results/sandbox/objects/tabs.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Tabs().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("Hello World.");
            doc.Add(p);

            p = new Paragraph();
            p.AddTabStops(new TabStop(60f, TabAlignment.LEFT));
            p.Add(new Tab());
            p.Add("Hello World with tab.");
            doc.Add(p);

            p = new Paragraph();
            p.AddTabStops(new TabStop(200f, TabAlignment.LEFT));
            p.Add(new Text("Hello World with"));
            p.Add(new Tab());
            p.Add(new Text("an inline tab."));
            doc.Add(p);

            p = new Paragraph();
            p.AddTabStops(new TabStop(60f, TabAlignment.LEFT));
            p.Add(new Tab());
            p.AddTabStops(new TabStop(200f, TabAlignment.LEFT));
            p.Add(new Text("Hello World with"));
            p.Add(new Tab());
            p.Add(new Text("an inline tab."));
            doc.Add(p);

            doc.Close();
        }
    }
}