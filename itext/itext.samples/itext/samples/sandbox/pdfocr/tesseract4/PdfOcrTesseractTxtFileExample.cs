using System;
using System.Collections.Generic;
using System.IO;
using iText.Pdfocr.Tesseract4;

namespace iText.Samples.Sandbox.Pdfocr.Tesseract4 {
    /// <summary>PdfOcrTesseractTxtFileExample.java</summary>
    /// <remarks>
    /// PdfOcrTesseractTxtFileExample.java
    /// <para />
    /// This example demonstrates how to perform OCR using provided
    /// <see cref="AbstractTesseract4OcrEngine"/>
    /// for the given list of input images and save output to a text file using provided path.
    /// <para />
    /// Software: iText 9.0.0, pdfOCR-Tesseract4 4.0.0. Requires Tesseract installation,
    /// see <a href="https://tesseract-ocr.github.io/tessdoc/home.html">Tesseract User Manual</a>,
    /// Leptonica library is required on Linux operating systems.
    /// </remarks>
    public class PdfOcrTesseractTxtFileExample {
        public const String DEST = "results/sandbox/pdfocr/tesseract4/PdfOcrTesseractTxtFileExample/ocr_result.txt";

        // Directory with trained data for tests
        protected internal const String LANG_TESS_DATA_DIRECTORY = "../../../resources/tessdata";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrTesseractTxtFileExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { new FileInfo(BASIC_IMAGE) };
            AbstractTesseract4OcrEngine ocrEngine = new Tesseract4ExecutableOcrEngine(new Tesseract4OcrEngineProperties
                ().SetPathToTessData(GetTessDataDirectory()));
            ocrEngine.CreateTxtFile(images, new FileInfo(DEST));
        }

        protected internal static FileInfo GetTessDataDirectory() {
            return new FileInfo(LANG_TESS_DATA_DIRECTORY);
        }
    }
}
