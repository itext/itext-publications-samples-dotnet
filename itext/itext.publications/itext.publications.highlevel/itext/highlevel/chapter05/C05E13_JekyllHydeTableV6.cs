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

namespace iText.Highlevel.Chapter05
{
    public class C05E13_JekyllHydeTableV6
    {
        public const String SRC = "../../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../../results/chapter05/jekyll_hyde_table6" + ".pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E13_JekyllHydeTableV6().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest)
        {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            IList<String> header = resultSet[0];
            resultSet.RemoveAt(0);
            Table table = new Table(UnitValue.CreatePercentArray(new float[] {3, 2, 14, 9, 4, 3}));
            table.SetWidth(UnitValue.CreatePercentValue(100));
            int nRows = resultSet.Count;
            table.SetNextRenderer(new AlternatingBackgroundTableRenderer(table, new Table.RowRange(0, nRows - 1)));
            foreach (String field in header)
            {
                table.AddHeaderCell(field);
            }

            foreach (IList<String> record in resultSet)
            {
                foreach (String field_1 in record)
                {
                    table.AddCell(field_1);
                }
            }

            document.Add(table);
            document.Close();
        }


        private class AlternatingBackgroundTableRenderer : TableRenderer
        {
            private bool isOdd = true;

            public AlternatingBackgroundTableRenderer(Table modelElement, Table.RowRange rowRange)
                : base(modelElement, rowRange)
            {
            }

            public AlternatingBackgroundTableRenderer(Table modelElement)
                : base(modelElement)
            {
            }

            public override IRenderer GetNextRenderer()
            {
                return new AlternatingBackgroundTableRenderer((Table) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                for (int i = 0; i < rows.Count && null != rows[i] && null != rows[i][0]; i++)
                {
                    CellRenderer[] renderers = rows[i];
                    Rectangle leftCell = renderers[0].GetOccupiedAreaBBox();
                    Rectangle rightCell = renderers[renderers.Length - 1].GetOccupiedAreaBBox();
                    Rectangle rect = new Rectangle(leftCell.GetLeft(), leftCell.GetBottom(),
                        rightCell.GetRight() - leftCell.GetLeft
                            (), leftCell.GetHeight());
                    PdfCanvas canvas = drawContext.GetCanvas();
                    canvas.SaveState();
                    if (isOdd)
                    {
                        canvas.SetFillColor(ColorConstants.LIGHT_GRAY);
                        isOdd = false;
                    }
                    else
                    {
                        canvas.SetFillColor(ColorConstants.YELLOW);
                        isOdd = true;
                    }

                    canvas.Rectangle(rect);
                    canvas.Fill();
                    canvas.RestoreState();
                }

                base.Draw(drawContext);
            }
        }
    }
}