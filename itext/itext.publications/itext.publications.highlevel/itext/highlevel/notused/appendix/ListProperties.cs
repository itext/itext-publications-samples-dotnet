/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>iText</author>
    public class ListProperties {
        public const String DEST = "results/appendix/list_properties.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListProperties().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW).SetTextAlignment(TextAlignment.CENTER);
            document.Add(CreateNewList().AddStyle(style).SetWidth(300).SetHorizontalAlignment(HorizontalAlignment.CENTER
                ).SetDestination("Top"));
            document.Add(CreateNewList().SetRotationAngle(Math.PI / 18).SetFont(font).SetFontSize(8).SetFontColor(ColorConstants
                .RED));
            document.Add(CreateNewList().SetHyphenation(new HyphenationConfig("en", "uk", 3, 3)).SetBorder(new SolidBorder
                (0.5f)).SetKeepWithNext(true));
            document.Add(CreateNewList().SetKeepTogether(true).SetHeight(200));
            document.Add(CreateNewList().SetWidth(UnitValue.CreatePercentValue(50)));
            document.Add(CreateNewList().SetRelativePosition(10, 10, 50, 10));
            document.Add(CreateNewList());
            document.Add(new AreaBreak());
            document.Add(CreateNewList().SetFixedPosition(100, 400, 350).SetAction(PdfAction.CreateGoTo("Top")));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.YELLOW).SetMarginBottom(10));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPaddingLeft(20).SetPaddingRight(50));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargin(50).SetPadding(30));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewList().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Close();
        }

        public static List CreateNewList() {
            List list = new List();
            list.Add("item 1");
            list.Add("item 2");
            list.Add("item 3");
            list.Add("item 4");
            list.Add("item 5");
            list.Add("item 6");
            list.Add("This is a long text snippet that " + "will be used and reused to test paragraph " + "properties. This paragraph should take "
                 + "more than one line. We'll change different " + "properties and then look at the effect " + "when we add the paragraph to the document."
                );
            return list;
        }
    }
}
