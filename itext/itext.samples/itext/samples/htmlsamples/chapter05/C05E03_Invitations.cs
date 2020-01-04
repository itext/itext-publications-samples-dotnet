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
    public class C05E03_Invitations
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch05/invitation{0}.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../resources/htmlsamples/html/invitation.html";

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

            C05E03_Invitations app = new C05E03_Invitations();
            String[] names = {"Bruno Lowagie", "Ingeborg Willaert", "John Doe"};
            int counter = 1;
            foreach (String name in names)
            {
                app.CreatePdf(name, SRC, String.Format(DEST, counter++));
            }
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="name">the name that will be used to replace the content of a custom tag</param>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String name, String src, String dest)
        {
            String sdf = DateTime.Now.ToString("MMMM d, yyy");
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetTagWorkerFactory(new CustomTagWorkerFactory(name, sdf));
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        private class CustomTagWorkerFactory : DefaultTagWorkerFactory
        {
            private String name;
            private String sdf;

            public CustomTagWorkerFactory(String name, String sdf)
            {
                this.name = name;
                this.sdf = sdf;
            }

            public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
            {
                if ("name".Equals(tag.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CustomSpanTagWorker(tag, context, name);
                }
                else if ("date".Equals(tag.Name(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CustomSpanTagWorker(tag, context, sdf);
                }

                return null;
            }
        }

        private class CustomSpanTagWorker : SpanTagWorker
        {
            private String text;

            public CustomSpanTagWorker(IElementNode element, ProcessorContext context, String text)
                : base(element, context)
            {
                this.text = text;
            }

            public override bool ProcessContent(String content, ProcessorContext context)
            {
                return base.ProcessContent(text, context);
            }
        }
    }
}