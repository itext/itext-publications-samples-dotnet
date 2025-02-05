using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Html;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.Layout;
using iText.Layout.Font;
using iText.Layout.Properties;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class PdfHtmlRenderingMode
    {
        public static readonly String SRC = "../../../resources/pdfhtml/PdfHtmlRenderingMode/";
        public static readonly String DEST = "results/sandbox/pdfhtml/PdfHtmlRenderingMode.pdf";

        public static void Main(string[] args)
        {
            String currentSrc = SRC + "PdfHtmlRenderingMode.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfHtmlRenderingMode().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public void ManipulatePdf(String htmlSource, String pdfDest, String freeFontsDirectory)
        {
            // In pdfHTML the iText layouting mechanism now works in HTML rendering mode. In order
            // to switch to the iText default rendering mode, you should declare a custom ICssApplierFactory
            // in which to create a custom ICssApplier for the body node. Then set the default rendering mode
            // property for the property container.
            ICssApplierFactory customCssApplierFactory = new DefaultModeCssApplierFactory();

            // Starting from pdfHTML version 3.0.0 the GNU Free Fonts family (e.g. FreeSans) that were shipped together with the pdfHTML distribution
            // are now replaced by Noto fonts. If your HTML file contains characters which are not supported by standard PDF fonts (basically most of the
            // characters except latin script and some of the symbol characters, e.g. cyrillic characters like in this sample) and also if no other fonts
            // are specified explicitly by means of iText `FontProvider` class or CSS `@font-face` rule, then pdfHTML shipped fonts covering a wide range
            // of glyphs are used instead. In order to replicate old behavior, one needs to exclude from `FontProvider` the fonts shipped by default and
            // provide GNU Free Fonts instead. GNU Free Fonts can be found at https://www.gnu.org/software/freefont/.
            FontProvider fontProvider = new BasicFontProvider(true, false, false);
            fontProvider.AddDirectory(freeFontsDirectory);

            ConverterProperties converterProperties = new ConverterProperties()
                .SetBaseUri(freeFontsDirectory)
                // Try removing registering of custom DefaultModeCssApplierFactory to compare legacy behavior
                // with the newly introduced. You would notice that now lines spacing algorithm is changed:
                // line spacing is considerably smaller and is closer compared to browsers rendering.
                // You would also notice that now single image in a line behaves according to HTML's "noQuirks" mode:
                // there's an additional "spacing" underneath the image which depends on element's `line-height` and
                // `font-size` CSS properties.
                .SetCssApplierFactory(customCssApplierFactory)
                .SetFontProvider(fontProvider);

            // When converting using the method #convertToElements to change the rendering mode you must also
            // use the flag Property.RENDERING_MODE. However it must be added to the elements from the
            // resulting list before adding these elements to the document. Then the elements will be
            // placed in the specified mode.
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open, FileAccess.Read),
                new FileStream(pdfDest, FileMode.Create), converterProperties);
        }

        private class DefaultModeCssApplierFactory : DefaultCssApplierFactory
        {
            public override ICssApplier GetCustomCssApplier(IElementNode tag)
            {
                if (TagConstants.BODY.Equals(tag.Name()))
                {
                    return new DefaultModeBodyTagCssApplier();
                }

                return null;
            }
        }

        private class DefaultModeBodyTagCssApplier : BodyTagCssApplier
        {
            public override void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
            {
                base.Apply(context, stylesContainer, tagWorker);
                IPropertyContainer container = tagWorker.GetElementResult();
                // Enable default mode
                container.SetProperty(Property.RENDERING_MODE, RenderingMode.DEFAULT_LAYOUT_MODE);
            }
        }
    }
}