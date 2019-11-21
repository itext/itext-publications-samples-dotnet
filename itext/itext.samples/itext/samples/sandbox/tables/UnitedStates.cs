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
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class UnitedStates
    {
        public static readonly string DEST = "results/sandbox/tables/united_states.pdf";

        public static readonly String DATA = "../../resources/data/united_states.csv";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new UnitedStates().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            PdfFont helvetica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont helveticaBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {14, 6, 12, 16, 12, 12, 12, 12, 6}));
            using (StreamReader br = new StreamReader(DATA))
            {
                String line = br.ReadLine();

                // The last argument defines which cell will be added: a header or the usual one
                AddRowToTable(table, line, helveticaBold, true);
                while ((line = br.ReadLine()) != null)
                {
                    AddRowToTable(table, line, helvetica, false);
                }
            }

            doc.Add(table);
            
            doc.Close();
        }


        private static void AddRowToTable(Table table, String line, PdfFont font, bool isHeader)
        {
            // Parses string line with specified delimiter 
            StringTokenizer tokenizer = new StringTokenizer(line, ";");

            // Creates cells according to parsed csv line
            while (tokenizer.HasMoreTokens())
            {
                Cell cell = new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font));

                if (isHeader)
                {
                    table.AddHeaderCell(cell);
                }
                else
                {
                    table.AddCell(cell);
                }
            }
        }
    }
}