using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxEasyOcrExample.cs</summary>
    /// <remarks>
    /// PdfOcrOnnxEasyOcrExample.cs
    /// <para />
    /// This example demonstrates how to perform OCR using
    /// <see cref="iText.Pdfocr.Onnx.OnnxOcrEngine"/>
    /// and EasyOCR ML-models
    /// for the given list of input images and save output to a PDF file using provided path.
    /// <para />
    /// EasyOCR models converted to ONNX format can be found at
    /// <a href="https://huggingface.co/itextresearch">iText Research Hugging Face page</a>.
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class PdfOcrOnnxEasyOcrExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxEasyOcrExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/easyocr/";

        private const String DETECTION = MODELS + "craft_mlt_25k.onnx";

        private const String RECOGNITION = MODELS + "latin_g2.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfOcrOnnxEasyOcrExample().Manipulate(DEST);
        }

        protected internal virtual void Manipulate(String destination) {
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.EasyOcr(DETECTION);
            IRecognitionPredictor recognitionPredictor = 
                OnnxRecognitionPredictor.EasyOcr(RECOGNITION, EasyOcrMapper.LATIN_G2);

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, recognitionPredictor)) {
                using (Stream output = FileUtil.GetFileOutputStream(destination)) {
                    OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
                    pdfCreator.CreatePdf(new List<FileInfo> { new FileInfo(BASIC_IMAGE) }, new PdfWriter(output))
                        .Close();
                }
            }
        }
    }
}
