using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FloatingRuler
{
    public class RulerForm : Form
    {
        private float angle = 0f;
        private int rulerWidth = 600;
        private int rulerHeight = 50;

        private bool dragging = false;
        private bool resizingLeft = false;
        private bool resizingRight = false;

        private Point dragStart;

        private const int resizeMargin = 15;

        public RulerForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.BackColor = Color.Black;
            this.Opacity = 0.95;

            UpdateFormBounds();

            this.MouseDown += MouseDownHandler;
            this.MouseMove += MouseMoveHandler;
            this.MouseUp += (s, e) =>
            {
                dragging = false;
                resizingLeft = false;
                resizingRight = false;
            };

            this.MouseWheel += MouseWheelHandler;
        }

        // ---------------- ROTATION ----------------

        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                // 45 derece snap
                if (e.Delta > 0)
                    angle = (float)(Math.Round((angle + 45) / 45) * 45);
                else
                    angle = (float)(Math.Round((angle - 45) / 45) * 45);
            }
            else
            {
                // hassas dönüş
                angle += e.Delta > 0 ? 2f : -2f;
            }

            UpdateFormBounds();
            UpdateRegion();
            Invalidate();
        }

        // ---------------- DRAG & RESIZE ----------------

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            dragStart = Cursor.Position;

            if (e.X < resizeMargin)
                resizingLeft = true;
            else if (e.X > Width - resizeMargin)
                resizingRight = true;
            else
                dragging = true;
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (resizingLeft)
            {
                int diff = Cursor.Position.X - dragStart.X;
                rulerWidth -= diff;
                if (rulerWidth < 100) rulerWidth = 100;

                dragStart = Cursor.Position;
                UpdateFormBounds();
                UpdateRegion();
                Invalidate();
            }
            else if (resizingRight)
            {
                int diff = Cursor.Position.X - dragStart.X;
                rulerWidth += diff;
                if (rulerWidth < 100) rulerWidth = 100;

                dragStart = Cursor.Position;
                UpdateFormBounds();
                UpdateRegion();
                Invalidate();
            }
            else if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragStart));
                Location = Point.Add(Location, new Size(diff));
                dragStart = Cursor.Position;
            }
            else
            {
                if (e.X < resizeMargin || e.X > Width - resizeMargin)
                    Cursor = Cursors.SizeWE;
                else
                    Cursor = Cursors.SizeAll;
            }
        }

        // ---------------- BOUNDS & REGION ----------------

        private void UpdateFormBounds()
        {
            double rad = angle * Math.PI / 180.0;

            int w = (int)(Math.Abs(rulerWidth * Math.Cos(rad)) +
                          Math.Abs(rulerHeight * Math.Sin(rad)));

            int h = (int)(Math.Abs(rulerWidth * Math.Sin(rad)) +
                          Math.Abs(rulerHeight * Math.Cos(rad)));

            this.Size = new Size(w + 20, h + 20);
        }

        private void UpdateRegion()
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle rect = GetCenteredRect();

            Matrix m = new Matrix();
            m.RotateAt(angle,
                new PointF(Width / 2f, Height / 2f));

            path.AddRectangle(rect);
            path.Transform(m);

            this.Region = new Region(path);
        }

        private Rectangle GetCenteredRect()
        {
            return new Rectangle(
                (Width - rulerWidth) / 2,
                (Height - rulerHeight) / 2,
                rulerWidth,
                rulerHeight);
        }

        // ---------------- DRAW ----------------

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float cx = Width / 2f;
            float cy = Height / 2f;

            g.TranslateTransform(cx, cy);
            g.RotateTransform(angle);
            g.TranslateTransform(-cx, -cy);

            Rectangle rect = GetCenteredRect();

            // Cetvel arka planı
            g.FillRectangle(Brushes.WhiteSmoke, rect);
            g.DrawRectangle(Pens.Black, rect);

            float dpiX = g.DpiX;
            float pixelsPerCm = dpiX / 2.54f;

            float numberAreaHeight = 18;   // üstte rakam alanı
            float tickTop = rect.Top + numberAreaHeight;

            for (int i = 0; i < rulerWidth; i++)
            {
                float x = rect.Left + i;

                // mm çizgileri
                if (i % (pixelsPerCm / 10) < 1)
                {
                    g.DrawLine(Pens.Black, x,
                        rect.Bottom - 10,
                        x,
                        rect.Bottom);
                }

                // cm çizgileri
                if (i % pixelsPerCm < 1)
                {
                    g.DrawLine(Pens.Black, x,
                        tickTop,
                        x,
                        rect.Bottom);

                    int cm = (int)(i / pixelsPerCm);

                    // Rakamı rectangle İÇİNDE yaz
                    g.DrawString(
                        cm.ToString(),
                        Font,
                        Brushes.Black,
                        x + 2,
                        rect.Top + 2);
                }
            }
        }
    }
}