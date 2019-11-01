/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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

namespace iText.Samples.Sandbox.Tables
{
    public class NestedTables6
    {
        public static readonly string DEST = "../../results/sandbox/tables/nested_tables6.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTables6().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(1200, 800));

            // Header part
            Table mainTable = new Table(UnitValue.CreatePercentArray(1));
            mainTable.SetWidth(1000);

            // Notice that in itext7 there is no getDefaultCell method
            // and you should set paddings, margins and other properties exactly on the element
            // you want to handle them
            Table subTable2 = new Table(new float[] {200, 100, 200, 200, 300});
            subTable2.AddCell("test 1");
            subTable2.AddCell("test 2");
            subTable2.AddCell("test 3");
            subTable2.AddCell("test 4");
            subTable2.AddCell("test 5");

            Cell cell2 = new Cell().Add(subTable2);
            cell2.SetPadding(0);
            mainTable.AddCell(cell2);

            doc.Add(mainTable);
            
            doc.Close();
        }
    }
}