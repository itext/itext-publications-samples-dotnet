/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter04 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C04E07_ListTypes {
        public const String DEST = "../../results/chapter04/list_types.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E07_ListTypes().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            PageSize pagesize = PageSize.A6.Rotate();
            Document document = new Document(pdf, pagesize);
            //Set column parameters
            float offSet = 36;
            float gutter = 23;
            float columnWidth = (pagesize.GetWidth() - offSet * 2) / 2 - gutter;
            float columnHeight = pagesize.GetHeight() - offSet * 2;
            //Define column areas
            Rectangle[] columns = new Rectangle[] { new Rectangle(offSet, offSet, columnWidth, columnHeight), new Rectangle
                (offSet + columnWidth + gutter, offSet, columnWidth, columnHeight) };
            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            List list = new List();
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.DECIMAL);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ENGLISH_LOWER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ENGLISH_UPPER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.GREEK_LOWER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.GREEK_UPPER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ROMAN_LOWER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ROMAN_UPPER);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ZAPF_DINGBATS_1);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ZAPF_DINGBATS_2);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ZAPF_DINGBATS_3);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ZAPF_DINGBATS_4);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            //Close document
            document.Close();
        }
    }
}
