/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Chapter05 {
    /// <author>iText</author>
    public class C05E14_JekyllHydeTableV7 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../results/chapter05/jekyll_hyde_table7.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E14_JekyllHydeTableV7().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 14, 9, 4, 3 }))
                .UseAllAvailableWidth();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            IList<String> header = resultSet[0];
            resultSet.RemoveAt(0);
            foreach (String field in header) {
                table.AddHeaderCell(field);
            }
            foreach (IList<String> record in resultSet) {
                table.AddCell(record[0]);
                table.AddCell(record[1]);
                Cell cell = new Cell().Add(new Paragraph(record[2]));
                cell.SetNextRenderer(new C05E14_JekyllHydeTableV7.RunlengthRenderer(cell, record[5]));
                table.AddCell(cell);
                table.AddCell(record[3]);
                table.AddCell(record[4]);
                table.AddCell(record[5]);
            }
            document.Add(table);
            document.Close();
        }

        private class RunlengthRenderer : CellRenderer {
            private int runlength;

            public RunlengthRenderer(Cell modelElement, String duration)
                : base(modelElement) {
                if (String.IsNullOrEmpty(duration.Trim())) {
                    this.runlength = 0;
                }
                else {
                    this.runlength = System.Convert.ToInt32(duration);
                }
            }

            public override IRenderer GetNextRenderer() {
                return new C05E14_JekyllHydeTableV7.RunlengthRenderer(((Cell)this.GetModelElement()), this.runlength
                    .ToString());
            }

            public override void DrawBackground(DrawContext drawContext) {
                if (this.runlength == 0) {
                    return;
                }
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.SaveState();
                if (this.runlength < 90) {
                    canvas.SetFillColor(ColorConstants.GREEN);
                }
                else {
                    if (this.runlength > 240) {
                        this.runlength = 240;
                        canvas.SetFillColor(ColorConstants.RED);
                    }
                    else {
                        canvas.SetFillColor(ColorConstants.ORANGE);
                    }
                }
                Rectangle rect = this.GetOccupiedAreaBBox();
                canvas.Rectangle(rect.GetLeft(), rect.GetBottom(), rect.GetWidth() * this.runlength / 240, rect.GetHeight(
                    ));
                canvas.Fill();
                canvas.RestoreState();
                base.DrawBackground(drawContext);
            }
        }
    }
}
