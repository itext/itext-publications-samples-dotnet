using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tagging
{
    public class CreateTaggedDocument
    {
        public static readonly String DEST = "results/sandbox/tagging/88th_Academy_Awards.pdf";

        private static readonly String TEXT1 =
            "The 88th Academy Awards ceremony, presented by the Academy of Motion Picture Arts and Sciences (AMPAS), " +
            "honored the best films of 2015 and took place on February 28, 2016, at the Dolby Theatre in Hollywood, Los Angeles, beginning " +
            "at 5:30 p.m. PST. During the ceremony, AMPAS presented Academy Awards (commonly referred to as Oscars) in 24 categories. The " +
            "ceremony was televised in the United States by ABC, and produced by David Hill and Reginald Hudlin and directed by Glenn Weiss. " +
            "Actor Chris Rock hosted the show for the second time, having previously hosted the 77th ceremony held in 2005.\n" +
            "In related events, the Academy held its 7th Annual Governors Awards ceremony at the Grand Ballroom of the Hollywood and Highland " +
            "Center on November 14, 2015. On February 13, 2016, in a ceremony at the Beverly Wilshire Hotel in Beverly Hills, California, the " +
            "Academy Awards for Technical Achievement were presented by hosts Olivia Munn and Jason Segel.\n" +
            "Mad Max: Fury Road won six awards, the most for the evening, and Spotlight won two awards including Best Picture. Other winners " +
            "include The Revenant with three awards, and A Girl in the River: The Price of Forgiveness, Amy, Bear Story, The Big Short, Bridge " +
            "of Spies, The Danish Girl, Ex Machina, The Hateful Eight, Inside Out, Room, Son of Saul, Spectre, and Stutterer with one each. The" +
            " telecast garnered more than 34 million viewers in the United States, making it the least watched Oscar ceremony since the 80th Academy Awards in 2008.";

        private static readonly String TEXT2 =
            "In related events, the Academy held its 7th Annual Governors Awards ceremony at the Grand Ballroom of the Hollywood and Highland " +
            "Center on November 14, 2015. On February 13, 2016, in a ceremony at the Beverly Wilshire Hotel in Beverly Hills, California, the Academy " +
            "Awards for Technical Achievement were presented by hosts Olivia Munn and Jason Segel.";

        private static readonly String TEXT3 =
            "Mad Max: Fury Road won six awards, the most for the evening, and Spotlight won two awards including Best Picture. Other winners include " +
            "The Revenant with three awards, and A Girl in the River: The Price of Forgiveness, Amy, Bear Story, The Big Short, Bridge of Spies, The Danish Girl, " +
            "Ex Machina, The Hateful Eight, Inside Out, Room, Son of Saul, Spectre, and Stutterer with one each. The telecast garnered more than 34 million viewers " +
            "in the United States, making it the least watched Oscar ceremony since the 80th Academy Awards in 2008.";

        private static readonly String TEXT4 =
            "The nominees for the 88th Academy Awards were announced on January 14, 2016, at 5:30 a.m. PST " +
            "(13:30 UTC), at the Samuel Goldwyn Theater in Beverly Hills, California, by directors Guillermo del Toro " +
            "and Ang Lee, Academy president Cheryl Boone Isaacs, and actor John Krasinski. The Revenant received the " +
            "most nominations with twelve total, with Mad Max: Fury Road coming in second with ten. For the second consecutive " +
            "year, a film directed by Alejandro G. Iñárritu received the most nominations. Composer Anohni became the second" +
            " transgender person to be nominated for an Oscar. (Angela Morley was the first, in 1974 and 1976.) Sylvester " +
            "Stallone became the sixth person to be nominated for playing the same role in two different films.";

        private static readonly String TEXT5 =
            "The winners were announced during the awards ceremony on February 28, 2016. With two Oscars, Spotlight " +
            "was the first film since The Greatest Show oTagn Earth in 1952 to win Best Picture with only one other award. " +
            "Alejandro G. Iñárritu became the only Mexican and third director to win two consecutive Oscars for Best" +
            " Director after John Ford in 1940-1941 and Joseph L. Mankiewicz in 1949-1950, respectively. At the age of 87," +
            " Ennio Morricone became the oldest winner in Oscar history for a competitive award. Having previously won " +
            "for Gravity and Birdman, Emmanuel Lubezki became the first person to win three consecutive Best " +
            "Cinematography awards. Bear Story became the first ever Chilean film to win an Oscar.";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateTaggedDocument().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // Mark that this document is tagged
            pdfDocument.SetTagged();

            Document document = new Document(pdfDocument);

            // Save some space at the beginning of pages for future additions
            document.SetTopMargin(100);

            // Here we obtain a TagTreePointer which is used internally for auto tagging.
            // In the beginning, this instance points at the root of the tag structure tree.
            // We can move it and as a result all new content will be under the current position of the auto tagging pointer.
            // Auto tagging pointer is also used for tagging annotations and forms, so the same approach could be used there.
            TagTreePointer autoTaggingPointer = pdfDocument.GetTagStructureContext().GetAutoTaggingPointer();

            // Create a new tag, which will be a kid of the root element
            autoTaggingPointer.AddTag(StandardRoles.SECT);

            // Add some content to the page
            Paragraph p = new Paragraph().Add(TEXT1).Add(TEXT2).Add(TEXT3);
            document.Add(p);

            // Create bold font to the header text
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Table table = new Table(UnitValue.CreatePercentArray(2));
            table
                .SetWidth(350)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(new Paragraph("Nominations").SetFont(bold));
            table.AddHeaderCell(new Paragraph("Film").SetFont(bold));
            table.AddCell("12").AddCell("The Revenant");
            table.AddCell("10").AddCell("Mad Max: Fury Road");
            table.AddCell("7").AddCell("The Martian");
            table.AddCell("6").AddCell("Spotlight");
            table.AddCell("5").AddCell("Star Wars: The Force Awakens");
            table.AddCell("4").AddCell("Room");
            table.AddCell("3").AddCell("The Hateful Eight");
            table.AddCell("2").AddCell("Ex Machina");
            document.Add(table);

            // From here we want to create another section of the document.
            // Move the root tag and create a new 'Section' tag
            autoTaggingPointer
                .MoveToParent()
                .AddTag(StandardRoles.SECT);

            p = new Paragraph(TEXT4);
            p.Add(TEXT5);
            document.Add(p);

            table = new Table(UnitValue.CreatePercentArray(2));
            table
                .SetWidth(350)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(new Paragraph("Awards").SetFont(bold));
            table.AddHeaderCell(new Paragraph("Film").SetFont(bold));
            table.AddCell("6").AddCell("Mad Max: Fury Road");
            table.AddCell("3").AddCell("The Revenant");
            table.AddCell("2").AddCell("Spotlight");
            document.Add(table);

            // Layout element roles could be changed, or set to null if you want to omit it in the tag structure.
            // But be aware that if you set null to the role of the tag which immediate kids are page content items,
            // then this content won't be tagged at all (for example if you set role to null for Text element, then the
            // text on the page won't be tagged too).
            Paragraph caption = new Paragraph().SetTextAlignment(TextAlignment.CENTER);
            caption.GetAccessibilityProperties().SetRole(null);
            Text captionText = new Text("Table 2, winners");
            captionText.GetAccessibilityProperties().SetRole(StandardRoles.CAPTION);
            caption.Add(captionText);
            document.Add(caption);

            // By default, root tag has role of 'Document'. Let's change it to 'Part'.
            // Move to the root tag (here we also could have used moveToParent method)
            // and change the role of the tag the pointer points
            autoTaggingPointer
                .MoveToRoot()
                .SetRole(StandardRoles.PART);

            document.Close();
        }
    }
}