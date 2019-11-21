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
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class NestedTables4
    {
        public static readonly string DEST = "results/sandbox/tables/nested_tables4.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTables4().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 12, 8, 1}));
            table.SetBorder(new SolidBorder(1));

            // first row
            Cell cell = new Cell(1, 4).Add(new Paragraph("Main table"));
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            // second row
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("nested table 1")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("nested table 2")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            // third row
            // third row cell 1
            table.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));

            // third row cell 2
            Table table1 = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table1.AddCell("cell 1 of nested table 1");
            table1.AddCell("cell 2 of nested table 1");
            table1.AddCell("cell 3 of nested table 1");
            table.AddCell(new Cell().Add(table1).SetBorder(Border.NO_BORDER));

            // third row cell 3
            Table table2 = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table2.AddCell(new Cell().SetMinHeight(10));
            table2.AddCell(new Cell().SetMinHeight(10));
            cell = new Cell(1, 2).Add(new Paragraph("cell 2 of nested table 2")).SetMinHeight(10);
            table2.AddCell(cell);
            cell = new Cell(1, 2).Add(new Paragraph("cell 3 of nested table 2")).SetMinHeight(10);
            table2.AddCell(cell);
            table.AddCell(new Cell().Add(table2).SetBorder(Border.NO_BORDER));

            // third row cell 4
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            // fourth row
            cell = new Cell(1, 4);
            cell.SetBorder(Border.NO_BORDER);
            cell.SetMinHeight(16);

            table.AddCell(cell);
            doc.Add(table);

            doc.Close();
        }
    }
}