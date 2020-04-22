using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable11
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table11.pdf";

        private String [][] data =
        {
            new[]
            {
                "ABC123", "The descriptive text may be more than one line and the text should wrap automatically",
                "$5.00", "10", "$50.00"
            },
            new[] {"QRS557", "Another description", "$100.00", "15", "$1,500.00"},
            new[] {"XYZ999", "Some stuff", "$1.00", "2", "$2.00"}
        };
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable11().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 2, 1, 1, 1}));
           
            table.AddCell(CreateCell("SKU", 2, 1, TextAlignment.LEFT));
            table.AddCell(CreateCell("Description", 2, 1, TextAlignment.LEFT));
            table.AddCell(CreateCell("Unit Price", 2, 1, TextAlignment.LEFT));
            table.AddCell(CreateCell("Quantity", 2, 1, TextAlignment.LEFT));
            table.AddCell(CreateCell("Extension", 2, 1, TextAlignment.LEFT));
            
            foreach (String[] row in data)
            {
                table.AddCell(CreateCell(row[0], 1, 1, TextAlignment.LEFT));
                table.AddCell(CreateCell(row[1], 1, 1, TextAlignment.LEFT));
                table.AddCell(CreateCell(row[2], 1, 1, TextAlignment.RIGHT));
                table.AddCell(CreateCell(row[3], 1, 1, TextAlignment.RIGHT));
                table.AddCell(CreateCell(row[4], 1, 1, TextAlignment.RIGHT));
            }

            table.AddCell(CreateCell("Totals", 2, 4, TextAlignment.LEFT));
            table.AddCell(CreateCell("$1,552.00", 2, 1, TextAlignment.RIGHT));

            doc.Add(table);

            doc.Close();
        }

        private static Cell CreateCell(String content, float borderWidth, int colspan, TextAlignment? alignment)
        {
            Cell cell = new Cell(1, colspan).Add(new Paragraph(content));
            cell.SetTextAlignment(alignment);
            cell.SetBorder(new SolidBorder(borderWidth));
            return cell;
        }
    }
}