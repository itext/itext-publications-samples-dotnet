using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Validation;
using iText.Kernel.XMP;
using iText.Layout.Tagging;
using iText.Pdfa;
using iText.Pdfua.Wtpdf;
using iText.Pdfua.Checkers;
using iText.Samples.Sandbox.Pdfua;
using iText.StyledXmlParser.Resolver.Font;
using iText.Test.Pdfa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iText.Samples.Sandbox.Wtpdf {

    // SimpleWtpdf.cs
    //
    // Example showing how to create well tagged pdf for reuse compliant document.
    // Demonstrates HTML to PDF conversion.
    internal class SimpleWtpdf {
        public static readonly String DEST = "results/sandbox/wtpdf/pdf_wtpdf.pdf";
        public static readonly String ARTICLE_SOURCE_FOLDER = "../../../resources/articledata/";

        public static void Main(String[] args) {
            var file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleWtpdf().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest) {
            PdfWriter writer = new PdfWriter(FileUtil.GetFileOutputStream(dest), new WriterProperties().
                SetPdfVersion(PdfVersion.PDF_2_0));

            //Just use WellTaggedPdfDocument class, it's that simple!
            PdfDocument pdf = new WellTaggedPdfDocument(writer, new WellTaggedPdfConfig(WellTaggedPdfConformance.FOR_REUSE,
                "well tagged pdf", "en-US"));

            BasicFontProvider fontProvider = new BasicFontProvider(false, false, false);
            fontProvider.AddFont(ARTICLE_SOURCE_FOLDER + "NotoSans-Regular.ttf");
            fontProvider.AddFont(ARTICLE_SOURCE_FOLDER + "NotoEmoji-Regular.ttf");

            ConverterProperties props = new ConverterProperties().SetBaseUri(ARTICLE_SOURCE_FOLDER)
                .SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(FileUtil.GetInputStreamForFile(ARTICLE_SOURCE_FOLDER + "article.html"), pdf, props);
            pdf.Close();
        }
    }
}
