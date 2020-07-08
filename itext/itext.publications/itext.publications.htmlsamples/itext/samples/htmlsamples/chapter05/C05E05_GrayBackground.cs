using System;
using System.Collections.Generic;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Css.Apply.Util;
using iText.Html2pdf.Html;
using iText.Layout;
using iText.License;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Htmlsamples.Chapter05
{
    /// <summary>
    /// Converts an HTML file to a PDF document overriding the CSS of that HTML.
    /// </summary>
    public class C05E05_GrayBackground
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch05/sxsw_grayBackground.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/sxsw.html";

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-html2pdf_typography.xml");
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            C05E05_GrayBackground app = new C05E05_GrayBackground();
            app.CreatePdf(SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String dest)
        {
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetCssApplierFactory(new CustomCssApplierFactory());
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        /// <summary>
        /// A custom implementation of the BlockCssApplier that will change the background color
        /// to gray, no matter which color was defined in the CSS of the HTML file.
        /// </summary>
        class GrayBackgroundBlockCssApplier : ICssApplier
        {
            /* (non-Javadoc)
             * @see iText.Html2pdf.Css.Apply.ICssApplier#apply(iText.Html2pdf.Attach.ProcessorContext, iText.StyledXmlParser.Node.IStylesContainer, iText.Html2pdf.Attach.ITagWorker)
             */
            public void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
            {
                IDictionary<String, String> cssProps = stylesContainer.GetStyles();
                IPropertyContainer container = tagWorker.GetElementResult();
                if (container != null && cssProps.ContainsKey(CssConstants.BACKGROUND_COLOR))
                {
                    cssProps.Remove(CssConstants.BACKGROUND_COLOR);
                    cssProps.Add(CssConstants.BACKGROUND_COLOR, "#dddddd");
                    BackgroundApplierUtil.ApplyBackground(cssProps, context, container);
                }
            }
        }

        private class CustomCssApplierFactory : DefaultCssApplierFactory
        {
            private ICssApplier grayBackground = new GrayBackgroundBlockCssApplier();

            public override ICssApplier GetCustomCssApplier(IElementNode tag)
            {
                if (tag.Name().Equals(TagConstants.DIV))
                {
                    return grayBackground;
                }

                return null;
            }
        }
    }
}