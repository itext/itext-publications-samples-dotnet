using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables {
    public class SplittingNestedTable1 {
        public static readonly string DEST = "results/sandbox/tables/splitting_nested_table1.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SplittingNestedTable1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(300, 210));

            doc.Add(new Paragraph("Table with setKeepTogether(true):"));
            Table table = CreateTable(true);
            doc.Add(table);

            doc.Add(new AreaBreak());

            doc.Add(new Paragraph("Table with setKeepTogether(false):"));
            table = CreateTable(false);
            doc.Add(table);

            doc.Close();
        }

        /// <summary>Creates a table with two cells, the second of which consists of an inner table.</summary>
        /// <param name="keepTableTogether">bool value which defines whether to keep the table together or not</param>
        /// <returns><see cref="Table"/> with the format specified above</returns>
        private static Table CreateTable(bool keepTableTogether) {
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetMarginTop(10);

            // If true, iText will do its best trying not to split the table and process it on a single area
            table.SetKeepTogether(keepTableTogether);

            Cell cell = new Cell();
            cell.Add(new Paragraph("G"));
            cell.Add(new Paragraph("R"));
            cell.Add(new Paragraph("O"));
            cell.Add(new Paragraph("U"));
            cell.Add(new Paragraph("P"));

            table.AddCell(cell);

            Table inner = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            inner.AddCell("row 1");
            inner.AddCell("row 2");
            inner.AddCell("row 3");
            inner.AddCell("row 4");
            inner.AddCell("row 5");

            cell = new Cell().Add(inner);
            cell.SetPadding(0);
            table.AddCell(cell);

            return table;
        }
    }
}