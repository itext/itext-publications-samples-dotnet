/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

/**
 * Example written by Bruno Lowagie in answer to the following question:
 * http://stackoverflow.com/questions/31263533/how-to-create-nested-column-using-itextsharp
 */

using System;
using System.IO;
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
        public static readonly string DEST = "results/sandbox/tables/simple_table12.pdf";

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

            Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
            
            // The Style instance will be passed by default always until the moment we desire to specify some custom border
            Style style = new Style();
            
            // Default border width value is 0.5, we set it on 1 for cells borders of the table which will
            // use the Style instance
            style.SetBorder(new SolidBorder(1));
            
            table.AddCell(CreateCell("Examination", 1, 2, style));
            table.AddCell(CreateCell("Board", 1, 2, style));
            table.AddCell(CreateCell("Month and Year of Passing", 1, 2, style));
            table.AddCell(CreateCell("", 1, 1, new Style()
                    .SetBorder(Border.NO_BORDER)
                    .SetBorderTop(new SolidBorder(1))
                    .SetBorderBottom(new SolidBorder(1))
                    .SetBorderLeft(new SolidBorder(1))));
            table.AddCell(CreateCell("Marks", 2, 1, new Style()
                    .SetBorder(Border.NO_BORDER)
                    .SetBorderTop(new SolidBorder(1))
                    .SetBorderBottom(new SolidBorder(1))
                    .SetBorderRight(new SolidBorder(1))));
            table.AddCell(CreateCell("Percentage", 1, 2, style));
            table.AddCell(CreateCell("Class / Grade", 1, 2, style));
            table.AddCell(CreateCell("", 1, 1, new Style()));
            table.AddCell(CreateCell("Obtained", 1, 1, style));
            table.AddCell(CreateCell("Out of", 1, 1, style));
            table.AddCell(CreateCell("12th / I.B. Diploma", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));
            table.AddCell(CreateCell("Aggregate (all subjects)", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));
            table.AddCell(CreateCell("", 1, 1, style));

            doc.Add(table);

            doc.Close();
        }

        private static Cell CreateCell(string content, int colspan, int rowspan, Style style) 
        {
            Paragraph paragraph = new Paragraph(content)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER);
            
            Cell cell = new Cell(rowspan, colspan).Add(paragraph);
            cell.AddStyle(style);
            
            return cell;
        }
    }
}