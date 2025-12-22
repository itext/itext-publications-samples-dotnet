using System;
using System.IO;
using System.Text;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
   
    // UnembedFont.cs
    //
    // This example demonstrates removing embedded TrueType fonts from an existing PDF document to reduce file size.
    // The sample creates a PDF with an embedded font, then processes it to remove font file data while preserving font references.
 
    public class UnembedFont
    {
        public static readonly String DEST = "results/sandbox/fonts/unembed_font.pdf";

        public static readonly String SRC = "results/sandbox/fonts/withSerifFont.pdf";
        public static readonly String FONT = "../../../resources/font/PT_Serif-Web-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new UnembedFont().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            
            // Create a pdf file with an embedded font in memory.
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(CreatePdf().ToArray())),
                new PdfWriter(dest));

            for (int i = 0; i < pdfDoc.GetNumberOfPdfObjects(); i++)
            {
                PdfObject obj = pdfDoc.GetPdfObject(i);

                // Skip all objects that aren't a dictionary
                if (obj == null || !obj.IsDictionary())
                {
                    continue;
                }

                // Process all dictionaries
                UnembedTTF((PdfDictionary) obj);
            }

            pdfDoc.Close();
        }

        /*
         * Creates a PDF with an embedded font.
         */
        public MemoryStream CreatePdf()
        {
            MemoryStream resultFile = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(resultFile));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            doc.Add(new Paragraph("This is a test with Times New Roman.").SetFont(font));

            doc.Close();
            return resultFile;
        }

        /*
         * Unembeds a font dictionary.
         */
        public void UnembedTTF(PdfDictionary dict)
        {
            // Ignore all dictionaries that aren't font dictionaries
            if (!PdfName.Font.Equals(dict.GetAsName(PdfName.Type)))
            {
                return;
            }

            // Only TTF fonts should be removed
            if (dict.GetAsDictionary(PdfName.FontFile2) != null)
            {
                return;
            }

            // Check if a subset was used (in which case we remove the prefix)
            PdfName baseFont = dict.GetAsName(PdfName.BaseFont);
            if (Encoding.UTF8.GetBytes(baseFont.GetValue())[6] == '+')
            {
                baseFont = new PdfName(baseFont.GetValue().Substring(7));
                dict.Put(PdfName.BaseFont, baseFont);
            }

            // Check if there's a font descriptor
            PdfDictionary fontDescriptor = dict.GetAsDictionary(PdfName.FontDescriptor);
            if (fontDescriptor == null)
            {
                return;
            }

            // Replace the fontname and remove the font file
            fontDescriptor.Put(PdfName.FontName, baseFont);
            fontDescriptor.Remove(PdfName.FontFile2);
        }
    }
}