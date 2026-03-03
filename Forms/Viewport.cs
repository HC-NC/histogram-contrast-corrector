using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Histogram_Contrast_Corrector
{
    public partial class Viewport : UserControl
    {
        private Graphics _graphics;
        private Image? _img;
        private Point _mouseDown;
        private int _startx = 0; // offset of image when mouse was pressed
        private int _starty = 0;
        private int _imgx = 0; // current offset of image
        private int _imgy = 0;

        private bool _mousepressed = false; // true as long as left mousebutton is pressed
        private bool _mouseOnPicture = false;
        private float _zoom = 1;

        private InterpolationMode _interpolationMode = InterpolationMode.NearestNeighbor;

        public Viewport()
        {
            InitializeComponent();
        }

        private void Viewport_Load(object sender, EventArgs e)
        {
            _graphics = CreateGraphics();
        }

        public void UpdateImage(Image? img, InterpolationMode interpolationMode = InterpolationMode.Default)
        {
            _img = img;
            _interpolationMode = interpolationMode;
            ResetImage(this, EventArgs.Empty);
        }

        private void ResetImage(object sender, EventArgs e)
        {
            if (_img == null)
                return;

            _zoom = Math.Min(
             ((float)pictureBox.Height / (float)_img.Height) * (_img.VerticalResolution / _graphics.DpiY),
             ((float)pictureBox.Width / (float)_img.Width) * (_img.HorizontalResolution / _graphics.DpiX)
            );

            _imgx = (int)(pictureBox.Width * 0.5f / _zoom - _img.Width * 0.5f);
            _imgy = (int)(pictureBox.Height * 0.5f / _zoom - _img.Height * 0.5f);

            pictureBox.Refresh();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_img == null)
            {
                e.Graphics.Clear(Color.White);
                return;
            }

            e.Graphics.InterpolationMode = _interpolationMode;
            e.Graphics.ScaleTransform(_zoom, _zoom);
            e.Graphics.DrawImage(_img, _imgx, _imgy);
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_mousepressed)
                {
                    _mousepressed = true;
                    _mouseDown = e.Location;
                    _startx = _imgx;
                    _starty = _imgy;

                    Cursor.Current = Cursors.Hand;
                }
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mousepressed = false;

            Cursor.Current = Cursors.Default;
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            _mouseOnPicture = true;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            _mouseOnPicture = false;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosNow = e.Location;

                // the distance the mouse has been moved since mouse was pressed
                int deltaX = mousePosNow.X - _mouseDown.X;
                int deltaY = mousePosNow.Y - _mouseDown.Y;

                // calculate new offset of image based on the current zoom factor
                _imgx = (int)(_startx + (deltaX / _zoom));
                _imgy = (int)(_starty + (deltaY / _zoom));

                pictureBox.Refresh();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_mouseOnPicture)
            {
                float oldzoom = _zoom;

                if (e.Delta > 0)
                {
                    _zoom *= 1.1F;
                }
                else if (e.Delta < 0)
                {
                    _zoom *= 0.9F;
                }

                MouseEventArgs mouse = e as MouseEventArgs;
                Point mousePosNow = mouse.Location;

                Point pBoxLocation = PointToClient(pictureBox.Parent.PointToScreen(pictureBox.Location));

                // Where location of the mouse in the pictureframe
                int x = mousePosNow.X - pBoxLocation.X;
                int y = mousePosNow.Y - pBoxLocation.Y;

                // Where in the IMAGE is it now
                int oldimagex = (int)(x / oldzoom);
                int oldimagey = (int)(y / oldzoom);

                // Where in the IMAGE will it be when the new zoom i made
                int newimagex = (int)(x / _zoom);
                int newimagey = (int)(y / _zoom);

                // Where to move image to keep focus on one point
                _imgx = newimagex - oldimagex + _imgx;
                _imgy = newimagey - oldimagey + _imgy;

                pictureBox.Refresh();  // calls imageBox_Paint
            }

            base.OnMouseWheel(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if (pictureBox.Focused && ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN)))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        _imgx -= (int)(pictureBox.Width * 0.1F / _zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Left:
                        _imgx += (int)(pictureBox.Width * 0.1F / _zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Down:
                        _imgy -= (int)(pictureBox.Height * 0.1F / _zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Up:
                        _imgy += (int)(pictureBox.Height * 0.1F / _zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageDown:
                        _imgy -= (int)(pictureBox.Height * 0.90F / _zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageUp:
                        _imgy += (int)(pictureBox.Height * 0.90F / _zoom);
                        pictureBox.Refresh();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
