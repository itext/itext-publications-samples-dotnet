using System;
using System.IO;
using iText.Html2pdf;
using iText.StyledXmlParser.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Validation;
using iText.Kernel.XMP;
using iText.Layout.Tagging;
using iText.Pdfa;
using iText.Pdfua.Checkers;
using iText.Test.Pdfa;

namespace iText.Samples.Sandbox.Pdfua {
    public class Wtpdf {
        public static readonly string DEST = "results/sandbox/pdfua/wtpdf.pdf";
        private static readonly string SOURCE_FOLDER = "../../../resources/wtpdf/";


        public static void Main(String[] args) {
            var file = new FileInfo(DEST);
            file.Directory.Create();

            new Wtpdf().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest) {
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


            // By default PdfUADocument has a tag repairing mechanism under the hood, to avoid creating illegal
            // tag structures for example from invalid html, but because PdfADocument as base we have to register it
            // manually
            pdfDocument.GetDiContainer().Register(typeof(ProhibitedTagRelationsResolver),
                new ProhibitedTagRelationsResolver(pdfDocument));
            ValidationContainer container = pdfDocument.GetDiContainer().GetInstance<ValidationContainer>();
            //Because we are using PDF/A4, there will already be a pdf 2.0 checker , so we only need to add the pdf ua checker
            container.AddChecker(new PdfUA2Checker(pdfDocument));


            // Use custom font provider as we only want embedded fonts
            var fontProvider = new BasicFontProvider(false, false, false);
            fontProvider.AddFont(SOURCE_FOLDER + "NotoSans-Regular.ttf");
            fontProvider.AddFont(SOURCE_FOLDER + "NotoEmoji-Regular.ttf");

            var converterProperties = new ConverterProperties()
                .SetBaseUri(SOURCE_FOLDER)
                .SetFontProvider(fontProvider);


            var fs = new FileStream(SOURCE_FOLDER + "article.html", FileMode.Open);

            HtmlConverter.ConvertToPdf(fs, pdfDocument, converterProperties);
            pdfDocument.Close();
            var validator = new VeraPdfValidator();
            if (null != validator.Validate(DEST)) {
                throw new Exception("Should not happen");
            }
        }
    }
}