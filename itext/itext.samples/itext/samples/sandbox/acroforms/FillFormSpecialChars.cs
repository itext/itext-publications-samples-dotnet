using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillFormSpecialChars
    {
        public static readonly String DEST = "results/sandbox/acroforms/fill_form_special_chars.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";
        public static readonly String SRC = "../../../resources/pdfs/test.pdf";

        // ěščřžýáíé characters
        public const String VALUE = "\u011b\u0161\u010d\u0159\u017e\u00fd\u00e1\u00ed\u00e9";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFormSpecialChars().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            // Being set as true, this parameter is responsible to generate an appearance Stream
            // while flattening for all form fields that don't have one. Generating appearances will
            // slow down form flattening, but otherwise Acrobat might render the pdf on its own rules.
            form.SetGenerateAppearance(true);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            form.GetField("test").SetValue(VALUE, font, 12f);
            form.GetField("test2").SetValue(VALUE, font, 12f);

            pdfDoc.Close();
        }
    }
}