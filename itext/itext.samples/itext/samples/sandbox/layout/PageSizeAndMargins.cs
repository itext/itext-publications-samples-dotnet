/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this Address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Layout
{
    public class PageSizeAndMargins
    {
        public static readonly string DEST = "results/sandbox/layout/pageSizeAndMargins.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PageSizeAndMargins().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(200, 200));

            String marginsText = doc.GetTopMargin() + ", " + doc.GetRightMargin() + ", "
                                 + doc.GetBottomMargin() + ", " + doc.GetLeftMargin();
            Paragraph p = new Paragraph("Margins: [" + marginsText + "]\nPage size: 150*150");
            doc.Add(p);

            // The margins will be applied on the pages,
            // which have been added after the call of this method.
            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new AreaBreak());
            p = new Paragraph("Margins: [10.0, 10.0, 10.0, 10.0]\nPage size: 150*150");
            doc.Add(p);

            // Add a new A4 page.
            doc.Add(new AreaBreak(PageSize.A4));
            p = new Paragraph("Margins: [10.0, 10.0, 10.0, 10.0]\nPage size: A4");
            doc.Add(p);

            // Add a new page.
            // The page size will be the same as it is set in the Document.
            doc.Add(new AreaBreak());
            p = new Paragraph("Margins: [10.0, 10.0, 10.0, 10.0]\nPage size: 150*150");
            doc.Add(p);

            doc.Close();
        }
    }
}