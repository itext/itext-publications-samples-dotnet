/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
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
    public class AddAccepted
    {
        public static readonly String DEST = "results/sandbox/annotations/add_accepted.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello_sticky_note.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddAccepted().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage page = pdfDoc.GetFirstPage();

            PdfAnnotation sticky = page.GetAnnotations()[0];
            Rectangle stickyRectangle = sticky.GetRectangle().ToRectangle();
            PdfAnnotation replySticky = new PdfTextAnnotation(stickyRectangle)
                .SetStateModel(new PdfString("Review"))
                .SetState(new PdfString("Accepted"))
                .SetIconName(new PdfName("Comment"))

                // This method sets an annotation to which the current annotation is "in reply".
                // Both annotations shall be on the same page of the document.
                .SetInReplyTo(sticky)

                // This method sets the text label that will be displayed in the title bar of the annotation's pop-up window
                // when open and active. This entry shall identify the user who added the annotation.
                .SetText(new PdfString("Bruno"))

                // This method sets the text that will be displayed for the annotation or the alternate description,
                // if this type of annotation does not display text.
                .SetContents("Accepted by Bruno")

                // This method sets a complete set of enabled and disabled flags at once. If not set specifically
                // the default value is 0.
                // The argument is an integer interpreted as set of one-bit flags
                // specifying various characteristics of the annotation.
                .SetFlags(sticky.GetFlags() + PdfAnnotation.HIDDEN);
            pdfDoc.GetFirstPage().AddAnnotation(replySticky);

            pdfDoc.Close();
        }
    }
}