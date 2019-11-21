/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddMarked
    {
        public static readonly String DEST = "results/sandbox/annotations/add_marked.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello_sticky_note.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddMarked().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage firstPage = pdfDoc.GetFirstPage();

            PdfAnnotation sticky = firstPage.GetAnnotations()[0];
            Rectangle stickyRectangle = sticky.GetRectangle().ToRectangle();
            PdfAnnotation replySticky = new PdfTextAnnotation(stickyRectangle)

                // This method specifies whether initially the annotation is displayed open or not.
                .SetOpen(false)
                .SetStateModel(new PdfString("Marked"))
                .SetState(new PdfString("Marked"))
                .SetIconName(new PdfName("Comment"))

                // This method sets an annotation to which the current annotation is "in reply".
                // Both annotations shall be on the same page of the document.
                .SetInReplyTo(sticky)

                // This method sets the text label that will be displayed in the title bar of the annotation's pop-up window
                // when open and active. This entry will identify the user who added the annotation.
                .SetText(new PdfString("Bruno"))

                // This method sets the text that will be displayed for the annotation or the alternate description,
                // if this type of annotation does not display text.
                .SetContents("Marked set by Bruno")

                // This method sets a complete set of enabled and disabled flags at once. If not set specifically
                // the default value is 0.
                // The argument is an integer interpreted as set of one-bit flags
                // specifying various characteristics of the annotation.
                .SetFlags(sticky.GetFlags() + PdfAnnotation.HIDDEN);
            firstPage.AddAnnotation(replySticky);
            pdfDoc.Close();
        }
    }
}