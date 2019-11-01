/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class NestedTableProblem
    {
        public static readonly string DEST = "../../results/sandbox/tables/nested_table_problem.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTableProblem().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(612, 792));

            doc.SetMargins(30, 21, 35, 21);

            // inner table
            Table innerTable = new Table(UnitValue.CreatePercentArray(1));
            innerTable.SetWidth(UnitValue.CreatePercentValue(80));
            innerTable.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            innerTable.AddCell(new Cell().SetBorder(new SolidBorder(ColorConstants.RED, 1))
                .Add(new Paragraph("Goodbye World")));

            // outer table
            Table outerTable = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            outerTable.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            Cell cell = new Cell();
            cell.SetBorder(new SolidBorder(ColorConstants.BLACK, 1));
            cell.Add(new Paragraph("Hello World"));
            cell.Add(innerTable);
            cell.Add(new Paragraph("Hello World"));

            outerTable.AddCell(cell);
            doc.Add(outerTable);

            doc.Close();
        }
    }
}