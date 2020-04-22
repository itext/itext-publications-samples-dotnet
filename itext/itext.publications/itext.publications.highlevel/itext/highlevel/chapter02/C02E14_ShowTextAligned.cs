using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter02 {
    /// <author>iText</author>
    public class C02E14_ShowTextAligned {
        public const String DEST = "../../../results/chapter02/showtextaligned.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E14_ShowTextAligned().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph title = new Paragraph("The Strange Case of Dr. Jekyll and Mr. Hyde");
            document.ShowTextAligned(title, 36, 806, TextAlignment.LEFT);
            Paragraph author = new Paragraph("by Robert Louis Stevenson");
            document.ShowTextAligned(author, 36, 806, TextAlignment.LEFT, VerticalAlignment.TOP);
            document.ShowTextAligned("Jekyll", 300, 800, TextAlignment.CENTER, 0.5f * (float)Math.PI);
            document.ShowTextAligned("Hyde", 300, 800, TextAlignment.CENTER, -0.5f * (float)Math.PI);
            document.ShowTextAligned("Jekyll", 350, 800, TextAlignment.CENTER, VerticalAlignment.TOP, 0.5f * (float)Math
                .PI);
            document.ShowTextAligned("Hyde", 350, 800, TextAlignment.CENTER, VerticalAlignment.TOP, -0.5f * (float)Math
                .PI);
            document.ShowTextAligned("Jekyll", 400, 800, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 0.5f * (float
                )Math.PI);
            document.ShowTextAligned("Hyde", 400, 800, TextAlignment.CENTER, VerticalAlignment.MIDDLE, -0.5f * (float)
                Math.PI);
            document.Close();
        }
    }
}
