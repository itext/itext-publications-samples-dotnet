/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ArrayToTable
    {
        public static readonly string DEST = "results/sandbox/tables/array_to_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArrayToTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();

            List<List<string>> dataset = GetData();
            foreach (List<string> record in dataset)
            {
                foreach (string field in record)
                {
                    table.AddCell(new Cell().Add(new Paragraph(field)));
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private static List<List<string>> GetData()
        {
            List<List<string>> data = new List<List<string>>();
            String[] tableTitleList = {" Title", " (Re)set", " Obs", " Mean", " Std.Dev", " Min", " Max", "Unit"};
            data.Add(tableTitleList.ToList());

            for (int i = 0; i < 10; i++)
            {
                List<string> dataLine = new List<string>();
                for (int j = 0; j < tableTitleList.Length; j++)
                {
                    dataLine.Add(tableTitleList[j] + " " + (i + 1));
                }

                data.Add(dataLine);
            }

            return data;
        }
    }
}