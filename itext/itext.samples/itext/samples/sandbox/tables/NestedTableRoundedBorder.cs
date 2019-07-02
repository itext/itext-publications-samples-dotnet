/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class NestedTableRoundedBorder
    {
        public static readonly String DEST = "../../results/sandbox/tables/nested_table_rounded_border.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTableRoundedBorder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // outer table
            Table outerTable = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // inner table 1
            Table innerTable = new Table(UnitValue.CreatePercentArray(new float[] {8, 12, 1, 4, 12}))
                .UseAllAvailableWidth();

            // first row
            // column 1
            Cell cell = new Cell().Add(new Paragraph("Record Ref:"));
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 2
            cell = new Cell().Add(new Paragraph("GN Staff"));
            cell.SetPaddingLeft(2);
            innerTable.AddCell(cell);

            // spacing
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 4
            cell = new Cell().Add(new Paragraph("Date: "));
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 5
            cell = new Cell().Add(new Paragraph("30/4/2015"));
            cell.SetPaddingLeft(2);
            innerTable.AddCell(cell);

            // spacing
            cell = new Cell(1, 5);
            cell.SetHeight(3);
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // second row
            // column 1
            cell = new Cell().Add(new Paragraph("Hospital:"));
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 2
            cell = new Cell().Add(new Paragraph("Derby Royal"));
            cell.SetPaddingLeft(2);
            innerTable.AddCell(cell);

            // spacing
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 4
            cell = new Cell().Add(new Paragraph("Ward: "));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetPaddingLeft(5);
            innerTable.AddCell(cell);

            // column 5
            cell = new Cell().Add(new Paragraph("21"));
            cell.SetPaddingLeft(2);
            innerTable.AddCell(cell);

            // spacing
            cell = new Cell(1, 5);
            cell.SetHeight(3);
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // first nested table
            cell = new Cell().Add(innerTable);
            cell.SetNextRenderer(new RoundedBorderCellRenderer(cell));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetPadding(8);
            outerTable.AddCell(cell);

            // inner table 2
            innerTable = new Table(UnitValue.CreatePercentArray(new float[] {3, 17, 1, 16}));
            innerTable.SetWidth(UnitValue.CreatePercentValue(100));

            // first row
            // column 1
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 2
            cell = new Cell().Add(new Paragraph("Name"));
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 3
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // column 4
            cell = new Cell().Add(new Paragraph("Signature: "));
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // spacing
            cell = new Cell(1, 4);
            cell.SetHeight(3);
            cell.SetBorder(Border.NO_BORDER);
            innerTable.AddCell(cell);

            // subsequent rows
            for (int i = 1; i < 4; i++)
            {
                // column 1
                cell = new Cell().Add(new Paragraph(string.Format("{0}:", i)));
                cell.SetBorder(Border.NO_BORDER);
                innerTable.AddCell(cell);

                // column 2
                cell = new Cell();
                innerTable.AddCell(cell);

                // column 3
                cell = new Cell();
                cell.SetBorder(Border.NO_BORDER);
                innerTable.AddCell(cell);

                // column 4
                cell = new Cell();
                innerTable.AddCell(cell);

                // spacing
                cell = new Cell(1, 4);
                cell.SetHeight(3);
                cell.SetBorder(Border.NO_BORDER);
                innerTable.AddCell(cell);
            }

            // second nested table
            cell = new Cell().Add(innerTable);
            cell.SetNextRenderer(new RoundedBorderCellRenderer(cell));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetPaddingLeft(8);
            cell.SetPaddingTop(8);
            cell.SetPaddingRight(8);
            cell.SetPaddingBottom(8);
            outerTable.AddCell(cell);

            // add the table
            doc.Add(outerTable);

            doc.Close();
        }

        private class RoundedBorderCellRenderer : CellRenderer
        {
            public RoundedBorderCellRenderer(Cell modelElement)
                : base(modelElement)
            {
            }

            public override void Draw(DrawContext drawContext)
            {
                drawContext.GetCanvas().RoundRectangle(GetOccupiedAreaBBox().GetX() + 1.5f,
                    GetOccupiedAreaBBox().GetY() + 1.5f, GetOccupiedAreaBBox().GetWidth() - 3,
                    GetOccupiedAreaBBox().GetHeight() - 3, 4);
                drawContext.GetCanvas().Stroke();
                base.Draw(drawContext);
            }
        }
    }
}