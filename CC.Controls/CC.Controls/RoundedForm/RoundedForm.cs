using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CC.Controls
{
    // TODO: Needs comments ...
    // TODO: Needs refactor ...
    public partial class RoundedForm : Form
    {
        //TODO: Move to CC.Utilities.Interop
        //TODO: Fix errors when no CornerRadius is set...
        #region Private Constants
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int HTBORDER = 18;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTCLOSE = 20;
        private const int HTGROWBOX = 4;
        private const int HTLEFT = 10;
        private const int HTMAXBUTTON = 9;
        private const int HTMINBUTTON = 8;
        private const int HTRIGHT = 11;
        private const int HTSYSMENU = 3;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;

        private const int WM_NCHITTEST = 0x84;
        #endregion

        #region Private Fields
        private int cornerRadius;
        private RoundedFormBorderStyle formBorderStyle = RoundedFormBorderStyle.None;
        #endregion

        #region Public Properties
        public bool AllowMove { get; set; }
                
        public int CornerRadius 
        {
            get { return cornerRadius; }
            set
            {
                if (value != cornerRadius)
                {
                    cornerRadius = value;
                    setRegion();
                    Invalidate();
                }
            }
        }

        public new RoundedFormBorderStyle FormBorderStyle
        {
            get { return formBorderStyle; }
            set
            {
                if (value != formBorderStyle)
                {
                    formBorderStyle = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region Private Constructor
        public RoundedForm()
        {
            InitializeComponent();
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            setRegion();
        }
        #endregion

        #region Private Event Handlers
        private void RoundedForm_Resize(object sender, EventArgs e)
        {
            setRegion();
            Invalidate();
        }
        #endregion

        #region Private Methods
        public GraphicsPath createRoundedGraphicsPath(float left, float top, float width, float height, float radius)
        {
            //TODO: Fix the issue with the left and right walls when the height is a small value (probably also impacts the top and bottom walls in conjunction to width)
            GraphicsPath graphicsPath = new GraphicsPath();
            
            if (radius > 0)
            {

                float lineHeight = height - (radius * 2.0f);
                float lineWidth = width - (radius * 2.0f);

                if (width > (radius * 2.0f))
                {
                    graphicsPath.AddLine(left + radius, top, left + lineWidth, top); // Top
                }
                
                graphicsPath.AddArc(left + lineWidth, top, radius * 2, radius * 2, 270, 90); // Top right
                
                if (height > (radius * 2.0f))
                {
                    graphicsPath.AddLine(left + width, top + radius, left + width, top + lineHeight); //Right
                }
                
                graphicsPath.AddArc(left + lineWidth, top + lineHeight, radius * 2, radius * 2, 0, 90); // Bottom right
                
                if (width > (radius * 2.0f))
                {
                    graphicsPath.AddLine(left + lineWidth, top + height, left + radius, top + height); // Bottom
                }
               
                graphicsPath.AddArc(left, top + lineHeight, radius * 2, radius * 2, 90, 90); // Bottom left
               
                if (height > (radius * 2.0f))
                {
                    graphicsPath.AddLine(left, top + lineHeight, left, top + radius); // Left
                }
                
                graphicsPath.AddArc(left, top, radius * 2, radius * 2, 180, 90); // Top left
                //graphicsPath.CloseFigure();
            }

            return graphicsPath;
        }

        private void drawBorder(Graphics graphics)
        {
            switch (FormBorderStyle)
	        {
                case RoundedFormBorderStyle.Raised:
                    {
                        Color[] borderColors = new Color[] { SystemColors.ControlLightLight, SystemColors.ControlLight, SystemColors.ControlDark, SystemColors.ControlDarkDark };

                        for (int i = 1; i <= 4; i++)
                        {
                            using (Pen pen = new Pen(borderColors[i - 1], 1))
                            {
                                using (GraphicsPath graphicsPath = createRoundedGraphicsPath(i, i, Width - (i * 2), Height - (i * 2), cornerRadius))
                                {
                                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics.DrawPath(pen, graphicsPath);
                                }
                            }
                        }
                        break;
                    }
                case RoundedFormBorderStyle.Single:
                    {
                        using (Pen pen = new Pen(SystemColors.ControlDarkDark, 1))
                        {
                            using (GraphicsPath graphicsPath = createRoundedGraphicsPath(1, 1, Width - 2, Height - 2, cornerRadius))
                            {
                                graphics.SmoothingMode = SmoothingMode.HighQuality;
                                graphics.DrawPath(pen, graphicsPath);
                            }
                        }
                        break;
                    }
                default:
                    break;
	        }

        }

        private void setRegion()
        {
            using (GraphicsPath graphicsPath = createRoundedGraphicsPath(0,0,Width,Height, cornerRadius))
            {
                Region = new Region(graphicsPath);
            }
        }
        #endregion

        #region Protected Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            drawBorder(e.Graphics);
        }

        protected override void WndProc(ref Message message) 
        {
            base.WndProc(ref message);
            
            if (message.Msg != WM_NCHITTEST)
            {
                return;
            }
            
            //TODO: Implement resize functionality
            switch (message.Result.ToInt32())
            {
                case HTLEFT:
                    {
                        Cursor = Cursors.SizeWE;
                        break;
                    }
		        default:
                    {
                        Cursor = Cursors.Default;
                        break;
                    }
            }

            if (AllowMove && message.Result.ToInt32() == HTCLIENT)
            {
                message.Result = (IntPtr)HTCAPTION;
            }
        }
        #endregion
    }
}
