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

namespace iText.Samples.Sandbox.Columntext
{
    public class ColumnTextChunkImage
    {
        public static readonly String DEST = "results/sandbox/columntext/column_text_chunk_image.pdf";

        public static readonly String DOG = "../../resources/img/dog.bmp";
        public static readonly String FOX = "../../resources/img/fox.bmp";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ColumnTextChunkImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image dog = new Image(ImageDataFactory.Create(DOG));
            Image fox = new Image(ImageDataFactory.Create(FOX));
            Paragraph p = new Paragraph("quick brown fox jumps over the lazy dog.");
            p.Add("Or, to say it in a more colorful way: quick brown ");
            p.Add(fox);
            p.Add(" jumps over the lazy ");
            p.Add(dog);
            p.Add(".");

            p.SetWidth(350);
            doc.ShowTextAligned(p, 50, 650, TextAlignment.LEFT);

            doc.Close();
        }
    }
}