using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class FullPageTable
    {
        public static readonly string DEST = "results/sandbox/tables/full_page_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FullPageTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(595, 842));
            doc.SetMargins(0, 0, 0, 0);

            Table table = new Table(new float[10]).UseAllAvailableWidth();
            table.SetMarginTop(0);
            table.SetMarginBottom(0);

            // first row
            Cell cell = new Cell(1, 10).Add(new Paragraph("DateRange"));
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetPadding(5);
            cell.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cell);

            table.AddCell("Calldate");
            table.AddCell("Calltime");
            table.AddCell("Source");
            table.AddCell("DialedNo");
            table.AddCell("Extension");
            table.AddCell("Trunk");
            table.AddCell("Duration");
            table.AddCell("Calltype");
            table.AddCell("Callcost");
            table.AddCell("Site");

            for (int i = 0; i < 100; i++)
            {
                table.AddCell("date" + i);
                table.AddCell("time" + i);
                table.AddCell("source" + i);
                table.AddCell("destination" + i);
                table.AddCell("extension" + i);
                table.AddCell("trunk" + i);
                table.AddCell("dur" + i);
                table.AddCell("toc" + i);
                table.AddCell("callcost" + i);
                table.AddCell("Site" + i);
            }

            doc.Add(table);

            doc.Close();
        }
    }
}