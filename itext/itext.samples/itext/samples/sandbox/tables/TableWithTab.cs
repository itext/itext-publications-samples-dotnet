/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class TableWithTab
    {
        public static readonly string DEST = "../../results/sandbox/tables/table_with_tab.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableWithTab().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            Paragraph p = new Paragraph();
            p.Add("Left");
            p.AddTabStops(new TabStop(1000, TabAlignment.RIGHT));
            p.Add(new Tab());
            p.Add("Right");
            table.AddCell(p);

            doc.Add(table);

            doc.Close();
        }
    }
}