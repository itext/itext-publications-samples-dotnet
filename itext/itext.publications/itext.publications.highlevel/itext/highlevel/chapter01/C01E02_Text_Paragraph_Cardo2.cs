using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;
using static iText.Kernel.Font.PdfFontFactory;


namespace iText.Highlevel.Chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>

    public class C01E02_Text_Paragraph_Cardo2 {
        
        public const String DEST = "../../../results/chapter01/text_paragraph_cardo{0}.pdf";
        public const String REGULAR = "../../../resources/fonts/Cardo-Regular.ttf";
        public const String BOLD = "../../../resources/fonts/Cardo-Bold.ttf";
        public const String ITALIC = "../../../resources/fonts/Cardo-Italic.ttf";

        protected PdfFont font;
        protected PdfFont bold;
        protected PdfFont italic;
        
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            C01E02_Text_Paragraph_Cardo2 app = new C01E02_Text_Paragraph_Cardo2();
            FontProgram fontProgram = FontProgramFactory.CreateFont(REGULAR);
            FontProgram boldProgram = FontProgramFactory.CreateFont(BOLD);
            FontProgram italicProgram = FontProgramFactory.CreateFont(ITALIC);
            for (int i = 0; i < 3; ) {
                app.font = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI, EmbeddingStrategy.PREFER_EMBEDDED);
                app.bold = PdfFontFactory.CreateFont(boldProgram, PdfEncodings.WINANSI, EmbeddingStrategy.PREFER_EMBEDDED);
                app.italic = PdfFontFactory.CreateFont(italicProgram, PdfEncodings.WINANSI, EmbeddingStrategy.PREFER_EMBEDDED);
                app.CreatePdf(String.Format(DEST, ++i));
            }
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));

            // Initialize document
            Document document = new Document(pdf);
            
            // Add content
            Text title = new Text("The Strange Case of Dr. Jekyll and Mr. Hyde").SetFont(bold);
            Text author = new Text("Robert Louis Stevenson").SetFont(font);
            Paragraph p = new Paragraph().SetFont(italic).Add(title).Add(" by ").Add(author);
            document.Add(p);
        
            //Close document
            document.Close();
        }
    }
}
