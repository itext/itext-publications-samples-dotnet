using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
   
    // ListInColumn.cs
    //
    // Example showing how to add a numbered list within a column layout.
    // Demonstrates using ColumnDocumentRenderer for constrained list placement.
 
    public class ListInColumn
    {
        public static readonly string DEST = "results/sandbox/objects/list_in_column.pdf";
        public static readonly string SRC = "../../../resources/pdfs/pages.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListInColumn().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            while (pdfDoc.GetNumberOfPages() > 2)
            {
                pdfDoc.RemovePage(pdfDoc.GetLastPage());
            }

            Document doc = new Document(pdfDoc);
            doc.SetRenderer(new ColumnDocumentRenderer(doc, new Rectangle[] {new Rectangle(250, 400, 250, 406)}));

            List list = new List(ListNumberingType.DECIMAL);
            for (int i = 0; i < 10; i++)
            {
                list.Add("This is a list item. It will be repeated a number of times. "
                         + "This is done only for test purposes. "
                         + "I want a list item that is distributed over several lines.");
            }

            doc.Add(list);

            doc.Close();
        }
    }
}