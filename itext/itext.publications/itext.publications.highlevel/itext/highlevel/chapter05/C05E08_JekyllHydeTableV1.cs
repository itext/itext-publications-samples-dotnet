/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
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
    public class C05E08_JekyllHydeTableV1 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../results/chapter05/jekyll_hyde_table1.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E08_JekyllHydeTableV1().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 14, 9, 4, 3 })).UseAllAvailableWidth();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");

            IList<String> header = resultSet[0];
            resultSet.RemoveAt(0);
            foreach (String field in header) {
                table.AddHeaderCell(field);
            }
            Cell cell = new Cell(1, 6).Add(new Paragraph("Continued on next page..."));
            table.AddFooterCell(cell).SetSkipLastFooter(true);
            foreach (IList<String> record in resultSet) {
                foreach (String field_1 in record) {
                    table.AddCell(field_1);
                }
            }
            document.Add(table);
            document.Close();
        }
    }
}
