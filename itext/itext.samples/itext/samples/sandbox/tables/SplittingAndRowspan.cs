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
    public class SplittingAndRowspan
    {
        public static readonly string DEST = "results/sandbox/tables/splitting_and_rowspan.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SplittingAndRowspan().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(300, 160));

            doc.Add(new Paragraph("Table with setKeepTogether(true):"));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetKeepTogether(true);
            table.SetMarginTop(10);

            Cell cell = new Cell(3, 1);
            cell.Add(new Paragraph("G"));
            cell.Add(new Paragraph("R"));
            cell.Add(new Paragraph("P"));

            table.AddCell(cell);
            table.AddCell("row 1");
            table.AddCell("row 2");
            table.AddCell("row 3");

            doc.Add(table);

            doc.Add(new AreaBreak());

            doc.Add(new Paragraph("Table with setKeepTogether(false):"));
            table.SetKeepTogether(false);

            doc.Add(table);
            
            doc.Close();
        }
    }
}