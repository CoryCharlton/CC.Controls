using System;
using System.Drawing;
using CC.Utilities;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public abstract class MarginBarMarkerBase
    {
        #region Constructor
        protected MarginBarMarkerBase(MarkerAlign align, float dpi, float inches, bool fromRight, int width, int height, int top)
        {
            Align = align;
            Dpi = dpi;
            Height = height;
            Inches = inches;
            FromRight = fromRight;
            Top = top;
            Width = width;
        }
        #endregion

        #region Public Properties
        public MarkerAlign Align { get; set; }
        public float Dpi { get; set; }
        public int Height { get; set; }
        public float Inches { get; set; }
        public bool FromRight { get; set; }
        public Rectangle InvalidationRectangle { get; protected set; }
        public bool Pushed { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        #endregion

        #region Protected Methods
        protected float GetPixelsFromRuler(int paddingLeft, float rulerLength)
        {
            return (ConvertEx.InchesToPixels(FromRight ? (rulerLength - Inches) : Inches, Dpi) + paddingLeft);
        }
        #endregion

        #region Public Methods
        public Rectangle GetRectangle(int paddingLeft, float rulerLength)
        {
            int horizontalCenter = (int) Math.Round(GetPixelsFromRuler(paddingLeft, rulerLength));
            int left = horizontalCenter - (Width / 2);

            return new Rectangle(left, Top, Width, Height);
        }

        public float GetInverseInches(float rulerLength)
        {
            return (rulerLength - Inches);
        }

        public abstract void Paint(Graphics g, int paddingLeft, float rulerLength);
        #endregion
    }
}
