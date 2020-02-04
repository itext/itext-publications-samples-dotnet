/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;

using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class JekyllHydeV5 {
        public const String SRC = "../../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../../results/chapter02/jekyll_hyde_v5.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new JekyllHydeV5().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            StreamReader sr = File.OpenText(SRC);
            LineSeparator separator = new LineSeparator(new DottedLine(2f, 5f));
            separator.SetMarginLeft(10);
            separator.SetMarginRight(10);
            bool chapter = false;
            Div div = new Div();
            String line;
            while ((line = sr.ReadLine()) != null) {
                div = new Div().SetFont(font).SetFontSize(11).SetMarginBottom(18);
                div.Add(new Paragraph(line).SetFont(bold).SetFontSize(12).SetMarginBottom(0));
                while ((line = sr.ReadLine()) != null) {
                    div.Add(new Paragraph(line).SetMarginBottom(0).SetFirstLineIndent(36));
                    if (String.IsNullOrEmpty(line)) {
                        if (chapter) {
                            div.Add(separator);
                        }
                        document.Add(div);
                        div = new Div();
                        chapter = true;
                        break;
                    }
                }
            }
            document.Add(div);
            //Close document
            document.Close();
        }
    }
}
