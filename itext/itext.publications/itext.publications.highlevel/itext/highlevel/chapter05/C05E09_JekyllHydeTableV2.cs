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
    /// <author>iText</author>
    public class C05E09_JekyllHydeTableV2 {
        public const String SRC = "../../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../../results/chapter05/jekyll_hyde_table2.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E09_JekyllHydeTableV2().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 14, 9, 4, 3 }))
                .UseAllAvailableWidth();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            IList<String> header = resultSet[0];
            resultSet.RemoveAt(0);
            foreach (String field in header) {
                table.AddHeaderCell(field);
            }
            foreach (IList<String> record in resultSet) {
                foreach (String field_1 in record) {
                    table.AddCell(field_1);
                }
            }
            Table outerTable = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth()
                .AddHeaderCell("Continued from previous page:").SetSkipFirstHeader(true)
                .AddCell(new Cell().Add(table).SetPadding(0));
            document.Add(outerTable);
            document.Close();
        }
    }
}
