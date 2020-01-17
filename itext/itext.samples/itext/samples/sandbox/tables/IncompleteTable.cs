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
    public class IncompleteTable
    {
        public static readonly string DEST = "results/sandbox/tables/incomplete_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new IncompleteTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // The second argument determines 'large table' functionality is used
            // It defines whether parts of the table will be written before all data is added.
            Table table = new Table(UnitValue.CreatePercentArray(5), true);

            for (int i = 0; i < 5; i++)
            {
                table.AddHeaderCell(new Cell().SetKeepTogether(true).Add(new Paragraph("Header " + i)));
            }

            // For the "large tables" they shall be added to the document before its child elements are populated
            doc.Add(table);

            for (int i = 0; i < 500; i++)
            {
                if (i % 5 == 0)
                {
                    // Flushes the current content, e.g. places it on the document.
                    // Please bear in mind that the method (alongside complete()) make sense only for 'large tables'
                    table.Flush();
                }

                table.AddCell(new Cell().SetKeepTogether(true).Add(new Paragraph("Test " + i)
                    .SetMargins(0, 0, 0, 0)));
            }

            // Flushes the rest of the content and indicates that no more content will be added to the table
            table.Complete();

            doc.Close();
        }
    }
}