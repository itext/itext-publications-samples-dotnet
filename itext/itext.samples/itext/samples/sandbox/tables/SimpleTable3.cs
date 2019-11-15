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
    public class SimpleTable3
    {
        public static readonly string DEST = "../../results/sandbox/tables/simple_table3.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable3().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A3.Rotate());

            Table table = new Table(UnitValue.CreatePercentArray(35)).UseAllAvailableWidth().SetFixedLayout();
            table.SetWidth(pdfDoc.GetDefaultPageSize().GetWidth() - 80);

            Cell contractor = new Cell(1, 5).Add(new Paragraph("XXXXXXXXXXXXX"));
            table.AddCell(contractor);

            Cell workType = new Cell(1, 5).Add(new Paragraph("Refractory Works"));
            table.AddCell(workType);

            Cell supervisor = new Cell(1, 4).Add(new Paragraph("XXXXXXXXXXXXXX"));
            table.AddCell(supervisor);

            Cell paySlipHead = new Cell(1, 10).Add(new Paragraph("XXXXXXXXXXXXXXXX"));
            table.AddCell(paySlipHead);

            Cell paySlipMonth = new Cell(1, 2).Add(new Paragraph("XXXXXXX"));
            table.AddCell(paySlipMonth);

            Cell blank = new Cell(1, 9).Add(new Paragraph(""));
            table.AddCell(blank);

            doc.Add(table);

            doc.Close();
        }
    }
}