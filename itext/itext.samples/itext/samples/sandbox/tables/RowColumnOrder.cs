/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
    public class RowColumnOrder
    {
        public static readonly string DEST = "results/sandbox/tables/row_column_order.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RowColumnOrder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(DEST));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("By design tables are filled row by row:"));

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            table.SetMarginTop(10);
            table.SetMarginBottom(10);

            for (int i = 1; i <= 15; i++)
            {
                table.AddCell("cell " + i);
            }

            doc.Add(table);

            doc.Add(new Paragraph(
                "If you want to change this behavior, you need to create a two-dimensional array first:"));

            String[][] array = new String[3][];
            int column = 0;
            int row = 0;

            for (int i = 1; i <= 15; i++)
            {
                if (column == 0)
                {
                    array[row] = new String[5];
                }

                array[row++][column] = "cell " + i;

                if (row == 3)
                {
                    column++;
                    row = 0;
                }
            }

            table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            table.SetMarginTop(10);

            foreach (String[] r in array)
            {
                foreach (String c in r)
                {
                    table.AddCell(c);
                }
            }

            doc.Add(table);

            doc.Close();
        }
    }
}