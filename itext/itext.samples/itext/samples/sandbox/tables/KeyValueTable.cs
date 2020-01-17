/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class KeyValueTable
    {
        public static readonly string DEST = "results/sandbox/tables/key_value_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KeyValueTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            UserObject rohit = new UserObject();
            rohit.Name = "Rohit";
            rohit.Id = "6633429";
            rohit.Reputation = 1;
            rohit.JobTitle = "Copy/paste artist";

            UserObject bruno = new UserObject();
            bruno.Name = "Bruno Lowagie";
            bruno.Id = "1622493";
            bruno.Reputation = 42690;
            bruno.JobTitle = "Java Rockstar";

            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            PdfFont regular = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

            document.Add(CreateTable(rohit, bold, regular));
            document.Add(CreateTable(bruno, bold, regular));

            document.Close();
        }

        private static Table CreateTable(UserObject user, PdfFont titleFont, PdfFont defaultFont)
        {
            Table table = new Table(UnitValue.CreatePercentArray(2));
            table.SetWidth(UnitValue.CreatePercentValue(30)).SetMarginBottom(10);
            table.AddHeaderCell(new Cell().SetFont(titleFont).Add(new Paragraph("Key")));
            table.AddHeaderCell(new Cell().SetFont(titleFont).Add(new Paragraph("Value")));

            table.AddCell(new Cell().SetFont(titleFont).Add(new Paragraph("Name")));
            table.AddCell(new Cell().SetFont(defaultFont).Add(new Paragraph(user.Name)));

            table.AddCell(new Cell().SetFont(titleFont).Add(new Paragraph("Id")));
            table.AddCell(new Cell().SetFont(defaultFont).Add(new Paragraph(user.Id)));

            table.AddCell(new Cell().SetFont(titleFont).Add(new Paragraph("Reputation")));
            table.AddCell(new Cell().SetFont(defaultFont).Add(new Paragraph(user.Reputation.ToString())));

            table.AddCell(new Cell().SetFont(titleFont).Add(new Paragraph("Job title")));
            table.AddCell(new Cell().SetFont(defaultFont).Add(new Paragraph(user.JobTitle)));

            return table;
        }

        private class UserObject
        {
            public string Name { set; get; }

            public string Id { set; get; }

            public string JobTitle { set; get; }

            public int Reputation { set; get; }
        }
    }
}