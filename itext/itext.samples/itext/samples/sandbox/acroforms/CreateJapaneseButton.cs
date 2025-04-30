using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // CreateJapaneseButton.cs
    // 
    // This example demonstrates how to create a PDF with a push button containing Japanese text.
    // It uses a custom font to ensure proper rendering of the Japanese characters.
 
    public class CreateJapaneseButton
    {
        public static readonly String DEST = "results/sandbox/acroforms/create_japanese_button.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        // あ き ら characters
        public const String JAPANESE_TEXT = "\u3042\u304d\u3089";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateJapaneseButton().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            // Define the position of a button that measures 108 by 26
            Rectangle rect = new Rectangle(36, 780, 108, 26);
            PdfButtonFormField pushButton = new PushButtonFormFieldBuilder(pdfDoc, "japanese")
                .SetWidgetRectangle(rect).SetCaption(JAPANESE_TEXT).CreatePushButton();
            pushButton.SetFont(font).SetFontSize(12f);
            form.AddField(pushButton);

            pdfDoc.Close();
        }
    }
}