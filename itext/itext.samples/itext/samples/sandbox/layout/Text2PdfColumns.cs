using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Layout
{
    public class Text2PdfColumns
    {
        public static readonly String TEXT = "../../../resources/txt/tree.txt";
        public static readonly String DEST = "results/sandbox/layout/text2pdf_columns.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Text2PdfColumns().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            document.SetTextAlignment(TextAlignment.JUSTIFIED);
            Rectangle[] columns =
            {
                new Rectangle(36, 36, 254, 770),
                new Rectangle(305, 36, 254, 770)
            };

            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            ParseTextAndFillDocument(document, TEXT);

            document.Close();
        }

        private static void ParseTextAndFillDocument(Document doc, String filePath)
        {
            using (StreamReader br = new StreamReader(filePath))
            {
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                bool title = true;
                String line;
                while ((line = br.ReadLine()) != null)
                {
                    Paragraph paragraph;
                    if (title)
                    {
                        
                        // If the text line is a title, then set a bold font
                        paragraph = new Paragraph(line).SetFont(bold);
                    }
                    else
                    {
                        
                        // If the text line is not a title, then set a normal font
                        paragraph = new Paragraph(line).SetFont(normal);
                    }

                    doc.Add(paragraph);
                    title = line.Equals("");
                }
            }
        }
    }
}