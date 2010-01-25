using System;
using System.Drawing;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public class MarginBarMarker : MarginBarMarkerBase
    {
        #region Constructor
        public MarginBarMarker(MarkerAlign align, float dpi, float inches, bool fromRight, int width, int height, int top): base(align, dpi, inches, fromRight, width, height, top) {}
        #endregion

        #region Public Methods
        public override void Paint(Graphics g, int paddingLeft, float rulerLength)
        {
            int horizontalCenter = (int)Math.Round(GetPixelsFromRuler(paddingLeft, rulerLength));
            Rectangle markerRectangle = GetRectangle(paddingLeft, rulerLength);
            Point[] pointsMarker; // Full polygon
            Point[] pointsLightLine; // Light 3D Border Lines
            Point[] pointsDarkLine; // Smaller center polygon

            if (Align == MarkerAlign.Top)
            {
                pointsMarker = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Top),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top),
                                 new Point(markerRectangle.Right, markerRectangle.Top + 1),
                                 new Point(markerRectangle.Right, markerRectangle.Top + 3),
                                 new Point(horizontalCenter, markerRectangle.Bottom),
                                 new Point(markerRectangle.Left, markerRectangle.Top + 3),
                                 new Point(markerRectangle.Left, markerRectangle.Top + 1)
                             };

                pointsLightLine = new[]
                             {
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top + 1),
                                 new Point(markerRectangle.Left + 2, markerRectangle.Top + 1),
                                 new Point(markerRectangle.Left + 1, markerRectangle.Top + 1),
                                 new Point(markerRectangle.Left + 1, markerRectangle.Top + 3),
                                 new Point(horizontalCenter, markerRectangle.Bottom - 1),
                             };

                pointsDarkLine = new[]
                             {
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top + 1),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Top + 3),
                                 new Point(horizontalCenter, markerRectangle.Bottom - 1),
                             };
            }
            else
            {
                pointsMarker = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Bottom),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Bottom),
                                 new Point(markerRectangle.Right, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Right, markerRectangle.Bottom - 3),
                                 new Point(horizontalCenter, markerRectangle.Top),
                                 new Point(markerRectangle.Left, markerRectangle.Bottom - 3),
                                 new Point(markerRectangle.Left, markerRectangle.Bottom - 1)
                             };

                pointsDarkLine = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Right - 2, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Right - 1, markerRectangle.Bottom - 3),
                                 new Point(horizontalCenter, markerRectangle.Top + 1),
                             };

                pointsLightLine = new[]
                             {
                                 new Point(markerRectangle.Left + 1, markerRectangle.Bottom - 1),
                                 new Point(markerRectangle.Left + 1, markerRectangle.Bottom - 3),
                                 new Point(horizontalCenter, markerRectangle.Top + 1),
                             };
            }

            using (SolidBrush centerBrush = new SolidBrush(Pushed ? SystemColors.Control : SystemColors.ControlLight))
            {
                g.FillPolygon(centerBrush, pointsMarker);
            }

            using (Pen lightPen = new Pen(Pushed ? SystemColors.ControlDark : SystemColors.ControlLightLight))
            {
                g.DrawLines(lightPen, pointsLightLine);
            }

            using (Pen darkPen = new Pen(Pushed ? SystemColors.ControlLightLight : SystemColors.ControlDark))
            {
                g.DrawLines(darkPen, pointsDarkLine);
            }

            g.DrawPolygon(Pens.Black, pointsMarker);

            markerRectangle.Inflate(2, 2);
            InvalidationRectangle = markerRectangle;
        }
        #endregion
    }
}
