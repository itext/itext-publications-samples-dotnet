using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable5
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table5.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable5().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            
            Cell cell = new Cell(1, 5).Add(new Paragraph("Table XYZ (Continued)"));
            table.AddHeaderCell(cell);

            cell = new Cell(1, 5).Add(new Paragraph("Continue on next page"));
            table.AddFooterCell(cell);

            table.SetSkipFirstHeader(true);
            table.SetSkipLastFooter(true);

            for (int i = 0; i < 350; i++)
            {
                table.AddCell((i + 1).ToString());
            }

            doc.Add(table);

            doc.Close();
        }
    }
}