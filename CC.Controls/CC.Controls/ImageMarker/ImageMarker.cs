using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CC.Utilities;
using CC.Utilities.Drawing;

namespace CC.Controls
{
    /// <summary>
    /// Represents an <see cref="Image"/> that can be drawn on.
    /// </summary>
    public partial class ImageMarker : UserControl
    {
        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ImageMarker"/>
        /// </summary>
        public ImageMarker()
        {
            Lines = new List<ImageMarkerLine>();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            
            InitializeComponent();
        }
        #endregion

        #region Public Events
        #endregion

        #region Private Fields
        private DoubleBufferedGraphics _DoubleBufferedGraphics = new DoubleBufferedGraphics();
        private Image _Image;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the <see cref="Image"/>
        /// </summary>
        public Image Image
        { 
            get { return _Image; }
            set
            {
                _Image = value;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ImageMarkerLine"/>s to draw.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ImageMarkerLine> Lines { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> that the <see cref="Image"/> will be captured from.
        /// </summary>
        public Control Source { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Show"/> method has been called.
        /// </summary>
        public bool Shown { get; protected set; }
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        private Rectangle GetImageMarkerLineRectangle(ImageMarkerLine imageMarkerLine)
        {
            return imageMarkerLine.GetRectangle(GetSecondaryValue(imageMarkerLine));
        }

        private float GetSecondaryValue(ImageMarkerLine imageMarkerLine)
        {
            return (imageMarkerLine.Orientation == Orientation.Horizontal ? Width : Height);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Paints the <see cref="ImageMarker"/>
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_DoubleBufferedGraphics.Initialized)
            {
                _DoubleBufferedGraphics.Initialize(Width, Height);
            }

            if (_Image != null)
            {
                _DoubleBufferedGraphics.SetBackgroundImage(_Image);
            }
            
            foreach (ImageMarkerLine imageMarkerLine in Lines)
            {
                imageMarkerLine.Paint(_DoubleBufferedGraphics.Graphics, GetSecondaryValue(imageMarkerLine));
            }

            _DoubleBufferedGraphics.Render(e.Graphics);
        }

        /// <summary>
        /// An empty override to prevent the background from being painted.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Fires the <see cref="Control.Resize"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _DoubleBufferedGraphics.Initialize(Width, Height);

            Invalidate();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Disposes the control.
        /// </summary>
        public new void Dispose()
        {
            if (_Image != null)
            {
                _Image.Dispose();
                _Image = null;
            }

            if (_DoubleBufferedGraphics != null)
            {
                _DoubleBufferedGraphics.Dispose();
                _DoubleBufferedGraphics = null;
            }
        }

        /// <summary>
        /// Hides the <see cref="ImageMarker"/>
        /// </summary>
        public new void Hide()
        {
            base.Hide();
            SendToBack();
            Shown = false;
        }

        /// <summary>
        /// Shows the <see cref="ImageMarker"/> and updates the <see cref="Image"/> from <see cref="Source"/> if necessary.
        /// </summary>
        public new void Show()
        {
            if (Source != null)
            {
                if (_Image != null)
                {
                    _Image.Dispose();
                    _Image = null;
                }

                _Image = Source.DrawToImage();
            }

            base.Show();
            BringToFront();
            Invalidate();
            Shown = true;
        }

        /// <summary>
        /// Replaces existing <see cref="Lines"/> with the specified <see cref="ImageMarkerLine"/>.
        /// </summary>
        /// <param name="line">The new <see cref="ImageMarkerLine"/></param>
        public void UpdateLines(ImageMarkerLine line)
        {
            UpdateLines(new List<ImageMarkerLine> {line});
        }

        /// <summary>
        /// Replaces existing <see cref="Lines"/> with the specified <see cref="ImageMarkerLine"/>s.
        /// </summary>
        /// <param name="lines">The new <see cref="ImageMarkerLine"/>s</param>
        public void UpdateLines(List<ImageMarkerLine> lines)
        {
            if (Lines.Count > 0)
            {
                ImageMarkerLine[] tempLines = new ImageMarkerLine[Lines.Count];
                Lines.CopyTo(tempLines);
                Lines.Clear();

                foreach (ImageMarkerLine imageMarkerLine in tempLines)
                {
                    Rectangle rectangle = GetImageMarkerLineRectangle(imageMarkerLine);
                    rectangle.Inflate(1, 1);
                    Invalidate(rectangle);
                }
            }

            if (lines.Count > 0)
            {
                Lines.AddRange(lines);

                foreach (ImageMarkerLine imageMarkerLine in Lines)
                {
                    Rectangle rectangle = GetImageMarkerLineRectangle(imageMarkerLine);
                    rectangle.Inflate(1, 1);
                    Invalidate(rectangle);
                }
            }
        }
        #endregion
    }
}
