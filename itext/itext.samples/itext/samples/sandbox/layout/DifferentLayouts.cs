/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this Address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Layout
{
    public class DifferentLayouts
    {
        public static readonly string DEST = "results/sandbox/layout/differentLayouts.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DifferentLayouts().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // Add a flowing paragraph
            doc.Add(new Paragraph("Flowing paragraph"));

            // Add a fixed paragraph
            Paragraph p = new Paragraph("Fixed paragraph")
                .SetHeight(200)
                .SetWidth(200)
                .SetBackgroundColor(ColorConstants.GREEN);
            doc.ShowTextAligned(p, 100, 100, TextAlignment.LEFT);

            doc.Close();
        }
    }
}