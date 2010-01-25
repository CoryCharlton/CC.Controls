using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CC.Controls
{
    /// <summary>
    /// Represents a line on an <see cref="ImageMarker"/>
    /// </summary>
    public class ImageMarkerLine
    {
        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ImageMarkerLine"/>
        /// </summary>
        public ImageMarkerLine()
        {
            Color = Control.DefaultForeColor;
            DashStyle = DashStyle.Solid;
            Orientation = Orientation.Horizontal;
            Padding = new Padding(0);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the <see cref="Color"/>.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DashStyle"/>
        /// </summary>
        public DashStyle DashStyle { get; set; }

        /// <summary>
        /// Gets the invalidation rectangle based on the last call to <see cref="Paint"/>
        /// </summary>
        public Rectangle InvalidationRectangle { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="Orientation"/>.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Padding"/>
        /// </summary>
        public Padding Padding { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float Value { get; set; }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets a <see cref="Rectangle"/> based on the <see cref="ImageMarkerLine"/>
        /// </summary>
        /// <param name="secondaryValue"></param>
        /// <returns></returns>
        public Rectangle GetRectangle(float secondaryValue)
        {
            return Orientation == Orientation.Horizontal ? 
                new Rectangle(Padding.Left, (int) Math.Round(Value), (int) Math.Round(secondaryValue - Padding.Left - Padding.Right), 1) : 
                new Rectangle((int) Math.Round(Value), Padding.Top, 1, (int) Math.Round(secondaryValue - Padding.Bottom - Padding.Top));
        }

        /// <summary>
        /// Paints the <see cref="ImageMarkerLine"/> to the specified <see cref="Graphics"/> object
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object to paint to</param>
        /// <param name="secondaryValue">If <see cref="Orientation"/> equals Orientation.Horizontal then this is the x2 value for the line. Otherwise it is the y2 value. <see cref="Graphics.DrawLine(System.Drawing.Pen,int,int,int,int)"/></param>
        public void Paint(Graphics g, float secondaryValue)
        {
            using (Pen pen = new Pen(Color, 1) {DashStyle = DashStyle})
            {
                if (Orientation == Orientation.Horizontal)
                {
                    g.DrawLine(pen, Padding.Left, Value, secondaryValue - Padding.Right, Value);
                }
                else
                {
                    g.DrawLine(pen, Value, Padding.Top, Value, secondaryValue - Padding.Bottom);
                }

            }

            InvalidationRectangle = GetRectangle(secondaryValue);
        }
        #endregion
    }
}
