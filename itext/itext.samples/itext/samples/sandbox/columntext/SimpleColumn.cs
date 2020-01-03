/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Columntext
{
    public class SimpleColumn
    {
        public static readonly String DEST = "results/sandbox/columntext/simple_column.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleColumn().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(100, 120));

            Paragraph paragraph = new Paragraph("REALLLLLLLLLLY LONGGGGGGGGGG text").SetFontSize(4.5f);

            paragraph.SetWidth(61);
            doc.ShowTextAligned(paragraph, 9, 85, TextAlignment.LEFT);

            doc.Close();
        }
    }
}