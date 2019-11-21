/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable10
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table10.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable10().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();

            Cell sn = new Cell(2, 1).Add(new Paragraph("S/N"));
            sn.SetBackgroundColor(ColorConstants.YELLOW);
            table.AddCell(sn);

            Cell name = new Cell(1, 3).Add(new Paragraph("Name"));
            name.SetBackgroundColor(ColorConstants.CYAN);
            table.AddCell(name);

            Cell age = new Cell(2, 1).Add(new Paragraph("Age"));
            age.SetBackgroundColor(ColorConstants.GRAY);
            table.AddCell(age);

            Cell surname = new Cell().Add(new Paragraph("SURNAME"));
            surname.SetBackgroundColor(ColorConstants.BLUE);
            table.AddCell(surname);

            Cell firstname = new Cell().Add(new Paragraph("FIRST NAME"));
            firstname.SetBackgroundColor(ColorConstants.RED);
            table.AddCell(firstname);

            Cell middlename = new Cell().Add(new Paragraph("MIDDLE NAME"));
            middlename.SetBackgroundColor(ColorConstants.GREEN);
            table.AddCell(middlename);

            Cell f1 = new Cell().Add(new Paragraph("1"));
            f1.SetBackgroundColor(ColorConstants.PINK);
            table.AddCell(f1);

            Cell f2 = new Cell().Add(new Paragraph("James"));
            f2.SetBackgroundColor(ColorConstants.MAGENTA);
            table.AddCell(f2);

            Cell f3 = new Cell().Add(new Paragraph("Fish"));
            f3.SetBackgroundColor(ColorConstants.ORANGE);
            table.AddCell(f3);

            Cell f4 = new Cell().Add(new Paragraph("Stone"));
            f4.SetBackgroundColor(ColorConstants.DARK_GRAY);
            table.AddCell(f4);

            Cell f5 = new Cell().Add(new Paragraph("17"));
            f5.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddCell(f5);

            doc.Add(table);

            doc.Close();
        }
    }
}