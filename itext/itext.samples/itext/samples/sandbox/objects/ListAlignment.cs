using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class ListAlignment
    {
        public static readonly string DEST = "results/sandbox/objects/list_alignment.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListAlignment().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            List list = new List();

            string text = "test 1 2 3 ";
            for (int i = 0; i < 5; i++)
            {
                text = text + text;
            }

            ListItem item = new ListItem(text);
            item.SetTextAlignment(TextAlignment.JUSTIFIED);
            list.Add(item);

            text = "a b c align ";
            for (int i = 0; i < 5; i++)
            {
                text = text + text;
            }

            item = new ListItem(text);
            item.SetTextAlignment(TextAlignment.JUSTIFIED);
            list.Add(item);

            text = "supercalifragilisticexpialidociousss ";
            for (int i = 0; i < 3; i++)
            {
                text = text + text;
            }

            item = new ListItem(text);
            item.SetTextAlignment(TextAlignment.JUSTIFIED);
            list.Add(item);

            doc.Add(list);

            doc.Close();
        }
    }
}