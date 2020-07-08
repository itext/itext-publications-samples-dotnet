using System;
using System.IO;
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;

namespace Tutorial.Chapter07 {
    public class C07E03_UnitedStates_PDFA_3a {
        public const String DATA = "../../../resources/data/united_states.csv";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public const String BOLD_FONT = "../../../resources/font/FreeSansBold.ttf";

        public const String INTENT = "../../../resources/color/sRGB_CS_profile.icm";

        public const String DEST = "../../../results/chapter07/united_states_PDFA-3a.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E03_UnitedStates_PDFA_3a().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfADocument pdf = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_3A, new PdfOutputIntent
                ("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", new FileStream(INTENT, FileMode.Open, FileAccess.Read
                )));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            //Setting some required parameters
            pdf.SetTagged();
            pdf.GetCatalog().SetLang(new PdfString("en-US"));
            pdf.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.SetTitle("iText7 PDF/A-3 example");
            //Add attachment
            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdf, File.ReadAllBytes(System.IO.Path.Combine(DATA
                )), "united_states.csv", "united_states.csv", new PdfName("text/csv"), parameters, PdfName.Data);
            fileSpec.Put(new PdfName("AFRelationship"), new PdfName("Data"));
            pdf.AddFileAttachment("united_states.csv", fileSpec);
            PdfArray array = new PdfArray();
            array.Add(fileSpec.GetPdfObject().GetIndirectReference());
            pdf.GetCatalog().Put(new PdfName("AF"), array);
            //Embed fonts
            PdfFont font = PdfFontFactory.CreateFont(FONT, true);
            PdfFont bold = PdfFontFactory.CreateFont(BOLD_FONT, true);
            // Create content
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
                    table.AddHeaderCell(new Cell().SetHorizontalAlignment(HorizontalAlignment.CENTER).Add(new Paragraph(tokenizer
                        .NextToken()).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFont(font)));
                }
                else {
                    table.AddCell(new Cell().SetHorizontalAlignment(HorizontalAlignment.CENTER).Add(new Paragraph(tokenizer.NextToken
                        ()).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFont(font)));
                }
            }
        }
    }
}
