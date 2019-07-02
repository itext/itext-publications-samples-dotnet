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
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class FitTableOnPage
    {
        public static readonly string DEST = "../../results/sandbox/tables/fit_table_on_page.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FitTableOnPage().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            for (int i = 0; i < 10; i++)
            {
                Cell cell;

                if (i == 9)
                {
                    cell = new Cell().Add(new Paragraph("Two\nLines"));
                }
                else
                {
                    cell = new Cell().Add(new Paragraph(i.ToString()));
                }

                table.AddCell(cell);
            }

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            IRenderer tableRenderer = table.CreateRendererSubTree().SetParent(doc.GetRenderer());
            LayoutResult tableLayoutResult = tableRenderer.Layout(new LayoutContext(
                new LayoutArea(0, new Rectangle(550 + 72, 1000))));

            pdfDoc.SetDefaultPageSize(new PageSize(550 + 72,
                tableLayoutResult.GetOccupiedArea().GetBBox().GetHeight() + 72));

            doc.Add(table);

            doc.Close();
        }
    }
}