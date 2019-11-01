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
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class TiledBackground
    {
        public static readonly string DEST = "../../results/sandbox/tables/tiled_background.pdf";

        public static readonly String IMG1 = "../../resources/img/ALxRF.png";

        public static readonly String IMG2 = "../../resources/img/bulb.gif";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TiledBackground().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            Cell cell = new Cell();
            ImageData image = ImageDataFactory.Create(IMG1);
            cell.SetNextRenderer(new TiledImageBackgroundCellRenderer(cell, image));
            cell.SetProperty(Property.BOX_SIZING, BoxSizingPropertyValue.BORDER_BOX);
            cell.SetHeight(770).SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            cell = new Cell();
            image = ImageDataFactory.Create(IMG2);
            cell.SetNextRenderer(new TiledImageBackgroundCellRenderer(cell, image));
            cell.SetProperty(Property.BOX_SIZING, BoxSizingPropertyValue.BORDER_BOX);
            cell.SetHeight(770).SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class TiledImageBackgroundCellRenderer : CellRenderer
        {
            private ImageData img;

            public TiledImageBackgroundCellRenderer(Cell modelElement, ImageData img)
                : base(modelElement)
            {
                this.img = img;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TiledImageBackgroundCellRenderer((Cell) modelElement, img);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfPattern.Tiling imgPattern = new PdfPattern.Tiling(img.GetWidth(), img.GetHeight(),
                    img.GetWidth(), img.GetHeight());

                PdfPatternCanvas patternCanvas = new PdfPatternCanvas(imgPattern, drawContext.GetDocument());
                patternCanvas.AddImage(img, 0, 0, false);
                
                PdfCanvas canvas = drawContext.GetCanvas();
                
                canvas.SaveState();

                ColorRectangle(canvas, new PatternColor(imgPattern), GetOccupiedAreaBBox().GetX(),
                    GetOccupiedAreaBBox().GetY(), GetOccupiedAreaBBox().GetWidth(), GetOccupiedAreaBBox().GetHeight());

                canvas.SetFillColor(new PatternColor(imgPattern));
                canvas.Stroke();

                canvas.RestoreState();
            }

            private static void ColorRectangle(PdfCanvas canvas, Color color, float x, float y, float width, float height) {
                canvas
                    .SaveState()
                    .SetFillColor(color)
                    .Rectangle(x, y, width, height)
                    .FillStroke()
                    .RestoreState();
            }
        }
    }
}