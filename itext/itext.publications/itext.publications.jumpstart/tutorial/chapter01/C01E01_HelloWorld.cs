/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter01 {
    /// <summary>Simple Hello World example.</summary>
    public class C01E01_HelloWorld {
        public const String DEST = "../../results/chapter01/hello_world.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E01_HelloWorld().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);
            //Add paragraph to the document
            document.Add(new Paragraph("Hello World!"));
            //Close document
            document.Close();
        }
    }
}
