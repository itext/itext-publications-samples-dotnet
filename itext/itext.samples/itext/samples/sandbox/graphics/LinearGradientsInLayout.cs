using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Colors.Gradients;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Graphics
{
    public class LinearGradientsInLayout
    {
        public static readonly string DEST = "results/sandbox/graphics/linearGradientsInLayout.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LinearGradientsInLayout().ManipulatePdf();
        }

        protected void ManipulatePdf()
        {
            Document doc = new Document(new PdfDocument(new PdfWriter(DEST)));

            AddLinearGradientAsElementBackground(doc);
            CreateColorBasedOnAbsolutelyPositionedLinearGradient(doc);

            doc.Close();
        }

        private void AddLinearGradientAsElementBackground(Document doc)
        {
            doc.Add(new Paragraph("The \"addLinearGradientAsElementBackground\" starts here."));

            AbstractLinearGradientBuilder gradientBuilder = new StrategyBasedLinearGradientBuilder()
                    .AddColorStop(new GradientColorStop(ColorConstants.RED.GetColorValue()))
                    .AddColorStop(new GradientColorStop(ColorConstants.GREEN.GetColorValue()))
                    .AddColorStop(new GradientColorStop(ColorConstants.BLUE.GetColorValue()));
            BackgroundImage backgroundImage = new BackgroundImage(gradientBuilder);

            if (backgroundImage.IsBackgroundSpecified())
            {
                String text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                              "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                              "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
                              "ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit " +
                              "in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                              "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui " +
                              "officia deserunt mollit anim id est laborum. ";

                Div div = new Div().Add(new Paragraph(text + text + text));
                div.SetProperty(Property.BACKGROUND_IMAGE, backgroundImage);
                doc.Add(div);

            }
            else
            {
                Console.Out.WriteLine("There are no linear gradient elements present within the passing image.");
            }
        }

        private void CreateColorBasedOnAbsolutelyPositionedLinearGradient(Document doc)
        {
            // The below such linear gradient spans across the whole page and therefore color created from it will be
            // different based at the location of the page
            AbstractLinearGradientBuilder gradientBuilder = new LinearGradientBuilder()
                    .SetGradientVector(PageSize.A4.GetLeft(), PageSize.A4.GetBottom(), PageSize.A4.GetRight(), PageSize.A4.GetTop())
                    .AddColorStop(new GradientColorStop(ColorConstants.RED.GetColorValue()))
                    .AddColorStop(new GradientColorStop(ColorConstants.PINK.GetColorValue()))
                    .AddColorStop(new GradientColorStop(ColorConstants.BLUE.GetColorValue()));

            Color gradientColor = gradientBuilder.BuildColor(PageSize.A4.Clone(), null, doc.GetPdfDocument());

            doc.Add(new Paragraph("The \"createColorBasedOnAbsolutelyPositionedLinearGradient\" starts here.").SetFontColor(gradientColor));
            String text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In dapibus aliquam quam. Aliquam at tincidunt mauris. "
                    + "Curabitur mollis leo venenatis diam bibendum consectetur. Etiam at lacus ultricies, vulputate dui nec, mattis ex. "
                    + "Sed quis leo in purus consectetur sodales. Nam sit amet felis orci. Aliquam non lacus ut nisi hendrerit sollicitudin "
                    + "a at ligula. Vivamus condimentum vehicula nulla a blandit. In sit amet ex hendrerit augue iaculis consectetur. "
                    + "Etiam semper risus pulvinar, faucibus ex eu, tristique felis. Integer ullamcorper ipsum ac nisi vulputate malesuada."
                    + "Nunc sit amet ipsum sollicitudin, consequat lectus ac, finibus erat. Vivamus malesuada a leo vel consequat."
                    + "Maecenas ac blandit velit, at eleifend ipsum. Praesent dui orci, molestie a semper eu, varius nec augue. "
                    + "Ut vehicula libero ligula, id tristique nisi convallis et. Curabitur nec velit ut nisi commodo rhoncus "
                    + "non eu ipsum. Pellentesque eget mauris ex. Nullam et lectus et eros sollicitudin tincidunt. Phasellus "
                    + "commodo erat nec diam consectetur elementum. Cras pellentesque commodo est, vel viverra nisi "
                    + "vulputate ac. Curabitur interdum nulla at viverra varius. Donec porttitor erat lacus, ac efficitur "
                    + "arcu malesuada dignissim. Aenean pretium ex tortor, a porttitor quam mollis vitae. Etiam id nibh dolor."
                    + " Curabitur vel ligula tortor. Etiam vestibulum velit neque, a mattis tortor vehicula sed. Sed sit amet "
                    + "ipsum leo. Mauris et tincidunt ex. Donec vehicula, magna eget convallis suscipit, nisi tellus ullamcorper "
                    + "massa, eu commodo lectus massa ac orci. Fusce nec gravida justo, ac lacinia metus. Etiam porttitor "
                    + "massa odio, vestibulum semper ipsum tristique eget. Donec semper elit nibh, tristique placerat arcu "
                    + "egestas et. Pellentesque leo metus, sodales in nisi ut, condimentum dignissim eros. Duis maximus eu "
                    + "mi faucibus mattis. Quisque leo urna, hendrerit eu finibus nec, ullamcorper at nunc.";
            Paragraph paragraph = new Paragraph(text).SetBorder(new SolidBorder(gradientColor, 5));

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            Cell cell = new Cell().Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
            table.AddCell(cell);
            table.SetBorder(new SolidBorder(gradientColor, 9));

            doc
                    .Add(paragraph)
                    .Add(table);
        }
    }
}