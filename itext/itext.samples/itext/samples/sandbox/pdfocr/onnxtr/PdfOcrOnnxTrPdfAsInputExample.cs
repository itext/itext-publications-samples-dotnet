using System;
using System.IO;
using iText.Pdfocr;
using iText.Pdfocr.Onnxtr;
using iText.Pdfocr.Onnxtr.Detection;
using iText.Pdfocr.Onnxtr.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnxtr {
    /// <summary>PdfOcrOnnxTrPdfAsInputExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxTrPdfAsInputExample.java
    /// <para />
    /// This example demonstrates how to perform OCR of all images in an input PDF file
    /// and generate searchable PDF using provided
    /// <see cref="OnnxTrOcrEngine"/>.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-OnnxTR 4.1.0.
    /// </remarks>
    public class PdfOcrOnnxTrPdfAsInputExample {
        public const String DEST = "results/sandbox/pdfocr/onnxtr/PdfOcrOnnxTrPdfAsInputExample/result.pdf";

        private const String PDF = "../../../resources/pdfs/numbers.pdf";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxTrPdfAsInputExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);
            using (OnnxTrOcrEngine ocrEngine = new OnnxTrOcrEngine(detectionPredictor, recognitionPredictor)) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
                pdfCreator.MakePdfSearchable(new FileInfo(PDF), new FileInfo(DEST));
            }
        }
    }
}
