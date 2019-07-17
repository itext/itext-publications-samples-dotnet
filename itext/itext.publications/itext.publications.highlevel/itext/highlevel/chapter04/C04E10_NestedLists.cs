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
    public class C04E10_NestedLists {
        public const String DEST = "../../results/chapter04/nested_list.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E10_NestedLists().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
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
            List list1 = new List(ListNumberingType.DECIMAL);
            List listEL = new List(ListNumberingType.ENGLISH_LOWER);
            listEL.Add("Dr. Jekyll");
            listEL.Add("Mr. Hyde");
            ListItem liEL = new ListItem();
            liEL.Add(listEL);
            list1.Add(liEL);
            List listEU = new List(ListNumberingType.ENGLISH_UPPER);
            listEU.Add("Dr. Jekyll");
            listEU.Add("Mr. Hyde");
            ListItem liEU = new ListItem();
            liEU.Add(listEU);
            list1.Add(liEU);
            ListItem li1 = new ListItem();
            li1.Add(list1);
            list.Add(li1);
            ListItem li = new ListItem();
            List listGL = new List(ListNumberingType.GREEK_LOWER);
            listGL.Add("Dr. Jekyll");
            listGL.Add("Mr. Hyde");
            li.Add(listGL);
            List listGU = new List(ListNumberingType.GREEK_UPPER);
            listGU.Add("Dr. Jekyll");
            listGU.Add("Mr. Hyde");
            li.Add(listGU);
            List listRL = new List(ListNumberingType.ROMAN_LOWER);
            listRL.Add("Dr. Jekyll");
            listRL.Add("Mr. Hyde");
            li.Add(listRL);
            List listRU = new List(ListNumberingType.ROMAN_UPPER);
            listRU.Add("Dr. Jekyll");
            listRU.Add("Mr. Hyde");
            li.Add(listRU);
            list.Add(li);
            List listZ1 = new List(ListNumberingType.ZAPF_DINGBATS_1);
            listZ1.Add("Dr. Jekyll");
            listZ1.Add("Mr. Hyde");
            ListItem liZ1 = new ListItem();
            liZ1.Add(listZ1);
            List listZ2 = new List(ListNumberingType.ZAPF_DINGBATS_2);
            listZ2.Add("Dr. Jekyll");
            listZ2.Add("Mr. Hyde");
            ListItem liZ2 = new ListItem();
            liZ2.Add(listZ2);
            List listZ3 = new List(ListNumberingType.ZAPF_DINGBATS_3);
            listZ3.Add("Dr. Jekyll");
            listZ3.Add("Mr. Hyde");
            ListItem liZ3 = new ListItem();
            liZ3.Add(listZ3);
            List listZ4 = new List(ListNumberingType.ZAPF_DINGBATS_4);
            listZ4.Add("Dr. Jekyll");
            listZ4.Add("Mr. Hyde");
            ListItem liZ4 = new ListItem();
            liZ4.Add(listZ4);
            listZ3.Add(liZ4);
            listZ2.Add(liZ3);
            listZ1.Add(liZ2);
            list.Add(liZ1);
            document.Add(list);
            //Close document
            document.Close();
        }
    }
}
