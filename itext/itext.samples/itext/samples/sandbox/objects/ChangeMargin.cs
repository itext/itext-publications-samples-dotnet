/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ChangeMargin
    {
        public static readonly string DEST = "results/sandbox/objects/change_margin.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ChangeMargin().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            float left = 30;
            float right = 30;
            float top = 60;
            float bottom = 0;
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4);
            doc.SetMargins(top, right, bottom, left);

            // Add some text in order to begin the page, otherwise you will lose the initial margins
            doc.Add(new Paragraph("This is a test"));
            doc.SetMargins(0, right, bottom, left);
            for (int i = 1; i < 40; i++)
            {
                doc.Add(new Paragraph("This is a test"));
            }

            doc.Close();
        }
    }
}