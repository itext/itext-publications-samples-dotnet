using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxPaddleOcrExample.cs</summary>
    /// <remarks>
    /// PdfOcrOnnxPaddleOcrExample.cs
    /// <para />
    /// This example demonstrates how to perform OCR using
    /// <see cref="iText.Pdfocr.Onnx.OnnxOcrEngine"/>
    /// and PaddleOCR ML-models
    /// for the given list of input images and save output to a PDF file using provided path.
    /// <para />
    /// PaddleOCR models converted to ONNX format can be found at
    /// <a href="https://huggingface.co/itextresearch">iText Research Hugging Face page</a>.
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class PdfOcrOnnxPaddleOcrExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxPaddleOcrExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/paddleocr/";

        private const String DETECTION = MODELS + "PP-OCRv5_mobile_det_infer";

        private const String RECOGNITION = MODELS + "PP-OCRv5_mobile_rec_infer";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfOcrOnnxPaddleOcrExample().Manipulate(DEST);
        }

        protected internal virtual void Manipulate(String destination) {
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.PaddleOcr(
                DETECTION + "/inference.onnx", DETECTION + "/inference.yml");
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.PaddleOcr(
                RECOGNITION + "/inference.onnx", RECOGNITION + "/inference.yml");

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, recognitionPredictor)) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
                pdfCreator.CreatePdf(new List<FileInfo> { new FileInfo(BASIC_IMAGE) }, new PdfWriter(destination))
                    .Close();
            }
        }
    }
}
