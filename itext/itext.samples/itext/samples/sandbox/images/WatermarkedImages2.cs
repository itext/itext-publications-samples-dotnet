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
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Images
{
    public class WatermarkedImages2
    {
        public static readonly String DEST = "results/sandbox/images/watermarked_images2.pdf";

        public static readonly String IMAGE1 = "../../../resources/img/bruno.jpg";
        public static readonly String IMAGE2 = "../../../resources/img/dog.bmp";
        public static readonly String IMAGE3 = "../../../resources/img/fox.bmp";
        public static readonly String IMAGE4 = "../../../resources/img/bruno_ingeborg.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WatermarkedImages2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(1).UseAllAvailableWidth();

            Image image = new Image(ImageDataFactory.Create(IMAGE1)).SetAutoScaleWidth(true);
            Cell cell = new Cell().Add(image);
            cell.SetNextRenderer(new WatermarkedCellRenderer(cell, "Bruno"));
            table.AddCell(cell);

            image = new Image(ImageDataFactory.Create(IMAGE2)).SetAutoScaleWidth(true);
            cell = new Cell().Add(image);
            cell.SetNextRenderer(new WatermarkedCellRenderer(cell, "Dog"));
            table.AddCell(cell);

            image = new Image(ImageDataFactory.Create(IMAGE3)).SetAutoScaleWidth(true);
            cell = new Cell().Add(image);
            cell.SetNextRenderer(new WatermarkedCellRenderer(cell, "Fox"));
            table.AddCell(cell);

            image = new Image(ImageDataFactory.Create(IMAGE4)).SetAutoScaleWidth(true);
            cell = new Cell().Add(image);
            cell.SetNextRenderer(new WatermarkedCellRenderer(cell, "Bruno and Ingeborg"));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class WatermarkedCellRenderer : CellRenderer
        {
            private String content;

            public WatermarkedCellRenderer(Cell modelElement, String content) : base(modelElement)
            {
                this.content = content;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new WatermarkedCellRenderer((Cell) modelElement, content);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                Paragraph p = new Paragraph(content).SetFontColor(ColorConstants.WHITE);
                Rectangle rect = GetOccupiedAreaBBox();
                float coordX = (rect.GetLeft() + rect.GetRight()) / 2;
                float coordY = (rect.GetBottom() + rect.GetTop()) / 2;
                float angle = (float) Math.PI / 6;
                new Canvas(drawContext.GetCanvas(), drawContext.GetDocument(), rect)
                    .ShowTextAligned(p, coordX, coordY, GetOccupiedArea().GetPageNumber(),
                        TextAlignment.CENTER, VerticalAlignment.MIDDLE, angle)
                    .Close();
            }
        }
    }
}