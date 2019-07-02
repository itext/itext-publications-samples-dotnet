/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class TableTemplate
    {
        public static readonly string DEST = "../../results/sandbox/tables/table_template.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableTemplate().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Table table = new Table(UnitValue.CreatePercentArray(15)).UseAllAvailableWidth();

            table.SetWidth(1500);

            for (int r = 'A'; r <= 'Z'; r++)
            {
                for (int c = 1; c <= 15; c++)
                {
                    Cell cell = new Cell();
                    cell.SetMinHeight(45);
                    cell.Add(new Paragraph(((char) r).ToString() + c));
                    table.AddCell(cell);
                }
            }

            PdfFormXObject tableTemplate = new PdfFormXObject(new Rectangle(1500, 1300));
            Canvas canvas = new Canvas(tableTemplate, pdfDoc);
            canvas.Add(table);

            for (int j = 0; j < 1500; j += 500)
            {
                for (int i = 1300; i > 0; i -= 650)
                {
                    PdfFormXObject clip = new PdfFormXObject(new Rectangle(500, 650));
                    new PdfCanvas(clip, pdfDoc).AddXObject(tableTemplate, -j, 650 - i);
                    new PdfCanvas(pdfDoc.AddNewPage()).AddXObject(clip, 36, 156);
                }
            }

            pdfDoc.Close();
        }
    }
}