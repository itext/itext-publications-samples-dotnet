using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Layout
{

    // OrphansWidowsExample.cs
    //
    // Example showing orphans and widows control in multi-column layout.
    // Demonstrates preventing awkward paragraph breaks across columns.
 
    public class OrphansWidowsExample
    {
        public static readonly string DEST = "results/sandbox/layout/orphansWidows.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new OrphansWidowsExample().ManipulatePdf(DEST);
        }
        
        public void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // The document is layouted in columns here in order to improve the visualization of how orphans and widows work.
            // Columns' heights and widths are defined so that several orphans and widows cases occur.
            Rectangle[] columns = GenerateColumns();
            doc.SetRenderer(new ColumnDocumentRenderer(doc, true, columns));

            // Paragraphs alternate background colors to ease the visual distinction between different paragraphs.
            Color[] colors = new Color[] {ColorConstants.LIGHT_GRAY, ColorConstants.GRAY};
            
            IList<String> content = CreateParagraphContents();
            for (int i = 0; i < content.Count; i++) 
            {
                Paragraph para = new Paragraph(content[i])
                    .SetBackgroundColor(colors[i % 2]);

                // Set orphan control property on a paragraph: value of minOrphansNum defines,
                // that at least 2 lines must remain on the area before the break,
                // when paragraph has to be split because of an area break,
                // otherwise the entire paragraph should be pushed to the next area.
                int minOrphansNum = 2;
                para.SetOrphansControl(new ParagraphOrphansControl(minOrphansNum));

                // Set widow control property on a paragraph: when paragraph has to be split
                // because of an area break, widows are to be handled in accordance with the following parameters:
                // value of minWidowsNum defines that at least 2 lines must be overflowed to the next area;
                // value of maxLinesToMove defines that not more than 1 line is allowed to be moved from the area before the break
                // to the next area to ensure that widows constraint isn't violated;
                // value of overflowParagraphOnViolation defines that the paragraph is allowed to be completely pushed to the next area
                // if widows constraint is violated and cannot be automatically fixed.
                // If custom handling of widow violation is needed, create a ParagraphWidowsControl extension
                // and override #handleViolatedWidows(ParagraphRenderer, String) accordingly.
                int minWidowsNum = 2;
                int maxLinesToMove = 1;
                bool overflowParagraphOnViolation = true;
                para.SetWidowsControl(
                        new ParagraphWidowsControl(minWidowsNum, maxLinesToMove, overflowParagraphOnViolation));

                doc.Add(para);
            }
            doc.Close();
        }
        
        private static Rectangle[] GenerateColumns() 
        {
            float margin = 40;
            float height = PageSize.A4.GetHeight() * 0.43f;
            float width = (PageSize.A4.GetWidth() - (2 * margin) - (margin / 2)) / 2;
            float bottom = (PageSize.A4.GetHeight() - height) / 2;
            Rectangle rectLeft = new Rectangle(margin, bottom, width, height);
            Rectangle rectRight = new Rectangle(margin + width + (margin / 2), bottom, width, height);
            return new Rectangle[]{rectLeft, rectRight};
        }
        
        private static IList<String> CreateParagraphContents() 
        {
            // Substrings of different lengths of the following string are used in order to create several orphans and widows cases.
            String content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget " +
                             "dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, " +
                             "nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis," +
                             " sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, " +
                             "vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. " +
                             "Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus " +
                             "elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor " +
                             "eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, " +
                             "feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. " +
                             "Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi." +
                             " Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem " +
                             "quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit " +
                             "vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. " +
                             "Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci " +
                             "eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec " +
                             "sodales sagittis magna. Donec sodales sagittis magna. Donec sodales sagittis magna. " +
                             "Donec sodales sagittis magna. Donec sodales sagittis magna.";
            int[] substringLengths = new int[] {361, 339, 1421, 208, 1245, 1251, 1122, 1270, 1276, 1230, 1420, 1180, 1190, 807};

            IList<String> items = new List<String>(substringLengths.Length);
            for (int i = 0; i < substringLengths.Length; i++)
            {
                items.Add(content.Substring(0, substringLengths[i]));
            }

            return items;
        }
    }
}