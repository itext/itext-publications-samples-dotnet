/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
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

namespace iText.Highlevel.Notused {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class JekyllHydeV7 {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../results/chapter02/jekyll_hyde_v7.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new JekyllHydeV7().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4);
            //Set column parameters
            float offSet = 27;
            float gutter = 18;
            float columnWidth = (PageSize.A4.GetWidth() - offSet * 2) / 3 - gutter;
            float columnHeight = PageSize.A4.GetHeight() - offSet * 2;
            //Define column areas
            Rectangle[] columns = new Rectangle[] { new Rectangle(offSet + gutter * 0.5f, offSet, columnWidth, columnHeight
                ), new Rectangle(offSet + columnWidth + gutter * 1.5f, offSet, columnWidth, columnHeight), new Rectangle
                (offSet + columnWidth * 2 + gutter * 2.5f, offSet, columnWidth, columnHeight) };
            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            StreamReader sr = File.OpenText(SRC);
            bool chapter = false;
            Div div = new Div();
            AreaBreak areaBreak = new AreaBreak(AreaBreakType.NEXT_PAGE);
            String line;
            while ((line = sr.ReadLine()) != null) {
                div = new Div().SetFont(font).SetFontSize(11).SetMarginBottom(18);
                div.Add(new Paragraph(line).SetFont(bold).SetFontSize(12).SetMarginBottom(0));
                while ((line = sr.ReadLine()) != null) {
                    div.Add(new Paragraph(line).SetMarginBottom(0).SetFirstLineIndent(36));
                    if (String.IsNullOrEmpty(line)) {
                        document.Add(div);
                        if (chapter) {
                            document.Add(areaBreak);
                        }
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
