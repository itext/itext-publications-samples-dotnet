using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Merge
{
    public class MergeWithToc2
    {
        public static readonly String DEST = "results/sandbox/merge/merge_with_toc2.pdf";

        public static readonly String SRC1 = "../../../resources/pdfs/united_states.pdf";
        public static readonly String SRC2 = "../../../resources/pdfs/hello.pdf";
        public static readonly String SRC3 = "../../../resources/pdfs/toc.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeWithToc2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            MemoryStream baos = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            Document doc = new Document(pdfDoc);

            // Copier contains the additional logic to copy acroform fields to a new page.
            // PdfPageFormCopier uses some caching logic which can potentially improve performance
            // in case of the reusing of the same instance.
            PdfPageFormCopier formCopier = new PdfPageFormCopier();

            // Copy all merging file's pages to the temporary pdf file
            Dictionary<String, PdfDocument> filesToMerge = InitializeFilesToMerge();
            Dictionary<int, String> toc = new Dictionary<int, String>();
            int page = 1;
            foreach (KeyValuePair<String, PdfDocument> entry in filesToMerge)
            {
                PdfDocument srcDoc = entry.Value;
                int numberOfPages = srcDoc.GetNumberOfPages();

                toc.Add(page, entry.Key);

                for (int i = 1; i <= numberOfPages; i++, page++)
                {
                    Text text = new Text(String.Format("Page {0}", page));
                    srcDoc.CopyPagesTo(i, i, pdfDoc, formCopier);

                    // Put the destination at the very first page of each merged document
                    if (i == 1)
                    {
                        text.SetDestination("p" + page);

                        PdfOutline rootOutLine = pdfDoc.GetOutlines(false);
                        PdfOutline outline = rootOutLine.AddOutline("p" + page);
                        outline.AddDestination(PdfDestination.MakeDestination(new PdfString("p" + page)));
                    }

                    doc.Add(new Paragraph(text)
                        .SetFixedPosition(page, 549, 810, 40)
                        .SetMargin(0)
                        .SetMultipliedLeading(1));
                }
            }

            PdfDocument tocDoc = new PdfDocument(new PdfReader(SRC3));
            tocDoc.CopyPagesTo(1, 1, pdfDoc, formCopier);
            tocDoc.Close();

            // Create a table of contents
            float tocYCoordinate = 750;
            float tocXCoordinate = doc.GetLeftMargin();
            float tocWidth = pdfDoc.GetDefaultPageSize().GetWidth() - doc.GetLeftMargin() - doc.GetRightMargin();
            foreach (KeyValuePair<int, String> entry in toc)
            {
                Paragraph p = new Paragraph();
                p.AddTabStops(new TabStop(500, TabAlignment.LEFT, new DashedLine()));
                p.Add(entry.Value);
                p.Add(new Tab());
                p.Add(entry.Key.ToString());
                p.SetAction(PdfAction.CreateGoTo("p" + entry.Key));
                doc.Add(p
                    .SetFixedPosition(pdfDoc.GetNumberOfPages(), tocXCoordinate, tocYCoordinate, tocWidth)
                    .SetMargin(0)
                    .SetMultipliedLeading(1));

                tocYCoordinate -= 20;
            }

            foreach (PdfDocument srcDocument in filesToMerge.Values)
            {
                srcDocument.Close();
            }

            doc.Close();

            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            PdfDocument srcPdfDoc = new PdfDocument(new PdfReader(new MemoryStream(baos.ToArray()),
                new ReaderProperties()));
            srcPdfDoc.InitializeOutlines();

            // Create a copy order list and set the page with a table of contents as the first page
            int tocPageNumber = srcPdfDoc.GetNumberOfPages();
            List<int> copyPagesOrderList = new List<int>();
            copyPagesOrderList.Add(tocPageNumber);
            for(int i = 1; i < tocPageNumber; i++) {
                copyPagesOrderList.Add(i);
            }

            srcPdfDoc.CopyPagesTo(copyPagesOrderList, resultDoc, formCopier);

            srcPdfDoc.Close();
            resultDoc.Close();
        }

        private static Dictionary<String, PdfDocument> InitializeFilesToMerge()
        {
            Dictionary<String, PdfDocument> filesToMerge = new Dictionary<String, PdfDocument>();
            filesToMerge.Add("01 Countries", new PdfDocument(new PdfReader(SRC1)));
            filesToMerge.Add("02 Hello World", new PdfDocument(new PdfReader(SRC2)));
            return filesToMerge;
        }
    }
}