using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Html;
using iText.Licensing.Base;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Htmlsamples.Chapter05
{
    /// <summary>
    /// Converts an HTML file to a PDF document overriding the CSS of that HTML.
    /// </summary>
    public class C05E06_DutchCss
    {
        public static readonly Dictionary<String, String> KLEUR = new Dictionary<String, String>();

        static C05E06_DutchCss()
        {
            KLEUR.Add("wit", "white");
            KLEUR.Add("zwart", "black");
            KLEUR.Add("rood", "red");
            KLEUR.Add("groen", "green");
            KLEUR.Add("blauw", "blue");
        }

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch05/dutch_css.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/dutch_css.html";

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-html2pdf_typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            C05E06_DutchCss app = new C05E06_DutchCss();
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
        class DutchColorCssApplier : BlockCssApplier
        {
            /* (non-Javadoc)
             * @see iText.Html2pdf.Css.Apply.Impl.BlockCssApplier#apply(iText.Html2pdf.Attach.ProcessorContext, iText.StyledXmlParser.Node.IStylesContainer, iText.Html2pdf.Attach.ITagWorker)
             */
            public override void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
            {
                IDictionary<String, String> cssStyles = stylesContainer.GetStyles();
                if (cssStyles.ContainsKey("kleur"))
                {
                    cssStyles.Add(CssConstants.COLOR, KLEUR[cssStyles["kleur"]]);
                    stylesContainer.SetStyles(cssStyles);
                }

                if (cssStyles.ContainsKey("achtergrond"))
                {
                    cssStyles.Add(CssConstants.BACKGROUND_COLOR, KLEUR[cssStyles["achtergrond"]]);
                    stylesContainer.SetStyles(cssStyles);
                }

                base.Apply(context, stylesContainer, tagWorker);
            }
        }

        private class CustomCssApplierFactory : DefaultCssApplierFactory
        {
            private ICssApplier dutchCssColor = new DutchColorCssApplier();

            public override ICssApplier GetCustomCssApplier(IElementNode tag)
            {
                if (tag.Name().Equals(TagConstants.H1)
                    || tag.Name().Equals(TagConstants.DIV))
                {
                    return dutchCssColor;
                }

                return null;
            }
        }
    }
}