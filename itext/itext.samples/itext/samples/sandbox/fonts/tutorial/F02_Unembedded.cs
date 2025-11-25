using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts.Tutorial
{
   
    // F02_Unembedded.cs
    //
    // Example showing Czech text with special characters using default fonts.
    // Demonstrates rendering accented characters with Unicode escape sequences.
 
    public class F02_Unembedded
    {
        public static readonly String DEST = "results/sandbox/fonts/tutorial/f02_unembedded.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new F02_Unembedded().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Odkud jste?"));

            // The text line is "Uvidíme se za chvilku. Měj se."
            doc.Add(new Paragraph("Uvid\u00EDme se za chvilku. M\u011Bj se."));

            // The text line is "Dovolte, abych se představil."
            doc.Add(new Paragraph("Dovolte, abych se p\u0159edstavil."));
            doc.Add(new Paragraph("To je studentka."));

            // The text line is "Všechno v pořádku?"
            doc.Add(new Paragraph("V\u0161echno v po\u0159\u00E1dku?"));

            // The text line is "On je inženýr. Ona je lékař."
            doc.Add(new Paragraph("On je in\u017Een\u00FDr. Ona je l\u00E9ka\u0159."));
            doc.Add(new Paragraph("Toto je okno."));

            // The text line is "Zopakujte to prosím"
            doc.Add(new Paragraph("Zopakujte to pros\u00EDm."));

            doc.Close();
        }
    }
}