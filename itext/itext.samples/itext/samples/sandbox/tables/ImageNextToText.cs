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
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ImageNextToText
    {
        public static readonly string DEST = "results/sandbox/tables/image_next_to_text.pdf";

        public static readonly string IMG1 = "../../resources/img/javaone2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImageNextToText().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 2}));
            table.AddCell(CreateImageCell(IMG1));
            table.AddCell(CreateTextCell(
                "This picture was taken at Java One.\nIt shows the iText crew at Java One in 2013."));

            doc.Add(table);

            doc.Close();
        }

        private static Cell CreateImageCell(string path)
        {
            Image img = new Image(ImageDataFactory.Create(path));
            img.SetWidth(UnitValue.CreatePercentValue(100));
            Cell cell = new Cell().Add(img);
            cell.SetBorder(Border.NO_BORDER);
            return cell;
        }

        private static Cell CreateTextCell(string text)
        {
            Cell cell = new Cell();
            Paragraph p = new Paragraph(text);
            p.SetTextAlignment(TextAlignment.RIGHT);
            cell.Add(p).SetVerticalAlignment(VerticalAlignment.BOTTOM);
            cell.SetBorder(Border.NO_BORDER);
            return cell;
        }
    }
}