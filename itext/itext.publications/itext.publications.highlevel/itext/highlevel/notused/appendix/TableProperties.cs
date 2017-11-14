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
    public class TableProperties {
        public const String DEST = "results/appendix/table_properties.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TableProperties().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW);
            document.Add(CreateNewTable().AddStyle(style).SetDestination("Top").SetWidth(300).SetHorizontalAlignment(HorizontalAlignment
                .CENTER)).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            document.Add(CreateNewTable().SetBorder(new DottedBorder(5)).SetHyphenation(new HyphenationConfig("en", "uk"
                , 3, 3)));
            document.Add(CreateNewTable().SetTextAlignment(TextAlignment.CENTER));
            document.Add(ListSeparatorProperties.CreateNewSeparator().SetMargin(10).SetWidth(300).SetKeepWithNext(true
                ));
            document.Add(CreateNewTable().SetKeepTogether(true).SetWidth(UnitValue.CreatePercentValue(90)));
            document.Add(CreateNewTable());
            document.Add(CreateNewTable().SetRelativePosition(10, 10, 50, 10));
            document.Add(CreateNewTable());
            document.Add(new AreaBreak());
            document.Add(CreateNewTable().SetFixedPosition(100, 400, 350).SetAction(PdfAction.CreateGoTo("Top")));
            document.Add(new AreaBreak());
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.YELLOW).SetMarginBottom(10));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPaddingLeft(20).SetPaddingRight(50));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargin(50).SetPadding(30));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewTable().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Close();
        }

        public virtual Table CreateNewTable() {
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell("test1");
            table.AddCell("test2");
            table.AddCell("test3");
            table.AddCell("test4");
            table.AddCell("test5");
            table.AddCell("test6");
            table.AddCell("test7");
            table.AddCell("This is a long text snippet that " + "will be used and reused to test paragraph " + "properties. This paragraph should take "
                 + "more than one line. We'll change different " + "properties and then look at the effect " + "when we add the paragraph to the document."
                );
            return table;
        }
    }
}
