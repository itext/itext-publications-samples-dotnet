/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
/*
 * This example was written in the context of the following book:
 * https://leanpub.com/itext7_pdfHTML
 * Go to http://developers.itextpdf.com for more info.
 */

using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.License;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Htmlsamples.Chapter05
{
    /// <summary>
    /// Creates a series of PDF files from HTML that uses some custom tags.
    /// </summary>
    public class C05E01_ATagAsSpan
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch05/no_link.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../resources/htmlsamples/html/2_inline_css.html";

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

            C05E01_ATagAsSpan app = new C05E01_ATagAsSpan();
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
            converterProperties.SetTagWorkerFactory(new CustomTagWorkerFactory());
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        private class CustomTagWorkerFactory : DefaultTagWorkerFactory
        {
            public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
            {
                if ("a".Equals(tag.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return new SpanTagWorker(tag, context);
                }

                return null;
            }
        }
    }
}