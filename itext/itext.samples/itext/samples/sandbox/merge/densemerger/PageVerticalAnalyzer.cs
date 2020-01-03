/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace iText.Samples.Sandbox.Merge.Densemerger
{
    /// <summary>
    /// This class calculates the border y coordinates between used (drawn onto)
    /// and unused vertical sections of the page if the supported render event is occurred.
    /// </summary>
    public class PageVerticalAnalyzer : IEventListener
    {
        private readonly ICollection<EventType> supportedEvents;
        private readonly List<float> verticalFlips = new List<float>();

        /// <summary>
        /// Constructs a PageVerticalAnalyzer.
        /// </summary>
        public PageVerticalAnalyzer()
        {
            supportedEvents = new HashSet<EventType>();
            supportedEvents.Add(EventType.RENDER_TEXT);
            supportedEvents.Add(EventType.RENDER_PATH);
            supportedEvents.Add(EventType.RENDER_IMAGE);
        }

        /// <summary>
        /// Gets the <c>verticalFlips</c>.
        /// </summary>
        /// <returns>
        /// a <see cref="System.Collections.Generic.List" /> of y coordinates between used (drawn onto)
        /// and unused vertical sections of the page, which represents coordinates of occupied space.
        /// Each odd coordinate i represents a starting y coordinate of a used vertical sections,
        /// each coordinate (i+1) - a finishing y coordinate of a used vertical sections.
        /// </returns>
        public List<float> GetVerticalFlips()
        {
            return verticalFlips;
        }

        //
        // EventListener implementation
        //

        /// <summary><inheritDoc/></summary>
        public void EventOccurred(IEventData data, EventType type)
        {
            switch (type)
            {
                case EventType.RENDER_IMAGE:
                {
                    ImageRenderInfo renderInfo = (ImageRenderInfo) data;
                    Matrix ctm = renderInfo.GetImageCtm();
                    float[] yCoords = new float[4];
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            Vector corner = new Vector(x, y, 1).Cross(ctm);
                            yCoords[2 * x + y] = corner.Get(Vector.I2);
                        }
                    }

                    Array.Sort(yCoords);
                    AddVerticalUseSection(yCoords[0], yCoords[3]);
                    break;
                }
                case EventType.RENDER_PATH:
                {
                    PathRenderInfo renderInfo = (PathRenderInfo) data;
                    if (renderInfo.GetOperation() != PathRenderInfo.NO_OP)
                    {
                        Matrix ctm = renderInfo.GetCtm();
                        Path path = renderInfo.GetPath();
                        foreach (Subpath subpath in path.GetSubpaths())
                        {
                            List<float> yCoordsList = new List<float>();
                            foreach (Point point2d in subpath.GetPiecewiseLinearApproximation())
                            {
                                Vector vector = new Vector((float) point2d.GetX(), (float) point2d.GetY(), 1);
                                vector = vector.Cross(ctm);
                                yCoordsList.Add(vector.Get(Vector.I2));
                            }

                            if (yCoordsList.Count != 0)
                            {
                                float[] yCoords = yCoordsList.ToArray();
                                Array.Sort(yCoords);
                                AddVerticalUseSection(yCoords[0], yCoords[yCoords.Length - 1]);
                            }
                        }
                    }

                    break;
                }
                case EventType.RENDER_TEXT:
                {
                    TextRenderInfo renderInfo = (TextRenderInfo) data;
                    LineSegment ascentLine = renderInfo.GetAscentLine();
                    LineSegment descentLine = renderInfo.GetDescentLine();
                    float[] yCoords = new float[]
                    {
                        ascentLine.GetStartPoint().Get(Vector.I2),
                        ascentLine.GetEndPoint().Get(Vector.I2),
                        descentLine.GetStartPoint().Get(Vector.I2),
                        descentLine.GetEndPoint().Get(Vector.I2)
                    };


                    Array.Sort(yCoords);
                    AddVerticalUseSection(yCoords[0], yCoords[3]);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary><inheritDoc/></summary>
        public ICollection<EventType> GetSupportedEvents()
        {
            return supportedEvents;
        }

        //
        // Helper methods
        //

        /// <summary>
        /// Marks the given interval as used.
        /// </summary>
        private void AddVerticalUseSection(float from, float to)
        {
            if (to < from)
            {
                float temp = to;
                to = from;
                from = temp;
            }

            int i = 0, j = 0;
            for (; i < verticalFlips.Count; i++)
            {
                float flip = verticalFlips[i];
                if (flip < from)
                {
                    continue;
                }

                for (j = i; j < verticalFlips.Count; j++)
                {
                    flip = verticalFlips[j];
                    if (flip < to)
                    {
                        continue;
                    }

                    break;
                }

                break;
            }

            bool fromOutsideInterval = i % 2 == 0;
            bool toOutsideInterval = j % 2 == 0;

            while (j-- > i)
            {
                verticalFlips.RemoveAt(j);
            }

            if (toOutsideInterval)
            {
                verticalFlips.Insert(i, to);
            }

            if (fromOutsideInterval)
            {
                verticalFlips.Insert(i, from);
            }
        }
    }
}