using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter02 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C02E05_JekyllHydeV1 {
        public const String SRC = "../../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../../results/chapter02/jekyll_hyde_v1.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E05_JekyllHydeV1().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Initialize document
            Document document = new Document(pdf);
            using (StreamReader sr = File.OpenText(SRC))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    document.Add(new Paragraph(line));
                }
            }

            document.Close();
        }
    }
}
