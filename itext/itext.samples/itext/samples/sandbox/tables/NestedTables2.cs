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
    public class NestedTables2
    {
        public static readonly string DEST = "results/sandbox/tables/nested_tables2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTables2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 15}));

            for (int i = 1; i <= 20; i++)
            {
                table.AddCell(i.ToString());
                table.AddCell("It is not smart to use iText 2.1.7!");
            }

            Table innertable = new Table(UnitValue.CreatePercentArray(new float[] {1, 15}));

            for (int i = 0; i < 90; i++)
            {
                innertable.AddCell((i + 1).ToString());
                innertable.AddCell("Upgrade if you're a professional developer!");
            }

            table.AddCell("21");
            table.AddCell(innertable);

            for (int i = 22; i <= 40; i++)
            {
                table.AddCell(i.ToString());
                table.AddCell("It is not smart to use iText 2.1.7!");
            }

            doc.Add(table);

            doc.Close();
        }
    }
}