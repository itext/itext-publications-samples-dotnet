/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class AddOverlappingImage
    {
        public static readonly string DEST = "results/sandbox/tables/add_overlapping_image.pdf";
        
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
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();

            for (int r = 'A'; r <= 'Z'; r++) 
            {
                for (int c = 1; c <= 5; c++)
                {
                    Cell cell = new Cell();
                    cell.Add(new Paragraph(((char)r) + c.ToString()));
                    table.AddCell(cell);
                }
            }

            // Adds drawn on a canvas image to the table
            table.SetNextRenderer(new OverlappingImageTableRenderer(table,
                ImageDataFactory.Create("../../../resources/img/hero.jpg")));

            doc.Add(table);

            doc.Close();
        }

        private class OverlappingImageTableRenderer : TableRenderer
        {
            private ImageData image;

            public OverlappingImageTableRenderer(Table modelElement, ImageData img)
                : base(modelElement)
            {
                image = img;
            }

        
        public override void DrawChildren(DrawContext drawContext)
        {

            // Use the coordinates of the cell in the fourth row and the second column to draw the image
            Rectangle rect = rows[3][1].GetOccupiedAreaBBox();
                base.DrawChildren(drawContext);

            drawContext.GetCanvas().AddImage(image, rect.GetLeft() + 10, rect.GetTop() - image.GetHeight(), false);
        }            
        
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
        {
            return new OverlappingImageTableRenderer((Table)modelElement, image);
        }
    }
}

}
