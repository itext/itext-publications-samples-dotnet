using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class UnequalPages
    {
        public static readonly string DEST = "results/sandbox/objects/unequal_pages.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new UnequalPages().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PageSize pageSizeOne = new PageSize(70, 140);
            PageSize pageSizeTwo = new PageSize(700, 400);
            Paragraph p = new Paragraph("Hi");

            pdfDoc.SetDefaultPageSize(pageSizeOne);
            doc.SetMargins(2, 2, 2, 2);
            doc.Add(p);

            pdfDoc.SetDefaultPageSize(pageSizeTwo);
            doc.SetMargins(20, 20, 20, 20);
            doc.Add(new AreaBreak());
            doc.Add(p);

            doc.Close();
        }
    }
}