using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class IndentTable
    {
        public static readonly string DEST = "results/sandbox/tables/indent_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new IndentTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfCanvas cb = new PdfCanvas(pdfDoc.AddNewPage());
            cb.MoveTo(36, 842);
            cb.LineTo(36, 0);
            cb.Stroke();

            Table table = new Table(UnitValue.CreatePercentArray(8));
            table.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            table.SetWidth(150);

            for (int aw = 0; aw < 16; aw++)
            {
                table.AddCell(new Cell().Add(new Paragraph("hi")));
            }

            table.SetMarginLeft(25);

            doc.Add(table);

            doc.Close();
        }
    }
}