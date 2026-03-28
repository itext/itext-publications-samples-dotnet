using System;
using System.Collections.Generic;
using System.IO;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxTxtFileExample.cs</summary>
    /// <remarks>
    /// PdfOcrOnnxTxtFileExample.cs
    /// <para />
    /// This example demonstrates how to perform OCR using provided
    /// <see cref="iText.Pdfocr.Onnx.OnnxOcrEngine"/>
    /// for the given list of input images and save output to a text file using provided path.
    /// <para />
    /// Required software: iText 9.6.0, pdfOCR-Onnx 5.0.0
    /// (itext.pdfocr.onnx.cpu dependency to execute ONNX models on CPU or
    /// itext.pdfocr.onnx.abstract and Microsoft.ML.OnnxRuntime.Gpu dependencies to execute ONNX models on GPU).
    /// </remarks>
    public class PdfOcrOnnxTxtFileExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxTxtFileExample/ocr_result.txt";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfOcrOnnxTxtFileExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { new FileInfo(BASIC_IMAGE) };

            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);

            // OnnxOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxOcrEngine ocrEngine = new OnnxOcrEngine(detectionPredictor, recognitionPredictor)) {
                ocrEngine.CreateTxtFile(images, new FileInfo(DEST));
            }
        }
    }
}
