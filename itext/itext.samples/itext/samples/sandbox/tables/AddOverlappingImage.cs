/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class AddOverlappingImage
    {
        public static readonly string DEST = "../../results/sandbox/tables/add_overlapping_image.pdf";
        
        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddOverlappingImage().ManipulatePdf(DEST);
        }
        
        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method set table to use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            
            // Adds drawn on a canvas image to the table
            table.SetNextRenderer(new OverlappingImageTableRenderer(table, new Table.RowRange(0, 25),
                ImageDataFactory.Create("../../resources/img/hero.jpg")));

            for (int r = 'A'; r <= 'Z'; r++) 
            {
                for (int c = 1; c <= 5; c++)
                {
                    Cell cell = new Cell();
                    cell.Add(new Paragraph(((char)r) + c.ToString()));
                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private class OverlappingImageTableRenderer : TableRenderer
        {
            private ImageData image;

            public OverlappingImageTableRenderer(Table modelElement, Table.RowRange rowRange, ImageData img)
                : base(modelElement, rowRange)
            {
                image = img;
            }

            public OverlappingImageTableRenderer(Table modelElement, ImageData img)
                : base(modelElement)
            {
                image = img;
            }

        
        public override void DrawChildren(DrawContext drawContext)
        {
            base.DrawChildren(drawContext);
            
            float x = Math.Max(GetOccupiedAreaBBox().GetX() +
                    GetOccupiedAreaBBox().GetWidth() / 3 - image.GetWidth(), 0);
            float y = Math.Max(GetOccupiedAreaBBox().GetY() +
                    GetOccupiedAreaBBox().GetHeight() / 3 - image.GetHeight(), 0);
            
            drawContext.GetCanvas().AddImage(image, x, y, false);
        }


        public override IRenderer GetNextRenderer()
        {
            return new OverlappingImageTableRenderer((Table)modelElement, image);
        }
    }
}

}
