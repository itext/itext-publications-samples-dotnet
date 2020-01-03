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
    public class SplitRowAtEndOfPage
    {
        public static readonly string DEST = "results/sandbox/tables/split_row_at_end_of_page.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SplitRowAtEndOfPage().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // Notice that the width is bigger than available area (612 - 36 - 36 = 540, where 36 is the value of the left (and the right) margin
            table.SetWidth(550);

            for (int i = 0; i < 6; i++)
            {
                Cell cell = new Cell()
                    .Add(new Paragraph((i == 5) ? "Three\nLines\nHere" : i.ToString()));
                
                table.AddCell(cell);
            }

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(612, 237));

            doc.Add(table);

            doc.Close();
        }
    }
}