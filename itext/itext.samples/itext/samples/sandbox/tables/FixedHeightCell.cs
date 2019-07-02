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
    public class FixedHeightCell
    {
        public static readonly string DEST = "../../results/sandbox/tables/fixed_height_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FixedHeightCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();

            for (int r = 'A'; r <= 'Z'; r++)
            {
                for (int c = 1; c <= 5; c++)
                {
                    Cell cell = new Cell();
                    cell.Add(new Paragraph(((char) r).ToString() + c));
                    
                    if (r == 'D')
                    {
                        cell.SetHeight(60);
                    }
                    if (r == 'E')
                    {
                        cell.SetHeight(60);
                        if (c == 4)
                        {
                            cell.SetHeight(120);
                        }
                    }
                    if (r == 'F')
                    {
                        cell.SetMinHeight(60);
                        cell.SetHeight(60);
                        if (c == 2)
                        {
                            cell.Add(new Paragraph("This cell has more content than the other cells"));
                        }
                    }

                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            doc.Close();
        }
    }
}