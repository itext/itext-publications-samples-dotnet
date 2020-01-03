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
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class TiledBackgroundColor2
    {
        public static readonly string DEST = "results/sandbox/tables/tiled_background_color2.pdf";

        public static readonly String IMG = "../../resources/img/bulb.gif";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TiledBackgroundColor2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell("Behold a cell with an image pattern:");

            Cell cell = new Cell();
            ImageData img = ImageDataFactory.Create(IMG);
            cell.SetNextRenderer(new TiledImageBackgroundRenderer(cell, img));
            cell.SetHeight(60);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class TiledImageBackgroundRenderer : CellRenderer
        {
            private ImageData img;

            public TiledImageBackgroundRenderer(Cell modelElement, ImageData img)
                : base(modelElement)
            {
                this.img = img;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TiledImageBackgroundRenderer((Cell) modelElement, img);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfCanvas canvas = drawContext.GetCanvas();
                Rectangle position = GetOccupiedAreaBBox();

                Image image = new Image(img);
                image.ScaleToFit(10000000, position.GetHeight());

                float x = position.GetLeft();
                float y = position.GetBottom();

                while (x + image.GetImageScaledWidth() < position.GetRight())
                {
                    image.SetFixedPosition(x, y);
                    canvas.AddImage(img, x, y, image.GetImageScaledWidth(), false);
                    x += image.GetImageScaledWidth();
                }
            }
        }
    }
}