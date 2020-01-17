/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Barcodes;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SmallTable
    {
        public static readonly string DEST = "results/sandbox/tables/small_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SmallTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(290, 112));
            doc.SetMargins(5, 5, 5, 5);

            Table table = new Table(new float[] {160, 120});

            // first row
            Cell cell = new Cell(1, 2).Add(new Paragraph("Some text here"));
            cell.SetHeight(30);
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            // second row
            cell = new Cell().Add(new Paragraph("Some more text").SetFontSize(10));
            cell.SetHeight(30);
            cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            Barcode128 code128 = new Barcode128(pdfDoc);
            code128.SetCode("14785236987541");
            code128.SetCodeType(Barcode128.CODE128);

            Image code128Image = new Image(code128.CreateFormXObject(pdfDoc));

            cell = new Cell().Add(code128Image.SetAutoScale(true));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetHeight(30);
            table.AddCell(cell);

            // third row
            table.AddCell(cell.Clone(true));
            cell = new Cell().Add(new Paragraph("and something else here").SetFontSize(10));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetTextAlignment(TextAlignment.RIGHT);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}