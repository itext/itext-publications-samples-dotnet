using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.columncontainer {

    // ColumnAllowMoreRelayouts.cs
    // 
    // This class demonstrates how to customize column handling by increasing relayout attempts.
    // It creates a PDF with a three-column container that uses a custom renderer to allow
    // more layout calculations, improving content distribution when working with complex text.
 
    public class ColumnAllowMoreRelayouts {

    public static readonly String DEST = "results/sandbox/columncontainer/allow_more_re_layouts.pdf";

    public static void Main(String[] args)  {
            var file = new FileInfo(DEST);
            file.Directory.Create();

        new ColumnAllowMoreRelayouts().ManipulatePdf(DEST);
    }

    public void ManipulatePdf(String dest) {
        PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
        Document doc = new Document(pdfDoc);

        // Add a flowing paragraph
        MulticolContainer container = new MulticolContainer();
        container.SetProperty(Property.COLUMN_COUNT, 3);
        container.SetNextRenderer(new MultiColRendererAllow10RetriesRenderer(container));
        Paragraph div = new Paragraph(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas malesuada vitae mi sed fermentum. "
                        + "Vestibulum magna orci, iaculis vitae rutrum vitae, bibendum eget urna. Aenean quis metus "
                        + "diam. Vivamus et lobortis nunc. Cras aliquet placerat diam ac pellentesque. Quisque "
                        + "pulvinar velit auctor, luctus mi et, interdum quam. Curabitur ac tristique libero. "
                        + "Vestibulum dictum ipsum sit amet iaculis efficitur. Vestibulum convallis, odio a varius "
                        + "efficitur, sapien nulla convallis arcu, vel interdum est odio nec nibh. Duis euismod a "
                        + "quam et blandit."
                        +
                        "Nulla dapibus pretium lectus, a dictum diam scelerisque ut. Mauris sit amet ipsum urna. Duis"
                        + " lacinia mi sem, non consectetur mi fermentum in. Aenean volutpat convallis eros tristique"
                        + " dignissim. Etiam tincidunt tortor nec arcu ultrices, vel suscipit sapien posuere. Cras "
                        + "aliquet velit in lacus tempor maximus. Fusce suscipit ex ut nisi aliquam scelerisque. Duis"
                        + " feugiat ultrices elementum. Morbi eget metus ut ligula ullamcorper interdum sed finibus "
                        + "justo."
                        +
                        "Nunc dictum ac eros ac porttitor. In viverra ex tortor. Sed ipsum sem, finibus at gravida "
                        + "sit amet, ultrices et odio. Maecenas posuere imperdiet lectus in aliquet. Nunc at nunc "
                        + "eget dui elementum posuere. In vitae venenatis neque. Vestibulum semper eu est in faucibus."
                        +
                        "Aliquam tellus arcu, suscipit a lacus ut, cursus aliquam lacus. Suspendisse mi urna, cursus "
                        + "eu gravida et, accumsan vitae nisl. Duis eget magna posuere, sodales enim id, ornare arcu."

        );
        container.Add(div);

        doc.Add(container);
        doc.Close();
    }

    public  class MultiColRendererAllow10RetriesRenderer : MulticolRenderer {

        /**
         * Creates a DivRenderer from its corresponding layout object.
         *
         * @param modelElement the {@link MulticolContainer} which this object should manage
         */
        public MultiColRendererAllow10RetriesRenderer(MulticolContainer modelElement) : base(modelElement) {
            SetHeightCalculator(new LayoutInInfiniteHeightCalculator());
        }

        /**
         * {@inheritDoc}
         */
        public IRenderer getNextRenderer() {
            return new MultiColRendererAllow10RetriesRenderer((MulticolContainer) modelElement);
        }
    }

    public  class LayoutInInfiniteHeightCalculator : MulticolRenderer.LayoutInInfiniteHeightCalculator {

        public LayoutInInfiniteHeightCalculator() {
            maxRelayoutCount = 10;
        }
    }
}
    
}