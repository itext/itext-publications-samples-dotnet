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

namespace iText.Samples.Sandbox.Tables
{
    public class KeepingTogetherInnerTable
    {
        public static readonly string DEST = "results/sandbox/tables/keeping_together_inner_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KeepingTogetherInnerTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(300, 160));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetMarginTop(10);

            Cell cell = new Cell();
            cell.Add(new Paragraph("G"));
            cell.Add(new Paragraph("R"));
            cell.Add(new Paragraph("O"));
            cell.Add(new Paragraph("U"));
            cell.Add(new Paragraph("P"));
            table.AddCell(cell);

            Table inner = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            inner.SetKeepTogether(true);
            inner.AddCell("row 1");
            inner.AddCell("row 2");
            inner.AddCell("row 3");
            inner.AddCell("row 4");
            inner.AddCell("row 5");

            cell = new Cell().Add(inner);
            cell.SetPadding(0);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}