using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Pdfocr;
using iText.Pdfocr.Tesseract4;

namespace iText.Samples.Sandbox.Pdfocr.Tesseract4 {
    /// <summary>PdfOcrTesseractExample.java</summary>
    /// <remarks>
    /// PdfOcrTesseractExample.java
    /// <para />
    /// This example demonstrates how to perform OCR using provided
    /// <see cref="Tesseract4ExecutableOcrEngine"/>
    /// or
    /// <see cref="Tesseract4LibOcrEngine"/>
    /// for the given list of input images and save output to a PDF file using provided path.
    /// <para />
    /// Software: iText 9.0.0, pdfOCR-Tesseract4 4.0.0. Requires
    /// <c>tess4j</c>
    /// dependency or Tesseract installation,
    /// see <a href="https://tesseract-ocr.github.io/tessdoc/home.html">Tesseract User Manual</a>,
    /// Leptonica library is required on Linux operating systems.
    /// </remarks>
    public class PdfOcrTesseractExample {
        public const String DEST = "results/sandbox/pdfocr/tesseract4/PdfOcrTesseractExample/result.pdf";

        // Directory with trained data for tests
        protected internal const String LANG_TESS_DATA_DIRECTORY = "../../../resources/tessdata";

        private const String LIB_DEST = "results/sandbox/pdfocr/tesseract4/PdfOcrTesseractExample/libResult.pdf";

        private const String EXE_DEST = "results/sandbox/pdfocr/tesseract4/PdfOcrTesseractExample/exeResult.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String ROTATED_IMAGE = "../../../resources/img/rotated.png";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrTesseractExample().Manipulate();
            // Merge both PDFs to compare the result.
            CreateFinalPDF();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> {
                new FileInfo(BASIC_IMAGE), new FileInfo(ROTATED_IMAGE)
            };
            // Create PDF with tess4j library.
            Tesseract4LibOcrEngine libOcrEngine = new Tesseract4LibOcrEngine(new Tesseract4OcrEngineProperties().SetPathToTessData
                (GetTessDataDirectory()));
            OcrPdfCreator pdfCreator = new OcrPdfCreator(libOcrEngine);
            using (PdfWriter writer = new PdfWriter(LIB_DEST)) {
                pdfCreator.CreatePdf(images, writer).Close();
            }
            // Create PDF with Tesseract executable.
            Tesseract4ExecutableOcrEngine exeOcrEngine = new Tesseract4ExecutableOcrEngine(GetTesseractExecutableCommand
                (), new Tesseract4OcrEngineProperties().SetPathToTessData(GetTessDataDirectory()));
            pdfCreator = new OcrPdfCreator(exeOcrEngine);
            using (PdfWriter writer_1 = new PdfWriter(EXE_DEST)) {
                pdfCreator.CreatePdf(images, writer_1).Close();
            }
        }

        protected internal static String GetTesseractExecutableCommand() {
            String tesseractDir = Environment.GetEnvironmentVariable("tesseractDir");
            String os = Environment.GetEnvironmentVariable("os.name") == null ? Environment.GetEnvironmentVariable("OS"
                ) : Environment.GetEnvironmentVariable("os.name");
            return os.ToLowerInvariant().Contains("win") && tesseractDir != null && !String.IsNullOrEmpty(tesseractDir
                ) ? tesseractDir + "\\tesseract.exe" : "tesseract";
        }

        protected internal static FileInfo GetTessDataDirectory() {
            return new FileInfo(LANG_TESS_DATA_DIRECTORY);
        }

        private static void CreateFinalPDF() {
            using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(DEST))) {
                using (PdfDocument lib = new PdfDocument(new PdfReader(LIB_DEST))) {
                    using (PdfDocument exe = new PdfDocument(new PdfReader(EXE_DEST))) {
                        PdfMerger merger = new PdfMerger(pdfDoc);
                        merger.Merge(lib, 1, lib.GetNumberOfPages());
                        merger.Merge(exe, 1, exe.GetNumberOfPages());
                    }
                }
            }
        }
    }
}
