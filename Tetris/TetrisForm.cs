using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Drawing;
using System.Timers;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        private readonly Board _board;
        private (int y, int x) _boardInitCoord;
        private int _size;

        private Pen _gridPen;
        private Brush _placedTileBrush;
        private Brush _currentTileBrush;

        private int[] _xMasked;

        private System.Windows.Forms.Timer _frameTimer;
        private System.Windows.Forms.Timer _inputTimer;
        private bool _canInput;

        public TetrisForm()
        {
            DoubleBuffered = true;

            InitializeComponent();
            _gridPen = new Pen(Color.DarkGray, 0.1f);
            _placedTileBrush = new SolidBrush(Color.PaleGreen);
            _currentTileBrush = new SolidBrush(Color.OrangeRed);

            _board = new Board(24, 10)!;
            _size = 18;
            _boardInitCoord = (20, 20);

            _xMasked = new int[10];
            for (int i = 0; i < 10; i++)
            {
                _xMasked[i] = (1 << i);
            }

            _board.Start();

            _frameTimer = new System.Windows.Forms.Timer();
            _frameTimer.Interval = 50;
            _frameTimer.Tick += Update!;
            _frameTimer.Start();

            _inputTimer = new System.Windows.Forms.Timer();
            _inputTimer.Interval = 10;
            _inputTimer.Tick += ResetInput!;
            _inputTimer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ResetInput(object sender, EventArgs e)
        {
            if(!_canInput)
            {
                _canInput = true;
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if(_canInput)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        _board.Turn(); break;
                    case Keys.Right:
                        _board.MoveRight(); break;
                    case Keys.Left:
                        _board.MoveLeft(); break;
                    case Keys.Down:
                        _board.Fall(); break;
                    case Keys.Space:
                        _board.HardFall(); break;
                }
                _inputTimer.Start();
                _canInput = false;
            }

            return base.IsInputKey(keyData);
        }

        private void DrawUI(Graphics g)
        {
            //OutLine
            g.DrawRectangle(
                _gridPen, 
                _boardInitCoord.x - 1,
                _boardInitCoord.y - 1,
                _size * _board.Width + 1 ,
                _size * _board.Height + 1
                );

            //Grid
            List<Rectangle> placedRectList = new List<Rectangle>();
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    var rect = new Rectangle(
                        _boardInitCoord.x + x * _size,
                        _boardInitCoord.y + y * _size,
                        _size - 1, _size - 1);

                    g.DrawRectangle(_gridPen, rect);

                    if ((_board.Placed[y] & _xMasked[x]) != 0)
                    {
                        placedRectList.Add(rect);
                    }
                }
            }

            //Placed Tile
            if (placedRectList.Count > 0)
            {
                g.FillRectangles(_placedTileBrush, placedRectList.ToArray());
            }

            //Current Tile
            List<Rectangle> currentRectList = new List<Rectangle>();
            if(_board.CurrentTile != null)
            {
                (int offSetY, int offSetX) = (_board.CurrentTile.Y, _board.CurrentTile.X);

                for(int y = 0; y < _board.CurrentTile.Pattern.bits.Length; y++)
                {
                    int patternMask = _board.CurrentTile.Pattern.bits[y] << offSetX;

                    for (int x = offSetX; x < offSetX + _board.CurrentTile.Pattern.x ; x++)
                    {
                        if ((patternMask & _xMasked[x]) != 0)
                        {
                            var rect = new Rectangle(
                                _boardInitCoord.x + x * _size,
                                _boardInitCoord.y + (y + offSetY) * _size,
                                _size - 1, _size - 1);

                            currentRectList.Add(rect);
                        }
                    }
                }
            }

            if(currentRectList.Count > 0)
            {
                g.FillRectangles(_currentTileBrush, currentRectList.ToArray());
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawUI(e.Graphics);
        }
    }
}