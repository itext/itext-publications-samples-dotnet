/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter05 {
    /// <author>iText</author>
    [WrapToTest]
    public class C05E10_JekyllHydeTableV3 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "results/chapter05/jekyll_hyde_table3.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E10_JekyllHydeTableV3().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            Table table = new Table(new float[] { 3, 2, 14, 9, 4, 3 });
            table.SetWidthPercent(100);
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            IList<String> header = resultSet[0];
            resultSet.RemoveAt(0);
            foreach (String field in header) {
                table.AddHeaderCell(field);
            }
            Cell cell;
            foreach (IList<String> record in resultSet) {
                cell = new Cell();
                FileInfo file = new FileInfo(String.Format("../../resources/img/{0}.jpg", record[0]));
                if (file.Exists) {
                    iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(file.FullName));
                    img.SetAutoScaleWidth(true);
                    cell.Add(img);
                }
                else {
                    cell.Add(record[0]);
                }
                table.AddCell(cell);
                table.AddCell(record[1]);
                table.AddCell(record[2]);
                table.AddCell(record[3]);
                table.AddCell(record[4]);
                table.AddCell(record[5]);
            }
            document.Add(table);
            document.Close();
        }
    }
}
