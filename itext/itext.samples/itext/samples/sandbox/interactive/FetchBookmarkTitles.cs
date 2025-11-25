using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Interactive
{
   
    // FetchBookmarkTitles.cs
    //
    // Example showing how to extract bookmark titles from a PDF document.
    // Demonstrates recursive traversal of outline tree and output to file.
 
    public class FetchBookmarkTitles
    {
        public static readonly String DEST = "results/txt/bookmarks.txt";

        public static readonly String SRC = "../../../resources/pdfs/bookmarks.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FetchBookmarkTitles().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));

            // This method returns a complete outline tree of the whole document.
            // If the flag is false, the method gets cached outline tree (if it was cached
            // via calling getOutlines method before).
            PdfOutline outlines = pdfDoc.GetOutlines(false);
            IList<PdfOutline> bookmarks = outlines.GetAllChildren();

            pdfDoc.Close();

            List<String> titles = new List<String>();
            foreach (PdfOutline bookmark in bookmarks)
            {
                AddTitle(bookmark, titles);
            }

            // See title's names in the console
            foreach (String title in titles)
            {
                Console.WriteLine(title);
            }

            CreateResultTxt(dest, titles);
        }

        // This recursive method calls itself if an examined bookmark entry has kids.
        // The method writes bookmark title to the passed list
        private void AddTitle(PdfOutline outline, List<String> result)
        {
            String bookmarkTitle = outline.GetTitle();
            result.Add(bookmarkTitle);

            IList<PdfOutline> kids = outline.GetAllChildren();
            if (kids != null)
            {
                foreach (PdfOutline kid in kids)
                {
                    AddTitle(kid, result);
                }
            }
        }

        private void CreateResultTxt(String dest, List<String> titles)
        {
            using (StreamWriter writer = new StreamWriter(dest))
            {
                for (int i = 0; i < titles.Count; i++)
                {
                    writer.Write("Title " + i + ": " + titles[i] + "\n");
                }
            }
        }
    }
}