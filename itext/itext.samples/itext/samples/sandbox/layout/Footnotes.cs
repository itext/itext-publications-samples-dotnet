using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties.Margins;

namespace iText.Samples.Sandbox.Layout {

    // Footnotes.cs
    //
    // Example showing how to add footnotes to a document.
    // Demonstrates linking inline text to a footnote at the bottom of the page
    // using FootnoteAnchor, styling the footnote content itself, and configuring
    // footnote numbering and container style via Document#SetFootnotesProperties.

    public class Footnotes
    {
        public static readonly string DEST = "results/sandbox/layout/footnotes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Footnotes().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            // Optional: enable tagging.
            pdfDoc.SetTagged();

            // Configure footnote numbering and the look of the footnotes container.
            // ROMAN_LOWER numbering will be used for the anchor markers (i, ii, iii, ...),
            // numbering restarts on every page, and the footnotes container gets a top border
            // and light background to visually separate it from the rest of the page.
            Style footnotesContainerStyle = new Style()
                    .SetBorderTop(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetBackgroundColor(new DeviceRgb(250, 250, 250))
                    .SetPaddingTop(8);

            FootnotesProperties footnotesProperties = new FootnotesProperties()
                    .SetFootnoteNumberingType(FootnoteNumberingType.ROMAN_LOWER)
                    .SetFootnoteNumberingConfig(FootnoteNumberingConfig.PER_PAGE)
                    .SetFootnotesContainerStyle(footnotesContainerStyle);

            doc.SetFootnotesProperties(footnotesProperties);

            doc.Add(new Paragraph("Footnotes")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            // With FootnotesProperties configured, FootnoteAnchor no longer needs an explicit
            // marker string: the marker is generated automatically based on the numbering type.
            Footnote firstFootnote = new Footnote(
                    "Coffee was first cultivated in Ethiopia and later spread through the Arab world.");
            firstFootnote.SetBorder(new SolidBorder(new DeviceRgb(140, 196, 255), 1));
            firstFootnote.SetBackgroundColor(new DeviceRgb(235, 245, 255));

            FootnoteAnchor firstAnchor = new FootnoteAnchor(firstFootnote);
            firstAnchor.SetFontColor(new DeviceRgb(20, 110, 200));

            Paragraph p1 = new Paragraph()
                    .Add("Coffee")
                    .Add(firstAnchor)
                    .Add(" is one of the most widely consumed beverages in the world.")
                    .SetMarginBottom(15);
            doc.Add(p1);

            // A Footnote can also be built from a Paragraph, allowing the footnote
            // content itself to be styled independently from the surrounding text.
            Footnote secondFootnote = new Footnote(
                    new Paragraph("Decaffeination removes at least 97% of the caffeine content.")
                            .SetFontColor(ColorConstants.WHITE)
                            .SetMargin(0));
            secondFootnote.SetBackgroundColor(new DeviceRgb(255, 150, 90));

            FootnoteAnchor secondAnchor = new FootnoteAnchor(secondFootnote);
            secondAnchor.SetFontColor(new DeviceRgb(220, 100, 40));

            Paragraph p2 = new Paragraph()
                    .Add("Some people prefer decaffeinated coffee")
                    .Add(secondAnchor)
                    .Add(" for the taste without the stimulant effect.")
                    .SetMarginBottom(15);
            doc.Add(p2);

            // Multiple anchors can point to footnotes within the same paragraph;
            // each footnote is rendered independently at the bottom of the page.
            Footnote thirdFootnote = new Footnote(
                    "Espresso is brewed by forcing hot water through finely-ground coffee under pressure.");
            thirdFootnote.SetBorder(new SolidBorder(new DeviceRgb(140, 220, 160), 1));
            thirdFootnote.SetBackgroundColor(new DeviceRgb(235, 250, 238));

            FootnoteAnchor thirdAnchor = new FootnoteAnchor(thirdFootnote);
            thirdAnchor.SetFontColor(new DeviceRgb(40, 150, 80));

            Paragraph p3 = new Paragraph()
                    .Add("Popular brewing methods include drip filtering, French press, and espresso")
                    .Add(thirdAnchor)
                    .Add(".");
            doc.Add(p3);

            // Because PER_PAGE numbering was configured, footnote markers restart from "i"
            // on every new page, instead of continuing as "iv" here.
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Page 2: numbering restarts")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            Footnote fourthFootnote = new Footnote(
                    "Arabica and Robusta are the two most commercially important coffee species.");
            fourthFootnote.SetBorder(new SolidBorder(new DeviceRgb(140, 196, 255), 1));
            fourthFootnote.SetBackgroundColor(new DeviceRgb(235, 245, 255));

            FootnoteAnchor fourthAnchor = new FootnoteAnchor(fourthFootnote);
            fourthAnchor.SetFontColor(new DeviceRgb(20, 110, 200));

            Paragraph p4 = new Paragraph()
                    .Add("There are dozens of coffee species")
                    .Add(fourthAnchor)
                    .Add(", but only a few are grown at commercial scale.")
                    .SetMarginBottom(15);
            doc.Add(p4);

            Footnote fifthFootnote = new Footnote(
                    "Robusta beans generally contain more caffeine than Arabica beans.");
            fifthFootnote.SetBackgroundColor(new DeviceRgb(255, 150, 90));

            FootnoteAnchor fifthAnchor = new FootnoteAnchor(fifthFootnote);
            fifthAnchor.SetFontColor(new DeviceRgb(220, 100, 40));

            Paragraph p5 = new Paragraph()
                    .Add("Robusta is often considered to have a harsher taste")
                    .Add(fifthAnchor)
                    .Add(" than Arabica.");
            doc.Add(p5);

            // A third page shows the marker sequence continuing correctly within the page
            // (here: "i", "ii") before resetting again on the next page.
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Page 3: numbering restarts again")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            Footnote sixthFootnote = new Footnote(
                    "The word 'espresso' comes from the Italian for 'pressed out' or 'expressed'.");
            sixthFootnote.SetBorder(new SolidBorder(new DeviceRgb(140, 220, 160), 1));
            sixthFootnote.SetBackgroundColor(new DeviceRgb(235, 250, 238));

            FootnoteAnchor sixthAnchor = new FootnoteAnchor(sixthFootnote);
            sixthAnchor.SetFontColor(new DeviceRgb(40, 150, 80));

            Paragraph p6 = new Paragraph()
                    .Add("Espresso")
                    .Add(sixthAnchor)
                    .Add(" is the base for many other popular coffee drinks, like cappuccino and latte.");
            doc.Add(p6);

            doc.Close();
        }
    }
}