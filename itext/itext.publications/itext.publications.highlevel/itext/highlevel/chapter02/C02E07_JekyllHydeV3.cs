/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter02 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C02E07_JekyllHydeV3 {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../results/chapter02/jekyll_hyde_v3.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E07_JekyllHydeV3().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
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
            //Close document
            document.Close();
        }
    }
}
