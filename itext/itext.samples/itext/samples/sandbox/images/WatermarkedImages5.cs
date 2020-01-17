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
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Images
{
    public class WatermarkedImages5
    {
        public static readonly String DEST = "results/sandbox/images/watermarked_images5.pdf";

        public static readonly String IMAGE1 = "../../resources/img/bruno.jpg";
        public static readonly String IMAGE2 = "../../resources/img/dog.bmp";
        public static readonly String IMAGE3 = "../../resources/img/fox.bmp";
        public static readonly String IMAGE4 = "../../resources/img/bruno_ingeborg.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WatermarkedImages5().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE1)));
            doc.Add(img);

            img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE2)));
            doc.Add(img);

            img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE3)));
            doc.Add(img);

            Image srcImage = new Image(ImageDataFactory.Create(IMAGE4));
            srcImage.ScaleToFit(400, 700);
            img = GetWatermarkedImage(pdfDoc, srcImage);
            doc.Add(img);

            doc.Close();
        }

        private static Image GetWatermarkedImage(PdfDocument pdfDocument, Image img)
        {
            float width = img.GetImageScaledWidth();
            float height = img.GetImageScaledHeight();

            Table table = InitTable(width);

            TableRenderer renderer = (TableRenderer) table.CreateRendererSubTree();
            renderer.SetParent(new DocumentRenderer(new Document(pdfDocument)));

            // Simulate the positioning of the renderer to find out how much space the table will occupy.
            LayoutResult result = renderer.Layout(new LayoutContext(new LayoutArea(
                1, new Rectangle(10000, 10000))));

            PdfFormXObject template = new PdfFormXObject(new Rectangle(width, height));
            new Canvas(template, pdfDocument)
                .Add(img)
                .Close();

            float left = 0;
            float bottom = height - result.GetOccupiedArea().GetBBox().GetHeight();
            new Canvas(template, pdfDocument)
                .Add(table.SetFixedPosition(left, bottom, width))
                .Close();

            return new Image(template);
        }

        private static Table InitTable(float width)
        {
            Table table = new Table(2).SetWidth(width);
            table.AddCell(new Cell().Add(new Paragraph("Test1")).SetBorder(new SolidBorder(ColorConstants.YELLOW, 1)));
            table.AddCell(new Cell().Add(new Paragraph("Test2")).SetBorder(new SolidBorder(ColorConstants.YELLOW, 1)));
            table.AddCell(new Cell().Add(new Paragraph("Test3")).SetBorder(new SolidBorder(ColorConstants.YELLOW, 1)));
            table.AddCell(new Cell().Add(new Paragraph("Test4")).SetBorder(new SolidBorder(ColorConstants.YELLOW, 1)));
            return table;
        }
    }
}