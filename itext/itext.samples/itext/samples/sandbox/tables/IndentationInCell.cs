using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class IndentationInCell
    {
        public static readonly string DEST = "results/sandbox/tables/indentation_in_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new IndentationInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            
            Cell cell = new Cell().Add(new Paragraph("TO:\n\n   name"));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("TO:\n\n\u00a0\u00a0\u00a0name"));
            table.AddCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("TO:"));
            Paragraph p = new Paragraph("name");
            p.SetMarginLeft(10);
            cell.Add(p);
            table.AddCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("TO:"));
            p = new Paragraph("name");
            p.SetTextAlignment(TextAlignment.RIGHT);
            cell.Add(p);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}