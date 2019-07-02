/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class TableBorder
    {
        public static readonly string DEST = "../../results/sandbox/tables/tables_border.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableBorder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();

            for (int aw = 0; aw < 16; aw++)
            {
                table.AddCell(new Cell().Add(new Paragraph("hi")).SetBorder(Border.NO_BORDER));
            }

            // Notice that one should set renderer after table completion
            table.SetNextRenderer(new TableBorderRenderer(table));

            doc.Add(table);

            doc.Close();
        }

        private class TableBorderRenderer : TableRenderer
        {
            public TableBorderRenderer(Table modelElement)
                : base(modelElement)
            {
            }

            protected override void DrawBorders(DrawContext drawContext)
            {
                Rectangle rect = GetOccupiedAreaBBox();
                drawContext.GetCanvas()
                    .SaveState()
                    .Rectangle(rect.GetLeft(), rect.GetBottom(), rect.GetWidth(), rect.GetHeight())
                    .Stroke()
                    .RestoreState();
            }
        }
    }
}