using System;
using System.Drawing;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public class MarginBarButton : MarginBarMarkerBase
    {
        #region Constructor
        public MarginBarButton(MarkerAlign align, float dpi, float inches, bool fromRight, int width, int height, int top) : base(align, dpi, inches, fromRight, width, height, top) { }
        #endregion

        #region Public Methods
        public override void Paint(Graphics g, int paddingLeft, float rulerLength)
        {
            int horizontalCenter = (int)Math.Round(GetPixelsFromRuler(paddingLeft, rulerLength));
            Rectangle markerRectangle = GetRectangle(paddingLeft, rulerLength);
            Point[] points1; // Full dark polygon
            Point[] points2; // Smaller light polygon

            points1 = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Top),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top),
                                 new Point(markerRectangle.Right, markerRectangle.Top + 1),

                                 new Point(markerRectangle.Right, markerRectangle.Bottom),
                                 //new Point(markerRectangle.Right, markerRectangle.Bottom - 1),
                                 //new Point(markerRectangle.Right - 1, markerRectangle.Bottom),
                                 //new Point(markerRectangle.Left + 1, markerRectangle.Bottom),
                                 new Point(markerRectangle.Left, markerRectangle.Bottom),
                                 //new Point(markerRectangle.Left, markerRectangle.Bottom - 1),

                                 new Point(markerRectangle.Left, markerRectangle.Top + 1)
                             };

            points2 = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Top),
                                 new Point(markerRectangle.Right - 2, markerRectangle.Top),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top + 1),

                                 new Point(markerRectangle.Right - 1, markerRectangle.Bottom - 1),
                                 //new Point(markerRectangle.Right - 1, markerRectangle.Bottom - 2),
                                 //new Point(markerRectangle.Right - 2, markerRectangle.Bottom - 1),
                                 //new Point(markerRectangle.Left + 1, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Left, markerRectangle.Bottom - 1),
                                 //new Point(markerRectangle.Left, markerRectangle.Bottom - 2),
                                 
                                 new Point(markerRectangle.Left, markerRectangle.Top + 1)
                             };

            using (SolidBrush outsideBrush = new SolidBrush(Pushed ? SystemColors.ControlDark : SystemColors.ControlDark)) // I know right ;-) Leaving here incase I want different colors in the future
            {
                g.FillPolygon(outsideBrush, points1);
            }

            using (SolidBrush insideBrush = new SolidBrush(Pushed ? SystemColors.Control : SystemColors.ControlLight))
            {
                g.FillPolygon(insideBrush, points2);
            }

            g.DrawPolygon(Pens.Black, points1);

            markerRectangle.Inflate(2, 2);
            InvalidationRectangle = markerRectangle;
        }
        #endregion
    }
}
