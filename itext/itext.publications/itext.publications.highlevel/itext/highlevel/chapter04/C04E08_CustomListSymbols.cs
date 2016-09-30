/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter04 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C04E08_CustomListSymbols {
        public const String DEST = "results/chapter04/custom_list_symbols.pdf";

        public const String INFO = "../../resources/img/test/info.png";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E08_CustomListSymbols().CreatePdf(DEST);
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
            list.SetListSymbol("\u2022");
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List();
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.ZAPFDINGBATS);
            list.SetListSymbol(new Text("*").SetFont(font).SetFontColor(Color.ORANGE));
            list.SetSymbolIndent(10);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            iText.Layout.Element.Image info = new Image(ImageDataFactory.Create(INFO));
            info.ScaleAbsolute(12, 12);
            list = new List().SetSymbolIndent(3);
            list.SetListSymbol(info);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List();
            list.SetListSymbol(ListNumberingType.ENGLISH_LOWER);
            list.SetPostSymbolText("- ");
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.DECIMAL);
            list.SetPreSymbolText("Part ");
            list.SetPostSymbolText(": ");
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.DECIMAL);
            list.SetItemStartIndex(5);
            list.Add("Dr. Jekyll");
            list.Add("Mr. Hyde");
            document.Add(list);
            list = new List(ListNumberingType.ROMAN_LOWER);
            list.SetListSymbolAlignment(ListSymbolAlignment.LEFT);
            for (int i = 0; i < 6; i++) {
                list.Add("Dr. Jekyll");
                list.Add("Mr. Hyde");
            }
            document.Add(list);
            //Close document
            document.Close();
        }
    }
}
