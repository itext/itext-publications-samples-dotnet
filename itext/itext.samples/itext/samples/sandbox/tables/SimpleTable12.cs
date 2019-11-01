/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable12
    {
        public static readonly string DEST = "../../results/sandbox/tables/simple_table12.pdf";

        private static PdfFont font;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable12().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
            
            table.AddCell(CreateCell("Examination", 1, 2, 15));
            table.AddCell(CreateCell("Board", 1, 2, 15));
            table.AddCell(CreateCell("Month and Year of Passing", 1, 2, 15));
            table.AddCell(CreateCell("", 1, 1, 1));
            table.AddCell(CreateCell("Marks", 2, 1, 1));
            table.AddCell(CreateCell("Percentage", 1, 2, 15));
            table.AddCell(CreateCell("Class / Grade", 1, 2, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("Obtained", 1, 1, 15));
            table.AddCell(CreateCell("Out of", 1, 1, 15));
            table.AddCell(CreateCell("12th / I.B. Diploma", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("Aggregate (all subjects)", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));
            table.AddCell(CreateCell("", 1, 1, 15));

            doc.Add(table);

            doc.Close();
        }

        private static Cell CreateCell(String content, int colspan, int rowspan, int border)
        {
            Cell cell = new Cell(rowspan, colspan).Add(new Paragraph(content).SetFont(font).SetFontSize(10));
            cell
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER);

            if (8 == (border & 8))
            {
                cell.SetBorderRight(new SolidBorder(1));
                cell.SetBorderBottom(new SolidBorder(1));
            }

            if (4 == (border & 4))
            {
                cell.SetBorderLeft(new SolidBorder(1));
            }

            if (2 == (border & 2))
            {
                cell.SetBorderBottom(new SolidBorder(1));
            }

            if (1 == (border & 1))
            {
                cell.SetBorderTop(new SolidBorder(1));
            }
            
            return cell;
        }
    }
}