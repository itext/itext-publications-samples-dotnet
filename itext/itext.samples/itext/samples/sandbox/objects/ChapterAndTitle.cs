/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ChapterAndTitle
    {
        public static readonly string DEST = "results/sandbox/objects/chapter_and_title.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ChapterAndTitle().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            string titleDestination = "title";

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLDOBLIQUE);
            Paragraph title = new Paragraph("This is the title with the font HELVETICA 16")
                .SetFont(font).SetFontSize(16);
            title.SetDestination(titleDestination);
            doc.Add(title);

            // It is an alternative for iText5 Chapter class, because
            // iText5 Chapter class also creates bookmarks automatically.
            PdfOutline root = pdfDoc.GetOutlines(false);
            root.AddOutline("This is the title")
                .AddDestination(PdfDestination.MakeDestination(new PdfString(titleDestination)));

            Paragraph p = new Paragraph("This is the paragraph with the default font");
            doc.Add(p);

            doc.Close();
        }
    }
}