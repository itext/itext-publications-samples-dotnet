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
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class TiledBackgroundColor
    {
        public static readonly string DEST = "results/sandbox/tables/tiled_background_color.pdf";

        public static readonly String IMG = "../../../resources/img/bulb.gif";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TiledBackgroundColor().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            ImageData img = ImageDataFactory.Create(IMG);
            Image image = new Image(img);

            PdfPattern.Tiling imgPattern = new PdfPattern.Tiling(image.GetImageScaledWidth(),
                image.GetImageScaledHeight());

            PdfPatternCanvas canvas = new PdfPatternCanvas(imgPattern, pdfDoc);
            canvas.AddImage(img, 0, 0, false);
            
            Color color = new PatternColor(imgPattern);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell(new Cell().Add(new Paragraph("Behold a cell with an image pattern:")));

            Cell cell = new Cell();
            cell.SetHeight(60);
            cell.SetBackgroundColor(color);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}