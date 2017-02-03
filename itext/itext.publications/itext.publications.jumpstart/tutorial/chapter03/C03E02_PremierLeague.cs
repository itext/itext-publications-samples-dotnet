/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test.Attributes;

namespace Tutorial.Chapter03 {
    /// <summary>Simple table renderer example.</summary>
    [WrapToTest]
    public class C03E02_PremierLeague {
        public const String DATA = "../../resources/data/premier_league.csv";

        public const String DEST = "../../results/chapter03/premier_league.pdf";

        internal Color greenColor = new DeviceCmyk(0.78f, 0, 0.81f, 0.21f);

        internal Color yellowColor = new DeviceCmyk(0, 0, 0.76f, 0.01f);

        internal Color redColor = new DeviceCmyk(0, 0.76f, 0.86f, 0.01f);

        internal Color blueColor = new DeviceCmyk(0.28f, 0.11f, 0, 0);

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E02_PremierLeague().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PageSize ps = new PageSize(842, 680);
            // Initialize document
            Document document = new Document(pdf, ps);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1.5f, 7, 2, 2, 2, 2, 3, 4, 4, 2 }));
            table.SetWidthPercent(100).SetTextAlignment(TextAlignment.CENTER).SetHorizontalAlignment(HorizontalAlignment
                .CENTER);
            StreamReader sr = File.OpenText(DATA);
            String line = sr.ReadLine();
            Process(table, line, bold, true);
            while ((line = sr.ReadLine()) != null) {
                Process(table, line, font, false);
            }
            sr.Close();
            document.Add(table);
            //Close document
            document.Close();
        }

        public virtual void Process(Table table, String line, PdfFont font, bool isHeader) {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            int columnNumber = 0;
            while (tokenizer.HasMoreTokens()) {
                if (isHeader) {
                    Cell cell = new Cell().Add(new Paragraph(tokenizer.NextToken()));
                    cell.SetNextRenderer(new C03E02_PremierLeague.RoundedCornersCellRenderer(this, cell));
                    cell.SetPadding(5).SetBorder(null);
                    table.AddHeaderCell(cell);
                }
                else {
                    columnNumber++;
                    Cell cell = new Cell().Add(new Paragraph(tokenizer.NextToken()));
                    cell.SetFont(font).SetBorder(new SolidBorder(Color.BLACK, 0.5f));
                    switch (columnNumber) {
                        case 4: {
                            cell.SetBackgroundColor(greenColor);
                            break;
                        }

                        case 5: {
                            cell.SetBackgroundColor(yellowColor);
                            break;
                        }

                        case 6: {
                            cell.SetBackgroundColor(redColor);
                            break;
                        }

                        default: {
                            cell.SetBackgroundColor(blueColor);
                            break;
                        }
                    }
                    table.AddCell(cell);
                }
            }
        }

        private class RoundedCornersCellRenderer : CellRenderer {
            public RoundedCornersCellRenderer(C03E02_PremierLeague _enclosing, Cell modelElement)
                : base(modelElement) {
                this._enclosing = _enclosing;
            }

            public override void DrawBorder(DrawContext drawContext) {
                Rectangle rectangle = this.GetOccupiedAreaBBox();
                float llx = rectangle.GetX() + 1;
                float lly = rectangle.GetY() + 1;
                float urx = rectangle.GetX() + this.GetOccupiedAreaBBox().GetWidth() - 1;
                float ury = rectangle.GetY() + this.GetOccupiedAreaBBox().GetHeight() - 1;
                PdfCanvas canvas = drawContext.GetCanvas();
                float r = 4;
                float b = 0.4477f;
                canvas.MoveTo(llx, lly).LineTo(urx, lly).LineTo(urx, ury - r).CurveTo(urx, ury - r * b, urx - r * b, ury, 
                    urx - r, ury).LineTo(llx + r, ury).CurveTo(llx + r * b, ury, llx, ury - r * b, llx, ury - r).LineTo(llx
                    , lly).Stroke();
                base.DrawBorder(drawContext);
            }

            private readonly C03E02_PremierLeague _enclosing;
        }
    }
}
