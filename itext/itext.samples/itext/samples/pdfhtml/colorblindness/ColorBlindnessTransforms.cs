/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

namespace iText.Samples.Pdfhtml.Colorblindness
{
    public class ColorBlindnessTransforms
    {
        public static readonly string PROTANOPIA = "Protanopia";
        private static float[][] PROTANOPIA_TRANSFORM =
            {
                new float[] {0.5667f, 0.43333f, 0}, 
                new float[] {0.55833f, 0.44167f, 0}, 
                new float[] {0, 0.24167f, 0.75833f}
            };

        public static readonly string PROTANOMALY = "Protanomaly";
        private static float[][] PROTANOMALY_TRANSFORM =
            {
                new float[] {0.81667f, 0.18333f, 0}, 
                new float[] {0.33333f, 0.66667f, 0}, 
                new float[] {0, 0.125f, 0.875f}
            };

        public static readonly string DEUTERANOPIA = "Deuteranopia";
        private static float[][] DEUTERANOPIA_TRANSFORM =
        {
            new float[] {0.625f, 0.375f, 0}, 
            new float[] {0.70f, 0.30f, 0}, 
            new float[] {0, 0.30f, 0.70f}
        };

        public static readonly string DEUTERANOMALY = "Deuteranomaly";
        private static float[][] DEUTERANOMALY_TRANSFORM =
        {
            new float[] {0.80f, 0.20f, 0}, 
            new float[] {0.25833f, 0.74167f, 0}, 
            new float[] {0, 0.14167f, 0.85833f}
        };

        public static readonly string TRITANOPIA = "Tritanopia";
        private static float[][] TRITANOPIA_TRANSFORM =
        {
            new float[] {0.95f, 0.05f, 0}, 
            new float[] {0, 0.43333f, 0.56667f}, 
            new float[] {0, 0.475f, 0.525f}
        };

        public static readonly string TRITANOMALY = "Tritanomaly";
        private static float[][] TRITANOMALY_TRANSFORM =
        {
            new float[] {0.96667f, 0.0333f, 0}, 
            new float[] {0, 0.73333f, 0.26667f}, 
            new float[] {0, 0.18333f, 0.81667f}
        };

        public static readonly string ACHROMATOPSIA = "Achromatopsia";
        private static float[][] ACHROMATOPSIA_TRANSFORM =
        {
            new float[] {0.299f, 0.587f, 0.114f}, 
            new float[] {0.299f, 0.587f, 0.114f}, 
            new float[] {0.299f, 0.587f, 0.114f}
        };

        public static readonly string ACHROMATOMALY = "Achromatomaly";
        private static float[][] ACHROMATOMALY_TRANSFORM =
        {
            new float[] {0.618f, 0.32f, 0.062f}, 
            new float[] {0.163f, 0.775f, 0.062f}, 
            new float[] {0.299f, 0.587f, 0.114f}
        };

        public static float[] SimulateColorBlindness(string code, float[] originalRgb)
        {
            if (code == PROTANOPIA)
            {
                return Simulate(originalRgb, PROTANOPIA_TRANSFORM);
            }
            if (code == PROTANOMALY)
            {
                return Simulate(originalRgb, PROTANOMALY_TRANSFORM);
            }
            if (code == DEUTERANOPIA)
            {
                return Simulate(originalRgb, DEUTERANOPIA_TRANSFORM);
            }
            if (code == DEUTERANOMALY)
            {
                return Simulate(originalRgb, DEUTERANOMALY_TRANSFORM);
            }
            if (code == TRITANOPIA)
            {
                return Simulate(originalRgb, TRITANOPIA_TRANSFORM);
            }
            if (code == TRITANOMALY)
            {
                return Simulate(originalRgb, TRITANOMALY_TRANSFORM);
            }
            if (code == ACHROMATOPSIA)
            {
                return Simulate(originalRgb, ACHROMATOPSIA_TRANSFORM);
            }
            if (code == ACHROMATOMALY)
            {
                return Simulate(originalRgb, ACHROMATOMALY_TRANSFORM);
            }
            return originalRgb;
        }

        private static float[] Simulate(float[] originalRgb, float[][] transformValues)
        {
            // Number of RGB colors
            int nrOfChannels = 3;
            float[] result = new float[nrOfChannels];

            for (int i = 0; i < nrOfChannels; i++)
            {
                result[i] = 0;
                for (int j = 0; j < nrOfChannels; j++)
                {
                    result[i] += originalRgb[j] * transformValues[i][j];
                }
            }

            return result;
        }
    }
}