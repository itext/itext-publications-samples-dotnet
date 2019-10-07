/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Chapter02 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C02E11_JekyllHydeV7 {
        internal class MyColumnRenderer : DocumentRenderer {
            protected internal int nextAreaNumber;

            protected internal readonly Rectangle[] columns;

            protected internal int currentAreaNumber;

            protected internal ICollection<int> moveColumn = new HashSet<int>();

            public MyColumnRenderer(Document document, Rectangle[] columns)
                : base(document, false) {
                this.columns = columns;
            }

            protected override LayoutArea UpdateCurrentArea(LayoutResult overflowResult) {
                if (overflowResult != null && overflowResult.GetAreaBreak() != null && overflowResult.GetAreaBreak().GetAreaType
                    () != AreaBreakType.NEXT_AREA) {
                    this.nextAreaNumber = 0;
                }
                if (this.nextAreaNumber % this.columns.Length == 0) {
                    base.UpdateCurrentArea(overflowResult);
                }
                this.currentAreaNumber = this.nextAreaNumber + 1;
                return (this.currentArea = new RootLayoutArea(this.currentPageNumber, this.columns[this.nextAreaNumber++ % this
                    .columns.Length].Clone()));
            }

            protected override PageSize AddNewPage(PageSize customPageSize) {
                if (this.currentAreaNumber != this.nextAreaNumber && this.currentAreaNumber % this.columns.Length != 0) {
                    this.moveColumn.Add(this.currentPageNumber - 1);
                }
                return base.AddNewPage(customPageSize);
            }

            protected override void FlushSingleRenderer(IRenderer resultRenderer) {
                int pageNum = resultRenderer.GetOccupiedArea().GetPageNumber();
                if (this.moveColumn.Contains(pageNum)) {
                    resultRenderer.Move(this.columns[0].GetWidth() / 2, 0);
                }
                base.FlushSingleRenderer(resultRenderer);
            }
        }

        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../results/chapter02/jekyll_hyde_v7.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E11_JekyllHydeV7().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            //Set column parameters
            float offSet = 36;
            float gutter = 23;
            float columnWidth = (PageSize.A4.GetWidth() - offSet * 2) / 2 - gutter;
            float columnHeight = PageSize.A4.GetHeight() - offSet * 2;
            //Define column areas
            Rectangle[] columns = new Rectangle[] { new Rectangle(offSet, offSet, columnWidth, columnHeight), new Rectangle
                (offSet + columnWidth + gutter, offSet, columnWidth, columnHeight) };
            DocumentRenderer renderer = new C02E11_JekyllHydeV7.MyColumnRenderer(document, columns);
            document.SetRenderer(renderer);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetFont(font).SetHyphenation(new HyphenationConfig("en"
                , "uk", 3, 3));
            StreamReader sr = File.OpenText(SRC);
            String line;
            Paragraph p;
            bool title = true;
            AreaBreak nextPage = new AreaBreak(AreaBreakType.NEXT_PAGE);
            while ((line = sr.ReadLine()) != null) {
                p = new Paragraph(line);
                if (title) {
                    p.SetFont(bold).SetFontSize(12);
                    title = false;
                }
                else {
                    p.SetFirstLineIndent(36);
                }
                if (String.IsNullOrEmpty(line)) {
                    document.Add(nextPage);
                    title = true;
                }
                document.Add(p);
            }
            renderer.Flush();
            document.Close();
        }
    }
}
