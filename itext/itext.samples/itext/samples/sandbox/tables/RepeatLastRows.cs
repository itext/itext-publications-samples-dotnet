/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class RepeatLastRows
    {
        public static readonly string DEST = "results/sandbox/tables/repeat_last_rows.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RepeatLastRows().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetNextRenderer(new RepeatTableRenderer(table, new Table.RowRange(0, 113)));

            for (int i = 1; i < 115; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("row " + i)));
            }

            doc.Add(table);
            
            doc.Close();
        }

        private class RepeatTableRenderer : TableRenderer
        {
            public RepeatTableRenderer(Table modelElement, Table.RowRange rowRange)
                : base(modelElement, rowRange)
            {
            }

            protected RepeatTableRenderer(Table modelElement)
                : base(modelElement)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new RepeatTableRenderer((Table) modelElement);
            }

            protected override TableRenderer[] Split(int row)
            {
                RepeatTableRenderer splitRenderer = (RepeatTableRenderer) CreateSplitRenderer
                    (new Table.RowRange(rowRange.GetStartRow(), rowRange.GetStartRow() + row));
                splitRenderer.rows = rows.ToList().GetRange(0, row);

                RepeatTableRenderer overflowRenderer;

                if (rows.Count - row > 5)
                {
                    overflowRenderer = (RepeatTableRenderer) CreateOverflowRenderer(
                        new Table.RowRange(rowRange.GetStartRow() + row, rowRange.GetFinishRow()));
                    overflowRenderer.rows = rows.ToList().GetRange(row, rows.Count);
                }
                else
                {
                    overflowRenderer = (RepeatTableRenderer) CreateOverflowRenderer(
                        new Table.RowRange(rowRange.GetFinishRow() - 5, rowRange.GetFinishRow()));
                    overflowRenderer.rows = rows.ToList().GetRange(rows.Count - 5, rows.Count);
                }

                splitRenderer.occupiedArea = occupiedArea;

                return new TableRenderer[] {splitRenderer, overflowRenderer};
            }
        }
    }
}