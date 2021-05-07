using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tutorial.Chapter01 {
    /// <summary>Simple table example.</summary>
    public class C01E04_UnitedStates {
        public const String DATA = "../../../resources/data/united_states.csv";

        public const String DEST = "../../../results/chapter01/united_states.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E04_UnitedStates().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1, 3, 4, 3, 3, 3, 3, 1 }))
                .UseAllAvailableWidth();
            using (StreamReader sr = File.OpenText(DATA))
            {
                String line = sr.ReadLine();
                Process(table, line, bold, true);
                while ((line = sr.ReadLine()) != null)
                {
                    Process(table, line, font, false);
                }
            }

            document.Add(table);
            //Close document
            document.Close();
        }

        public virtual void Process(Table table, String line, PdfFont font, bool isHeader) {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            while (tokenizer.HasMoreTokens()) {
                if (isHeader) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
                else {
                    table.AddCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
            }
        }
    }
}
