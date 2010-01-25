using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public partial class WatermarkTextBox : TextBox
    {
        #region Constructor
        public WatermarkTextBox()
        {
            InitializeComponent();

            _Font = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
            _WatermarkEnabled = string.IsNullOrEmpty(Text);

            SetStyle(ControlStyles.UserPaint, _WatermarkEnabled);
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        #endregion

        #region Private Fields
        private Font _Font;
        private HorizontalAlignment _WatermarkAlign = HorizontalAlignment.Left;
        private Color _WatermarkColor = Color.Gray;
        private bool _WatermarkEnabled;
        private string _WatermarkText = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets how the watermark is aligned
        /// </summary>
        public HorizontalAlignment WatermarkAlign
        {
            get { return _WatermarkAlign; }
            set { _WatermarkAlign = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the textcolor of the watermark
        /// </summary>
        [Category("Appearance")]
        public Color WatermarkColor
        {
            get { return _WatermarkColor; }
            set { _WatermarkColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the watermark text
        /// </summary>
        public string WatermarkText
        {
            get { return _WatermarkText; }
            set { _WatermarkText = value; Invalidate(); }
        }
        #endregion

        #region Protected Methods
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _Font = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                StringFormat stringFormat = new StringFormat
                                                {
                                                    LineAlignment = StringAlignment.Center,
                                                    Trimming = StringTrimming.EllipsisWord
                                                };

                switch (WatermarkAlign)
                    {
                        case HorizontalAlignment.Center:
                            {
                                stringFormat.Alignment = StringAlignment.Center;
                                break;
                            }
                        case HorizontalAlignment.Right:
                            {
                                stringFormat.Alignment = StringAlignment.Far;
                                break;
                            }
                        default:
                            {
                                stringFormat.Alignment = StringAlignment.Near;
                                break;
                            }
                }

                e.Graphics.DrawString(_WatermarkText, Font, new SolidBrush(_WatermarkColor), ClientRectangle, stringFormat);
            }

            base.OnPaint(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            bool watermarkEnabledChanged = false;

            if (string.IsNullOrEmpty(Text) && !_WatermarkEnabled)
            {
                _Font = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
                _WatermarkEnabled = true;
                watermarkEnabledChanged = true;
            }
            else if (_WatermarkEnabled)
            {
                _WatermarkEnabled = false;
                watermarkEnabledChanged = true;
            }

            if (watermarkEnabledChanged)
            {
                SetStyle(ControlStyles.UserPaint, _WatermarkEnabled);

                if (!_WatermarkEnabled && _Font != null)
                {
                    Font = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);    
                }

                Refresh();
            }
        }
        #endregion
    }
}
