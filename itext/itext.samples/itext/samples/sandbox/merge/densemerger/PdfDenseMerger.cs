using System;
using System.Collections.Generic;
using iText.Kernel;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Utils;

namespace iText.Samples.Sandbox.Merge.Densemerger
{
    /// <summary>
    /// This class is a tool for merging PDF <em>page contents</em> in a condensed manner,
    /// i.e. if the source pages are only partly filled, the contents of multiple pages
    /// are drawn on a single target page. If on the other hand the content of a source
    /// page do not completely fit onto a target page, the source page content is split
    /// and as much as possible of it is added onto the current target page before a new
    /// page is started.
    /// <para />
    /// In contrast to <see cref="PdfMerger"/>, though, this class does not copy information
    /// beyond page content, in particular it completely ignores annotations and the
    /// structure hierarchy of tagged PDFs.
    /// <para />
    /// Beware: There are numerous situations in which this tool may not create the desired result.
    /// It e.g. should not be used to copy documents with header or footer information as
    /// they will be considered part of the content and so copied and probably moved to
    /// the middle of the page. Furthermore, water marks, text box backgrounds, etc. will
    /// also be considered part of the content and, therefore, also will prevent condensed
    /// merging. The page size of the source document should be equal to the page size
    /// of the resultant document.
    /// </summary>
    public class PdfDenseMerger
    {
        private readonly PdfDocument pdfDocument;

        private PageSize pageSize;
        private float top;
        private float bottom;
        private float gap;

        private PdfCanvas canvas;
        private float yPosition = 0;

        /// <summary>
        /// Creates a PdfDenseMerger to merge content into the passed pdf document.
        /// </summary>
        /// <param name="pdfDocument">The document into which source documents will be merged.</param>
        public PdfDenseMerger(PdfDocument pdfDocument)
        {
            this.pdfDocument = pdfDocument;
            this.pageSize = pdfDocument.GetDefaultPageSize();
        }

        //
        // Getters and setters for layout values
        //

        /// <summary>
        /// Gets the field <c>pageSize</c>.
        /// </summary>
        /// <returns>
        /// a <see cref="iText.Kernel.Geom.PageSize"> object</see>
        /// </returns>
        public PageSize GetPageSize()
        {
            return pageSize;
        }

        /// <summary>
        /// Gets the top margin.
        /// </summary>
        /// <returns>a float.</returns>
        public float GetTopMargin()
        {
            return top;
        }

        /// <summary>
        /// Sets the top margin.
        /// </summary>
        /// <param name="top">a top margin of the resultant document.</param>
        /// <returns>this element.</returns>
        public PdfDenseMerger SetTopMargin(float top)
        {
            this.top = top;
            return this;
        }

        /// <summary>
        /// Gets the bottom margin.
        /// </summary>
        /// <returns>a float.</returns>
        public float GetBottomMargin()
        {
            return bottom;
        }

        /// <summary>
        /// Sets the bottom margin.
        /// </summary>
        /// <param name="bottom">a bottom margin of the resultant document.</param>
        /// <returns>this element.</returns>
        public PdfDenseMerger SetBottomMargin(float bottom)
        {
            this.bottom = bottom;
            return this;
        }

        /// <summary>
        /// Gets the gap.
        /// </summary>
        /// <returns>a float.</returns>
        public float GetGap()
        {
            return gap;
        }

        /// <summary>
        /// Sets the gap between the content of the documents to be merged.
        /// </summary>
        /// <param name="gap">the gap between the content of the documents to be merged.</param>
        /// <returns>this element.</returns>
        public PdfDenseMerger SetGap(float gap)
        {
            this.gap = gap;
            return this;
        }

        /// <summary>
        /// Gets the pdfDocument.
        /// </summary>
        /// <returns>this element.</returns>
        public PdfDocument GetPdfDocument()
        {
            return pdfDocument;
        }

        //
        // Methods controlling the actual merger
        //

        /// <summary>
        /// Adds pages from the source document to the target document.
        /// Note that the page size of the source document is expected
        /// to equal the page size of the resultant document
        /// </summary>
        /// <param name="from">document, from which pages will be copied.</param>
        /// <param name="fromPage">start page in the range of pages to be copied.</param>
        /// <param name="toPage">end page in the range to be copied.</param>
        public void AddPages(PdfDocument from, int fromPage, int toPage)
        {
            for (int pageNum = fromPage; pageNum <= toPage; pageNum++)
            {
                Merge(from, pageNum);
            }
        }

        /// <summary>
        /// Adds pages from the source document to the target document.
        /// Note that the page size of the source document is expected
        /// to equal the page size of the resultant document
        /// </summary>
        /// <param name="from">document, from which pages will be copied.</param>
        /// <param name="pages">list of numbers of pages which will be copied.</param>
        public void AddPages(PdfDocument from, List<int> pages)
        {
            foreach (int pageNum in pages)
            {
                Merge(from, pageNum);
            }
        }

        //
        // Helper methods
        //

        private void Merge(PdfDocument from, int pageNum)
        {
            PdfPage page = from.GetPage(pageNum);
            Rectangle currentPageRect = page.GetPageSize();
            if (!(pageSize.GetHeight() == currentPageRect.GetHeight())
                || !(pageSize.GetWidth() == currentPageRect.GetWidth()))
            {
                throw new PdfException("Page size of the copied page should be the same as "
                                       + "the page size of the resultant document.");
            }

            PdfFormXObject formXObject = page.CopyAsFormXObject(pdfDocument);

            PdfDocumentContentParser contentParser = new PdfDocumentContentParser(from);
            PageVerticalAnalyzer finder = contentParser.ProcessContent(pageNum, new PageVerticalAnalyzer());

            List<float> verticalFlips = finder.GetVerticalFlips();
            if (verticalFlips.Count < 2)
            {
                return;
            }

            Rectangle pageSizeToImport = page.GetPageSize();

            int startFlip = verticalFlips.Count - 1;

            bool first = true;
            while (startFlip > 0)
            {
                if (!first)
                {
                    NewPage();
                }

                float freeSpace = yPosition - (pageSize.GetBottom() + bottom);
                int endFlip = startFlip + 1;
                while ((endFlip > 1) && (verticalFlips[startFlip] - verticalFlips[endFlip - 2] < freeSpace))
                {
                    endFlip -= 2;
                }

                if (endFlip < startFlip)
                {
                    float height = verticalFlips[startFlip] - verticalFlips[endFlip];

                    canvas.SaveState();
                    canvas.Rectangle(0, yPosition - height, pageSizeToImport.GetWidth(), height);
                    canvas.Clip();
                    canvas.EndPath();

                    canvas.AddXObjectAt(formXObject, 0,
                        yPosition - (verticalFlips[startFlip] - pageSizeToImport.GetBottom()));

                    canvas.RestoreState();
                    yPosition -= height + gap;
                    startFlip = endFlip - 1;
                }
                else if (!first)
                {
                    throw new ArgumentException(String.Format("Page {0} content sections too large.", page));
                }

                first = false;
            }
        }

        private void NewPage()
        {
            PdfPage page = pdfDocument.AddNewPage(pageSize);
            canvas = new PdfCanvas(page);
            yPosition = pageSize.GetTop() - top;
        }
    }
}