using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts.Tutorial
{
    public class F03_Embedded
    {
        public static readonly String DEST = "results/sandbox/fonts/tutorial/f03_embedded.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new F03_Embedded().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.CP1250, 
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            
            // Font is an inheritable property, setting it on a document implies that this font
            // will be used for document's children, unless they have this property overwritten.
            doc.SetFont(font);

            doc.Add(new Paragraph("Odkud jste?"));

            // The text line is "Uvidíme se za chvilku. Měj se."
            doc.Add(new Paragraph("Uvid\u00edme se za chvilku. M\u011bj se."));

            // The text line is "Dovolte, abych se představil."
            doc.Add(new Paragraph("Dovolte, abych se p\u0159edstavil."));
            doc.Add(new Paragraph("To je studentka."));

            // The text line is "Všechno v pořádku?"
            doc.Add(new Paragraph("V\u0161echno v po\u0159\u00e1dku?"));

            // The text line is "On je inženýr. Ona je lékař."
            doc.Add(new Paragraph("On je in\u017een\u00fdr. Ona je l\u00e9ka\u0159."));
            doc.Add(new Paragraph("Toto je okno."));

            // The text line is "Zopakujte to prosím"
            doc.Add(new Paragraph("Zopakujte to pros\u00edm."));

            doc.Close();
        }
    }
}