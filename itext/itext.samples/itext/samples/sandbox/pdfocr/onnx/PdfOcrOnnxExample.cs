using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Onnx;
using iText.Pdfocr.Onnx.Detection;
using iText.Pdfocr.Onnx.Orientation;
using iText.Pdfocr.Onnx.Recognition;

namespace iText.Samples.Sandbox.Pdfocr.Onnx {
    /// <summary>PdfOcrOnnxExample.java</summary>
    /// <remarks>
    /// PdfOcrOnnxExample.java
    /// <para />
    /// This example demonstrates how to perform OCR using provided
    /// <see cref="iText.Pdfocr.Onnx.OnnxTrOcrEngine"/>
    /// for the given list of input images and save output to a PDF file using provided path.
    /// <para />
    /// Required software: iText 9.3.0, pdfOCR-Onnx 5.0.0.
    /// </remarks>
    public class PdfOcrOnnxExample {
        public const String DEST = "results/sandbox/pdfocr/onnx/PdfOcrOnnxExample/result.pdf";

        private const String BASIC_IMAGE = "../../../resources/img/ocrExample.png";

        private const String ROTATED_IMAGE = "../../../resources/img/rotated.png";

        private const String MODELS = "../../../resources/models/";

        private const String FAST = MODELS + "rep_fast_tiny-28867779.onnx";

        private const String CRNNVGG16 = MODELS + "crnn_vgg16_bn-662979cc.onnx";

        private const String MOBILENETV3 = MODELS + "mobilenet_v3_small_crop_orientation-5620cf7e.onnx";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfOcrOnnxExample().Manipulate();
        }

        protected internal virtual void Manipulate() {
            IList<FileInfo> images = new List<FileInfo> {
                new FileInfo(BASIC_IMAGE), new FileInfo(ROTATED_IMAGE)
            };
            IDetectionPredictor detectionPredictor = OnnxDetectionPredictor.Fast(FAST);
            IOrientationPredictor orientationPredictor = OnnxOrientationPredictor.MobileNetV3(MOBILENETV3);
            IRecognitionPredictor recognitionPredictor = OnnxRecognitionPredictor.CrnnVgg16(CRNNVGG16);
            // OnnxTrOcrEngine shall be closed after usage to avoid native allocations leak.
            // It will also close all predictors used for its creation.
            using (OnnxTrOcrEngine ocrEngine = new OnnxTrOcrEngine(detectionPredictor, orientationPredictor, recognitionPredictor
                )) {
                OcrPdfCreator pdfCreator = new OcrPdfCreator(ocrEngine);
                pdfCreator.CreatePdf(images, new PdfWriter(DEST)).Close();
            }
        }
    }
}
