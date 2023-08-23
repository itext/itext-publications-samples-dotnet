using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter05 {
    public class C05E12_JekyllHydeTableV5 {
        public const String SRC = "../../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../../results/chapter05/jekyll_hyde_table5.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E12_JekyllHydeTableV5().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 32 }))
                .UseAllAvailableWidth();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            resultSet.RemoveAt(0);
            table.AddHeaderCell("imdb").AddHeaderCell("Information about the movie");
            Cell cell;
            foreach (IList<String> record in resultSet) {
                table.AddCell(record[0]);
                cell = new Cell().Add(new Paragraph(record[1])).Add(new Paragraph(record[2])).Add(new Paragraph(record[3])
                    ).Add(new Paragraph(record[4])).Add(new Paragraph(record[5]));
                cell.SetKeepTogether(true);
                table.AddCell(cell);
            }
            document.Add(table);
            document.Close();
        }
    }
}
