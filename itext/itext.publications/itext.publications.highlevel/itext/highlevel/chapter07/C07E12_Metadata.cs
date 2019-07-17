/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C07E12_Metadata {
        public const String DEST = "../../results/chapter07/metadata.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E12_Metadata().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest, new WriterProperties().AddXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_6)));
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.SetTitle("The Strange Case of Dr. Jekyll and Mr. Hyde");
            info.SetAuthor("Robert Louis Stevenson");
            info.SetSubject("A novel");
            info.SetKeywords("Dr. Jekyll, Mr. Hyde");
            info.SetCreator("A simple tutorial example");
            Document document = new Document(pdf);
            document.Add(new Paragraph("Mr. Jekyl and Mr. Hyde"));
            document.Close();
        }
    }
}
