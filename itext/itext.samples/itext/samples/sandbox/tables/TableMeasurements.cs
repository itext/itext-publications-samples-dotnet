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
    public class TableMeasurements
    {
        public static readonly string DEST = "results/sandbox/tables/tables_measurements.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableMeasurements().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(10));
            table.SetWidth(MillimetersToPoints(100));
            table.AddCell(GetCell(10));
            table.AddCell(GetCell(5));
            table.AddCell(GetCell(3));
            table.AddCell(GetCell(2));
            table.AddCell(GetCell(3));
            table.AddCell(GetCell(5));
            table.AddCell(GetCell(1));

            doc.Add(table);

            doc.Close();
        }

        private static float MillimetersToPoints(float value)
        {
            return (value / 25.4f) * 72f;
        }

        private static Cell GetCell(int cm)
        {
            Cell cell = new Cell(1, cm);
            Paragraph p = new Paragraph(String.Format("{0}mm", 10 * cm)).SetFontSize(8);
            p.SetTextAlignment(TextAlignment.CENTER);
            p.SetMultipliedLeading(0.5f);
            p.SetMarginTop(0);
            cell.Add(p);
            return cell;
        }
    }
}