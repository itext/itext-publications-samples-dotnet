using System;
using System.Collections.Generic;
using System.IO;
using iText.Pdfocr.Onnxtr;
using iText.Pdfocr.Onnxtr.Detection;
using iText.Pdfocr.Onnxtr.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnxtr {
    /// <summary>PdfOcrOnnxTrTxtFileExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxTrTxtFileExample.java
    /// <para />
    /// This example demonstrates how to perform OCR using provided
    /// <see cref="OnnxTrOcrEngine"/>
    /// for the given list of input images and save output to a text file using provided path.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-OnnxTR 4.1.0.
    /// </remarks>
    public class PdfOcrOnnxTrTxtFileExample {
        public const String DEST = "results/sandbox/pdfocr/onnxtr/PdfOcrOnnxTrTxtFileExample/ocr_result.txt";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxTrTxtFileExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> { new FileInfo(BASIC_IMAGE) };
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);
            // OnnxTrOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxTrOcrEngine ocrEngine = new OnnxTrOcrEngine(detectionPredictor, recognitionPredictor)) {
                ocrEngine.CreateTxtFile(images, new FileInfo(DEST));
            }
        }
    }
}
