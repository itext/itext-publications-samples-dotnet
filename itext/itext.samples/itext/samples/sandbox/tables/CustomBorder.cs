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
    public class CustomBorder
    {
        public static readonly string DEST = "../../results/sandbox/tables/custom_border.pdf";

        public static readonly string TEXT = "This is some long paragraph\n" +
                                             "that will be added over and over\n" +
                                             "again to prove a point.\n" +
                                             "It should result in rows\n" +
                                             "that are split\n" +
                                             " and rows that aren't.";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomBorder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method set table to use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            // Fill a table with cells.
            // Be aware that by default all the cells will not have ay bottom borders
            for (int i = 1; i <= 60; i++) 
            {
                table.AddCell(new Cell().Add(new Paragraph("Cell " + i)).SetBorderBottom(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(TEXT)).SetBorderBottom(Border.NO_BORDER));
            }

            // We do not need a smart collapse logic: on the contrary, we want to override
            // processing of some cells, that's why we set SEPARATE value here
            table.SetBorderCollapse(BorderCollapsePropertyValue.SEPARATE);

            // Use a custom renderer in which borders drawing is overridden
            table.SetNextRenderer(new CustomBorderTableRenderer(table));

            doc.Add(table);

            doc.Close();
        }

        private class CustomBorderTableRenderer : TableRenderer 
        {
            private bool bottom = true;
            private bool top = true;

            public CustomBorderTableRenderer(Table modelElement) : base(modelElement) 
            {   
            }

            public override IRenderer GetNextRenderer() 
            {
                return new CustomBorderTableRenderer((Table)modelElement);
            }

            protected override TableRenderer[] Split(int row, bool hasContent, bool cellWithBigRowspanAdded) 
            {
                TableRenderer[] renderers = base.Split(row, hasContent, cellWithBigRowspanAdded); 

                // The first row of the split renderer represent the first rows of the current renderer
                ((CustomBorderTableRenderer)renderers[0]).top = top;

                // If there are some split cells, we should draw top borders of the overflow renderer
                // and bottom borders of the split renderer
                if (hasContent) 
                {
                    ((CustomBorderTableRenderer)renderers[0]).bottom = false;
                    ((CustomBorderTableRenderer)renderers[1]).top = false;
                }
                return renderers;
            }

            public override void Draw(DrawContext drawContext) 
            {

                // If not set, iText will omit drawing of top borders
                if (!top) 
                {
                    foreach (CellRenderer cellRenderer in rows[0]) 
                    {
                        cellRenderer.SetProperty(Property.BORDER_TOP, Border.NO_BORDER);
                    }
                }

                // If set, iText will draw bottom borders
                if (bottom) 
                {
                    foreach (CellRenderer cellRenderer in rows[rows.Count - 1])
                    {
                        cellRenderer.SetProperty(Property.BORDER_BOTTOM, new SolidBorder(0.5f));
                    }
                }

                base.Draw(drawContext);
            }
        }
    }
}