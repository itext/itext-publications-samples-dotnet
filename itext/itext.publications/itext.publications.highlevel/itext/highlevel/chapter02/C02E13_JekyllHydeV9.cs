using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter02 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C02E13_JekyllHydeV9 {
        public const String SRC = "../../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../../results/chapter02/jekyll_hyde_v9.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E13_JekyllHydeV9().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4, false);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3))
                .SetFont(font).SetFontSize(11);
            StreamReader sr = File.OpenText(SRC);
            String line;
            Paragraph p;
            bool title = true;
            while ((line = sr.ReadLine()) != null) {
                p = new Paragraph(line);
                p.SetKeepTogether(true);
                if (title) {
                    p.SetFont(bold).SetFontSize(12);
                    title = false;
                }
                else {
                    p.SetFirstLineIndent(36);
                }
                if (String.IsNullOrEmpty(line)) {
                    p.SetMarginBottom(12);
                    title = true;
                }
                else {
                    p.SetMarginBottom(0);
                }
                document.Add(p);
            }
            int n = pdf.GetNumberOfPages();
            Paragraph footer;
            for (int page = 1; page <= n; page++) {
                footer = new Paragraph(String.Format("Page {0} of {1}", page, n));
                document.ShowTextAligned(footer, 297.5f, 20, page, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 0);
            }
            //Close document
            document.Close();
        }
    }
}
