using System;
using System.Collections.Generic;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfa;
using iText.StyledXmlParser.Node;
using iText.Test.Pdfa;
using Org.BouncyCastle.Utilities;

namespace iText.Samples.Sandbox.Pdfua
{
    public class Wtpdf
    {
        public static readonly string DEST = "results/sandbox/pdfua/wtpdf.pdf";
        private static readonly string SOURCE_FOLDER = "../../../resources/wtpdf/";


        public static void Main(String[] args)
        {
            var file = new FileInfo(DEST);
            file.Directory.Create();

            new Wtpdf().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            var fileStream =
                new FileStream("../../../resources/data/sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read);

            var outputIntent = new PdfOutputIntent(
                "Custom",
                "",
                "http://www.color.org",
                "sRGB IEC61964-2.1", fileStream);


            var writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0);
            var pdfDocument = new PdfADocument(new PdfWriter(dest, writerProperties),
                PdfAConformance.PDF_A_4, outputIntent);

            // setup the general requirements for a wtpdf document
            var bytes = File.ReadAllBytes(Path.Combine(SOURCE_FOLDER + "simplePdfUA2.xmp"));
            var xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            pdfDocument.SetXmpMetadata(xmpMeta);
            pdfDocument.SetTagged();
            pdfDocument.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDocument.GetCatalog().SetLang(new PdfString("en-US"));
            var info = pdfDocument.GetDocumentInfo();
            info.SetTitle("Well tagged PDF document");

            // Use custom font provider as we only want embedded fonts
            var fontProvider = new DefaultFontProvider(false, false, false);
            fontProvider.AddFont(SOURCE_FOLDER + "NotoSans-Regular.ttf");
            fontProvider.AddFont(SOURCE_FOLDER + "NotoEmoji-Regular.ttf");

            var converterProperties = new ConverterProperties()
                .SetBaseUri(SOURCE_FOLDER)
                // We need the custom factory to set role of children to null instead of P because P as in element of Hn
                // is not allowed by PDF2.0 spec
                .SetTagWorkerFactory(new CustomTagWorkerFactory())
                .SetFontProvider(fontProvider);


            var fs = new FileStream(SOURCE_FOLDER + "article.html", FileMode.Open);

            HtmlConverter.ConvertToPdf(fs, pdfDocument, converterProperties);
            pdfDocument.Close();
            var validator = new VeraPdfValidator();
            if (null != validator.Validate(DEST))
            {
                throw new Exception("Should not happen");
            }
        }


        class CustomTagWorkerFactory : DefaultTagWorkerFactory
        {
            private static readonly HashSet<String> H_TAGS = new HashSet<string>()
                { "h1", "h2", "h3", "h4", "h5", "h6" };

            public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
            {
                if (H_TAGS.Contains(tag.Name()))
                {
                    return new CustomHTagWorker(tag, context);
                }

                return base.GetCustomTagWorker(tag, context);
            }
        }

        class CustomHTagWorker : HTagWorker
        {
            public CustomHTagWorker(IElementNode element, ProcessorContext context) : base(element, context)
            {
            }

            public override IPropertyContainer GetElementResult()
            {
                var elementResult = base.GetElementResult();
                if (!(elementResult is Div result)) return elementResult;
                foreach (IElement child in result.GetChildren())
                {
                    if (child is Paragraph paragraph)
                    {
                        paragraph.SetNeutralRole();
                    }
                }

                return elementResult;
            }
        }
    }
}