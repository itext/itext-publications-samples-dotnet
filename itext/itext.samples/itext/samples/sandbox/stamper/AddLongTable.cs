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

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddLongTable 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_long_table.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddLongTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4);
            doc.SetTopMargin(72);
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            for (int i = 0; i < 250; ) 
            {
                table.AddCell("Row " + (++i));
                table.AddCell("Test");
            }
            doc.Add(table);
            doc.Close();
        }
    }
}
