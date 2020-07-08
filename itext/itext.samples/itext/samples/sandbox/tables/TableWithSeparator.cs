using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class TableWithSeparator
    {
        public static readonly string DEST = "results/sandbox/tables/table_with_separator.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableWithSeparator().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.AddCell(GetCell1());
            table.AddCell(GetCell2());
            table.AddCell(GetCell3());
            table.AddCell(GetCell4());

            doc.Add(table);
            
            doc.Close();
        }

        private static Cell GetCell1()
        {
            Cell cell = new Cell();
            
            Paragraph p1 = new Paragraph("My fantastic data");
            p1.SetLineThrough();
            cell.Add(p1);

            Paragraph p2 = new Paragraph("Other data");
            cell.Add(p2);

            return cell;
        }

        private static Cell GetCell2()
        {
            Cell cell = new Cell();
            
            Paragraph p1 = new Paragraph("My fantastic data");
            cell.Add(p1);

            LineSeparator ls = new LineSeparator(new SolidLine());
            cell.Add(ls);

            Paragraph p2 = new Paragraph("Other data");
            cell.Add(p2);

            return cell;
        }

        private static Cell GetCell3()
        {
            Cell cell = new Cell();
            
            Paragraph p1 = new Paragraph("My fantastic data");
            cell.Add(p1);

            LineSeparator ls = new LineSeparator(new SolidLine());
            cell.Add(ls);

            Paragraph p2 = new Paragraph("Other data");
            p2.SetFixedLeading(25);
            cell.Add(p2);

            return cell;
        }

        private static Cell GetCell4()
        {
            Cell cell = new Cell();
            
            Paragraph p1 = new Paragraph("My fantastic data");
            p1.SetMarginBottom(20);
            cell.Add(p1);

            LineSeparator ls = new LineSeparator(new SolidLine());
            cell.Add(ls);

            Paragraph p2 = new Paragraph("Other data");
            p2.SetMarginTop(10);
            cell.Add(p2);

            return cell;
        }
    }
}