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
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class DottedLineLeader
    {
        public static readonly string DEST = "../../results/sandbox/tables/dotted_line_leader.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DottedLineLeader().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {10, 30, 10}));
            table.SetWidth(UnitValue.CreatePercentValue(50));

            // Creates dotted line leader
            ILineDrawer leader = new DottedLine(1.5f, 6);

            table.AddCell(GetCell(new Paragraph("fig 1"), VerticalAlignment.TOP));

            Paragraph p = new Paragraph("Title text");
            p.AddTabStops(new TabStop(150, TabAlignment.RIGHT, leader));
            p.Add(new Tab());
            table.AddCell(GetCell(p, VerticalAlignment.TOP));
            table.AddCell(GetCell(new Paragraph("2"), VerticalAlignment.BOTTOM));
            table.AddCell(GetCell(new Paragraph("fig 2"), VerticalAlignment.TOP));

            p = new Paragraph("This is a longer title text that wraps");
            p.AddTabStops(new TabStop(150, TabAlignment.RIGHT, leader));
            p.Add(new Tab());
            table.AddCell(GetCell(p, VerticalAlignment.TOP));
            table.AddCell(GetCell(new Paragraph("55"), VerticalAlignment.BOTTOM));
            table.AddCell(GetCell(new Paragraph("fig 3"), VerticalAlignment.TOP));

            p = new Paragraph("Another title text");
            table.AddCell(GetCell(p, VerticalAlignment.TOP));
            p.AddTabStops(new TabStop(150, TabAlignment.RIGHT, leader));
            p.Add(new Tab());
            table.AddCell(GetCell(new Paragraph("89"), VerticalAlignment.BOTTOM));

            doc.Add(table);

            doc.Close();
        }

        private Cell GetCell(Paragraph p, VerticalAlignment? verticalAlignment)
        {
            Cell cell = new Cell();
            cell.SetVerticalAlignment(verticalAlignment);
            p.SetMargin(2);
            p.SetMultipliedLeading(1);
            cell.Add(p);
            return cell;
        }
    }
}