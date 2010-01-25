using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CC.Utilities;
using CC.Utilities.Drawing;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public partial class MarginBar : UserControl
    {
        #region Constructor
        public MarginBar()
        {
            using (Graphics g = CreateGraphics())
            {
                _Dpi = g.DpiX;
            }

            _HangingIndent = new MarginBarMarker(MarkerAlign.Top, _Dpi, 0, false, PointerWidth, PointerHeight, GetMarkerTop(MarkerAlign.Top));
            _LeftMargin = new MarginBarMarker(MarkerAlign.Bottom, _Dpi, 0, false, PointerWidth, PointerHeight, GetMarkerTop(MarkerAlign.Bottom));
#if DEBUG
            _LeftMargins = new MarginBarButton(MarkerAlign.Top, _Dpi, 0, false, ButtonWidth, ButtonHeight, 0);
#endif
            _RightMargin = new MarginBarMarker(MarkerAlign.Bottom, _Dpi, 0, true, PointerWidth, PointerHeight, GetMarkerTop(MarkerAlign.Bottom));

            ImageMarkerDashStyle = DashStyle.Dash;
            ImageMarkerLineColor = DefaultForeColor;
            InitializeComponent();
            InitializePaint();
        }
        #endregion

        #region Private Constants
        private const int ButtonHeight = 4;
        private const int ButtonWidth = PointerWidth;
        private const int PointerHeight = 8;
        private const int PointerWidth = 10;
        private const int RulerSteps = 16;
        #endregion

        #region Private Fields
        private readonly DoubleBufferedGraphics _DoubleBufferedGraphics = new DoubleBufferedGraphics();
        private readonly float _Dpi;
        private readonly MarginBarMarker _HangingIndent;
        private readonly MarginBarMarker _LeftMargin;
#if DEBUG
        private readonly MarginBarButton _LeftMargins;
#endif
        private readonly List<MarginBarMarkerBase> _Markers = new List<MarginBarMarkerBase>();
        private readonly List<MarginBarMarkerBase> _PushedMarkers = new List<MarginBarMarkerBase>();
        private readonly MarginBarMarker _RightMargin;
        private float _RightOffset;
        private Rectangle _RightOffsetRectangle;
        private float _RulerLength = 8.5f;
        private Rectangle _RulerRectangle;
        private bool _ShowHangingIndent = true;
        private bool _ShowLeftMargin = true;
        private bool _ShowRightMargin = true;
        #endregion

        #region Public Events
        public event EventHandler HangingIndentChanged;
        public event EventHandler LeftMarginChanged;
        public event EventHandler RightMarginChanged;
        #endregion

        #region Public Properties
        public float HangingIndent
        {
            get { return _HangingIndent.Inches; }
            set { _HangingIndent.Inches = value; InvalidateMarker(_HangingIndent); OnHangingIndentChanged(new EventArgs()); }
        }

        public ImageMarker ImageMarkerControl { get; set; }

        public DashStyle ImageMarkerDashStyle { get; set; }

        public Color ImageMarkerLineColor { get; set; }

        public float LeftMargin
        {
            get { return _LeftMargin.Inches; }
            set { _LeftMargin.Inches = value; InvalidateMarker(_LeftMargin); OnLeftMarginChanged(new EventArgs()); }
        }

        public float RightMargin
        {
            get { return _RightMargin.Inches; }
            set { _RightMargin.Inches = value; InvalidateMarker(_RightMargin); OnRightMarginChanged(new EventArgs()); }
        }

        public float RightOffset
        {
            get { return _RightOffset; }
            set { _RightOffset = value; SetRulerRectangles(); }
        }

        public float RulerLength
        {
            get { return _RulerLength; }
            set { _RulerLength = value; SetRulerRectangles(); }
        }

        public bool ShowHangingIndent
        {
            get { return _ShowHangingIndent; }
            set { _ShowHangingIndent = value; Invalidate(); }
        }

        public bool ShowLeftMargin
        {
            get { return _ShowLeftMargin; }
            set { _ShowLeftMargin = value; Invalidate(); }
        }

        public bool ShowRightMargin
        {
            get { return _ShowRightMargin; }
            set { _ShowRightMargin = value; Invalidate(); }
        }

        public bool SnapToRuler { get; set; }
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        private void DrawRuler(Graphics g)
        {
            const int majorIntervalPaddingBottom = 7;
            const int majorIntervalPaddingTop = 5;
            const int minorIntervalPaddingBottom = 9;
            const int minorIntervalPaddingTop = 7;

            g.FillRectangle(Brushes.White, _RulerRectangle);

            ControlPaint.DrawBorder3D(g, new Rectangle(_RulerRectangle.X - 1, _RulerRectangle.Y, _RulerRectangle.Width + 4, _RulerRectangle.Height));

            if (_RightOffsetRectangle != default(Rectangle))
            {
                using (SolidBrush solidBrush = new SolidBrush(SystemColors.Control))
                {
                    g.FillRectangle(solidBrush, _RightOffsetRectangle);
                }

                ControlPaint.DrawBorder3D(g, _RightOffsetRectangle);
            }

            float step = _Dpi/RulerSteps;
            int steps = (int) (_RulerLength*RulerSteps);

            using (Pen pen = new Pen(ForeColor, 1))
            {
                for (int i = 1; i <= steps; i++)
                {
                    PointF center = new PointF((i*step) + Padding.Left, _RulerRectangle.Top + (_RulerRectangle.Height/2.0f));

                    if (i % 16 == 0)
                    {
                        SizeF size = g.MeasureString(i.ToString(), Font);
                        float left = center.X - (size.Width/2);
                        float top = center.Y - (size.Height/2);

                        if ((left + size.Width) < _RulerRectangle.Right)
                        {
                            RectangleF stringRectangle = new RectangleF(left, top, size.Width, size.Height);

                            StringFormat stringFormat = new StringFormat
                                                            {
                                                                Alignment = StringAlignment.Center,
                                                                LineAlignment = StringAlignment.Center
                                                            };

                            g.DrawLine(pen, center.X, _RulerRectangle.Top + 1, center.X, _RulerRectangle.Top + 2);
                            g.DrawLine(pen, center.X, _RulerRectangle.Bottom - 3, center.X, _RulerRectangle.Bottom - 4);

                            using (SolidBrush foreColorBrush = new SolidBrush(ForeColor))
                            {
                                g.DrawString((i/16).ToString(), Font, foreColorBrush, stringRectangle, stringFormat);
                            }
                        }
                    }
                    else if (center.X < _RulerRectangle.Right)
                    {
                        PointF topPoint;
                        PointF bottomPoint;

                        if (i%8 == 0)
                        {
                            topPoint = new PointF(center.X, _RulerRectangle.Top + majorIntervalPaddingTop);
                            bottomPoint = new PointF(center.X, _RulerRectangle.Bottom - majorIntervalPaddingBottom);
                        }
                        else if (i%4 == 0)
                        {
                            topPoint = new PointF(center.X, _RulerRectangle.Top + minorIntervalPaddingTop);
                            bottomPoint = new PointF(center.X, _RulerRectangle.Bottom - minorIntervalPaddingBottom);
                        }
                        else
                        {
                            topPoint = new PointF(center.X, _RulerRectangle.Top + 9);
                            bottomPoint = new PointF(center.X, _RulerRectangle.Bottom - 11);
                        }

                        g.DrawLine(pen, topPoint, bottomPoint);
                    }
                }
            }
        }

        private float GetActualRulerLength()
        {
            return (_RulerLength - _RightOffset);
        }

        private int GetActualRulerRightPixel()
        {
            return ((_RightOffset <= 0 || _RulerRectangle.Right < _RightOffsetRectangle.Left) ? _RulerRectangle.Right : _RightOffsetRectangle.Left);
        }

        private ImageMarkerLine GetImageMarkerLine(MarginBarMarkerBase marker)
        {
            Rectangle markerRectangle = GetMarkerRectangle(marker);

            ImageMarkerLine imageMarkerLine = new ImageMarkerLine
                                                  {
                                                      Color = ImageMarkerLineColor, 
                                                      DashStyle = ImageMarkerDashStyle, 
                                                      Orientation = Orientation.Vertical, 
                                                      Padding = new Padding(2), 
                                                      Value = markerRectangle.Left + (markerRectangle.Width/2)
                                                  };

            return imageMarkerLine;
        }

        private float GetInchesFromRuler(float pixels, bool fromRight, bool useBounds, bool useActualBounds)
        {
            float inches = fromRight ? 
                ConvertEx.PixelsToInches(GetActualRulerRightPixel() - pixels, _Dpi) : 
                ConvertEx.PixelsToInches(pixels - Padding.Left, _Dpi);

            if (SnapToRuler)
            {
                const float rulerStep = (1f/RulerSteps);
                float remainder = inches%rulerStep;

                if (remainder != 0)
                {
                    const float halfStep = rulerStep/2;
                    inches = (inches - remainder) + (remainder > halfStep ? rulerStep : 0);
                }
            }

            if (useBounds)
            {
                float maxInches = useActualBounds ? GetActualRulerLength() : _RulerLength;

                if (inches < 0)
                {
                    inches = 0;
                }
                else if (inches > maxInches)
                {
                    inches = maxInches;
                }
            }

            return inches;
        }

        private Rectangle GetMarkerRectangle(MarginBarMarkerBase marker)
        {
            return marker.GetRectangle(Padding.Left, GetActualRulerLength());
        }

        private int GetMarkerTop(MarkerAlign align)
        {
            return (align == MarkerAlign.Top) ? (_RulerRectangle.Top - 3) : (_RulerRectangle.Bottom - (PointerHeight - 3) - 2);
        }

        private void InitializePaint()
        {
            _DoubleBufferedGraphics.Initialize(Width, Height);

            SetRulerRectangles();
            SetMarkerTops();
            InvalidateMarkers();
        }

        private void InvalidateImageMarkerControl(MarginBarMarkerBase marker)
        {
            if (ImageMarkerControl != null)
            {
                if (!ImageMarkerControl.Shown)
                {
                    ImageMarkerControl.Show();
                }

                ImageMarkerControl.UpdateLines(GetImageMarkerLine(marker));
            }
        }

        private void InvalidateMarker(MarginBarMarkerBase marker)
        {
            Invalidate(marker.InvalidationRectangle);

            Rectangle markerRectangle = GetMarkerRectangle(marker);
            markerRectangle.Inflate(1, 1);

            Invalidate(markerRectangle);
        }

        private void InvalidateMarkers()
        {
            InvalidateMarker(_HangingIndent);
            InvalidateMarker(_LeftMargin);
            InvalidateMarker(_RightMargin);
        }

        private void SetMarkerTops()
        {
            _HangingIndent.Top = GetMarkerTop(_HangingIndent.Align);
            _LeftMargin.Top = GetMarkerTop(_LeftMargin.Align);
#if DEBUG
            _LeftMargins.Top = _LeftMargin.GetRectangle(Padding.Left, GetActualRulerLength()).Bottom -1;
#endif
            _RightMargin.Top = GetMarkerTop(_RightMargin.Align);
        }

        private void SetRulerRectangles()
        {
            int rulerWidth = (int) ConvertEx.InchesToPixels(_RulerLength, _Dpi);
            int rulerHeight = Height - Padding.Top - Padding.Bottom;

            _RulerRectangle = new Rectangle(Padding.Left, Padding.Top, rulerWidth, rulerHeight);
            
            if (_RightOffset > 0)
            {
                int offsetInchPixels = (int)Math.Round(ConvertEx.InchesToPixels(_RightOffset, _Dpi));
                int rightMarginOffsetHeight = _RulerRectangle.Height;
                int rightMarginOffsetLeft = _RulerRectangle.Right - offsetInchPixels;
                int rightMarginOffsetTop = _RulerRectangle.Top;
                int rightMarginOffsetWidth = _RulerRectangle.Right - rightMarginOffsetLeft + 2;

                _RightOffsetRectangle = new Rectangle(rightMarginOffsetLeft, rightMarginOffsetTop, rightMarginOffsetWidth, rightMarginOffsetHeight);
            }
            else
            {
                _RightOffsetRectangle = new Rectangle();
            }

            Invalidate();
        }
        #endregion

        #region Protected Methods
        private void OnHangingIndentChanged(EventArgs e)
        {
            if (HangingIndentChanged != null)
            {
                HangingIndentChanged(this, e);
            }
        }

        private void OnLeftMarginChanged(EventArgs e)
        {
            if (LeftMarginChanged != null)
            {
                LeftMarginChanged(this, e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (GetMarkerRectangle(_HangingIndent).Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                _HangingIndent.Pushed = true;
                InvalidateMarker(_HangingIndent);
            }
            else if (GetMarkerRectangle(_LeftMargin).Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                _LeftMargin.Pushed = true;
                InvalidateMarker(_LeftMargin);
            }
            else if (GetMarkerRectangle(_RightMargin).Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                _RightMargin.Pushed = true;
                InvalidateMarker(_RightMargin);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            float tempValue;

            if (_HangingIndent.Pushed)
            {
                tempValue = GetInchesFromRuler(e.X, false, true, true);

                if (tempValue != _HangingIndent.Inches)
                {
                    _HangingIndent.Inches = tempValue;
                    InvalidateMarker(_HangingIndent);
                }
            } 
            else if (_LeftMargin.Pushed)
            {
                tempValue = GetInchesFromRuler(e.X, false, true, true);

                float rightMarginInverse = _RightMargin.GetInverseInches(GetActualRulerLength());

                if (tempValue > rightMarginInverse)
                {
                    tempValue = rightMarginInverse;
                }

                if (tempValue != _LeftMargin.Inches)
                {
                    _LeftMargin.Inches = tempValue;
                    InvalidateMarker(_LeftMargin);
                }
            }
            else if (_RightMargin.Pushed)
            {
                tempValue = GetInchesFromRuler(e.X, true, true, true);

                float leftMarginInverse = _LeftMargin.GetInverseInches(GetActualRulerLength());
                
                if (tempValue > leftMarginInverse)
                {
                    tempValue = leftMarginInverse;
                }

                if (tempValue != _RightMargin.Inches)
                {
                    _RightMargin.Inches = tempValue;
                    InvalidateMarker(_RightMargin);
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_HangingIndent.Pushed)
            {
                _HangingIndent.Pushed = false;
                InvalidateMarker(_HangingIndent);
                OnHangingIndentChanged(new EventArgs());
            }
            
            if (_LeftMargin.Pushed)
            {
                _LeftMargin.Pushed = false;
                InvalidateMarker(_LeftMargin);
                OnLeftMarginChanged(new EventArgs());
            }
            
            if (_RightMargin.Pushed)
            {
                _RightMargin.Pushed = false;
                InvalidateMarker(_RightMargin);
                OnRightMarginChanged(new EventArgs());
            }

            if (ImageMarkerControl != null)
            {
                ImageMarkerControl.Hide();
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            InitializePaint();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _Markers.Clear();
            _PushedMarkers.Clear();
            
            if (!_DoubleBufferedGraphics.Initialized)
            {
                _DoubleBufferedGraphics.Initialize(Width, Height);
            }

            base.OnPaint(new PaintEventArgs(_DoubleBufferedGraphics.Graphics, e.ClipRectangle));

            DrawRuler(_DoubleBufferedGraphics.Graphics);

            if (_ShowHangingIndent)
            {
                if (_HangingIndent.Pushed)
                {
                    _PushedMarkers.Add(_HangingIndent);
                }
                else
                {
                    _Markers.Add(_HangingIndent);
                }
            }

            if (_ShowLeftMargin)
            {
#if DEBUG
                if (_LeftMargins.Pushed)
                {
                    _PushedMarkers.Add(_LeftMargins);
                }
                else
                {
                    _Markers.Add(_LeftMargins);
                }
#endif
                if (_LeftMargin.Pushed)
                {
                    _PushedMarkers.Add(_LeftMargin);
                }
                else
                {
                    _Markers.Add(_LeftMargin);
                }
            }

            if (_ShowRightMargin)
            {
                if (_RightMargin.Pushed)
                {
                    _PushedMarkers.Add(_RightMargin);
                }
                else
                {
                    _Markers.Add(_RightMargin);
                }
            }

            foreach (MarginBarMarkerBase marginBarMarkerBase in _Markers)
            {
                marginBarMarkerBase.Paint(_DoubleBufferedGraphics.Graphics, Padding.Left, GetActualRulerLength());
            }

            foreach (MarginBarMarkerBase marginBarMarkerBase in _PushedMarkers)
            {
                MarginBarMarker marginBarMarker = marginBarMarkerBase as MarginBarMarker;
                
                if (marginBarMarker != null)
                {
                    InvalidateImageMarkerControl(marginBarMarker);
                }

                marginBarMarkerBase.Paint(_DoubleBufferedGraphics.Graphics, Padding.Left, GetActualRulerLength());
            }

            _DoubleBufferedGraphics.Render(e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!_DoubleBufferedGraphics.Initialized)
            {
                _DoubleBufferedGraphics.Initialize(Width, Height);
            }

            base.OnPaintBackground(new PaintEventArgs(_DoubleBufferedGraphics.Graphics, e.ClipRectangle));
            
            _DoubleBufferedGraphics.Render(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InitializePaint();
        }

        private void OnRightMarginChanged(EventArgs e)
        {
            if (RightMarginChanged != null)
            {
                RightMarginChanged(this, e);
            }
        }
        #endregion


    }
}
