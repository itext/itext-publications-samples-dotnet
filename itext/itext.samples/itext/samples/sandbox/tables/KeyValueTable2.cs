using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class KeyValueTable2
    {
        public static readonly string DEST = "results/sandbox/tables/key_value_table2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KeyValueTable2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            PdfFont regular = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

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

            document.Add(CreateTable(rohit, bruno, bold, regular));

            document.Close();
        }

        private static Table CreateTable(UserObject user1, UserObject user2, PdfFont bold, PdfFont regular)
        {
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(bold).Add(new Paragraph("Name:")));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user1.Name)));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user2.Name)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(bold).Add(new Paragraph("Id:")));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user1.Id)));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user2.Id)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(bold).Add(new Paragraph("Reputation:")));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular)
                .Add(new Paragraph(user1.Reputation.ToString())));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular)
                .Add(new Paragraph(user2.Reputation.ToString())));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(bold).Add(new Paragraph("Job title:")));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user1.JobTitle)));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetFont(regular).Add(new Paragraph(user2.JobTitle)));

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