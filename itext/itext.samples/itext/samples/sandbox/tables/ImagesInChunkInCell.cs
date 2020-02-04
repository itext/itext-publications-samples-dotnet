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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Tables
{
    public class ImagesInChunkInCell
    {
        public static readonly string DEST = "results/sandbox/tables/images_in_chunk_in_cell.pdf";

        public static readonly string IMG = "../../../resources/img/bulb.gif";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImagesInChunkInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image image = new Image(ImageDataFactory.Create(IMG));
            Table table = new Table(new float[] {120});
            Paragraph listOfDots = new Paragraph();

            for (int i = 0; i < 40; i++)
            {
                listOfDots.Add(image);
                listOfDots.Add(new Text(" "));
            }

            table.AddCell(listOfDots);

            doc.Add(table);

            doc.Close();
        }
    }
}