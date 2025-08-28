using System;
using System.IO;
using iText.Pdfocr;
using iText.Pdfocr.Tesseract4;

namespace iText.Samples.Sandbox.Pdfocr.Tesseract4 {
    /// <summary>PdfOcrTesseractPdfAsInputExample.java</summary>
    /// <remarks>
    /// PdfOcrTesseractPdfAsInputExample.java
    /// <para />
    /// This example demonstrates how to perform OCR of all images in an input PDF file
    /// and generate searchable PDF using provided
    /// <see cref="AbstractTesseract4OcrEngine"/>.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-Tesseract4 4.1.0. Requires Tesseract installation,
    /// see <a href="https://tesseract-ocr.github.io/tessdoc/home.html">Tesseract User Manual</a>,
    /// Leptonica library is required on Linux operating systems.
    /// </remarks>
    public class PdfOcrTesseractPdfAsInputExample {
        public const String DEST = "results/sandbox/pdfocr/tesseract4/PdfOcrTesseractPdfAsInputExample/result.pdf";

        // Directory with trained data for tests
        protected internal const String LANG_TESS_DATA_DIRECTORY = "../../../resources/tessdata";

        private const String PDF = "../../../resources/pdfs/numbers.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrTesseractPdfAsInputExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            AbstractTesseract4OcrEngine ocrEngine = new Tesseract4ExecutableOcrEngine(new Tesseract4OcrEngineProperties
                ().SetPathToTessData(GetTessDataDirectory()).SetTextPositioning(TextPositioning.BY_WORDS_AND_LINES));
            OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
            pdfCreator.MakePdfSearchable(new FileInfo(PDF), new FileInfo(DEST));
        }

        protected internal static FileInfo GetTessDataDirectory() {
            return new FileInfo(LANG_TESS_DATA_DIRECTORY);
        }
    }
}
