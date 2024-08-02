using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;
using iText.Test.Pdfa;

namespace iText.Samples.Sandbox.Pdfa
{
    public class PdfA4
    {
        public static readonly string DEST = "results/sandbox/pdfa/pdf_a4.pdf";


        public static readonly String IMG = "../../../resources/img/hero.jpg";

        public static readonly String FONT = "../../../resources/font/OpenSans-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfA4().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            Stream fileStream =
                new FileStream("../../../resources/data/sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read);
            var pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_3B,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", fileStream));


            var document = new Document(pdfDoc);
            var logoImage = new Image(ImageDataFactory.Create(IMG));
            document.Add(logoImage);


            // PDF/A spec requires font to be embedded, iText will warn you if you do something against the PDF/A4 spec
            var font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            var element = new Paragraph("Hello World")
                .SetFont(font)
                .SetFontSize(10);
            document.Add(element);

            pdfDoc.Close();
            if (null != new VeraPdfValidator().Validate(dest))
            {
                throw new Exception("Pdfdocument should be compliant");
            }
        }
    }
}