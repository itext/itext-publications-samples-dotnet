/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class ImageBackground
    {
        public static readonly string DEST = "../../results/sandbox/tables/image_background.pdf";

        public static readonly string IMG = "../../resources/img/bruno.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImageBackground().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetWidth(400);

            Cell cell = new Cell();
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            Paragraph p = new Paragraph("A cell with an image as background color.")
                .SetFont(font).SetFontColor(DeviceGray.WHITE);
            cell.Add(p);

            Image img = new Image(ImageDataFactory.Create(IMG));

            // Draws an image as the cell's background
            cell.SetNextRenderer(new ImageBackgroundCellRenderer(cell, img));
            cell.SetHeight(600 * img.GetImageHeight() / img.GetImageWidth());
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class ImageBackgroundCellRenderer : CellRenderer
        {
            private Image img;

            public ImageBackgroundCellRenderer(Cell modelElement, Image img)
                : base(modelElement)
            {
                this.img = img;
            }

            public override void Draw(DrawContext drawContext)
            {
                img.ScaleToFit(GetOccupiedAreaBBox().GetWidth(), GetOccupiedAreaBBox().GetHeight());
                drawContext.GetCanvas().AddXObject(img.GetXObject(), GetOccupiedAreaBBox());
                base.Draw(drawContext);
            }
        }
    }
}