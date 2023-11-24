using System.Drawing;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        private Board _board;
        private (int y, int x) _boardInitCoord;
        private int _size;

        private Pen _gridPen;
        private Brush _placedTileBrush;
        private Brush _currentTileBrush;

        private (int y, int x) _tileStartCoord;

        private int[] _xMasked;

        public TetrisForm()
        {
            InitializeComponent();
            _gridPen = new Pen(Color.DarkGray, 0.1f);
            _placedTileBrush = new SolidBrush(Color.PaleGreen);
            _currentTileBrush = new SolidBrush(Color.OrangeRed);

            _board = new Board(24, 10)!;
            _size = 18;
            _boardInitCoord = (20, 20);
            _tileStartCoord = (0, 4);

            _xMasked = new int[10];
            for(int i = 0; i < 10; i++)
            {
                _xMasked[i] = (1 << i);
            }
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
            List<Rectangle> currentRectList = new List<Rectangle>();
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    var rect = new Rectangle(
                        _boardInitCoord.x + x * _size,
                        _boardInitCoord.y + y * _size,
                        _size - 1, _size - 1);

                    g.DrawRectangle(_gridPen, rect);

                    if ((_board.Placed[y] & _xMasked[x]) == _xMasked[x])
                    {
                        placedRectList.Add(rect);
                    }

                    if (y <_board.CurrentTile?.Patterns?[0].Length &&
                        (_board.CurrentTile?.Patterns?[0][y] << _tileStartCoord.x & _xMasked[x]) == _xMasked[x])
                    {
                        currentRectList.Add(rect);
                    }
                }
            }

            //Placed Tile
            if (placedRectList.Count > 0)
            {
                g.FillRectangles(_placedTileBrush, placedRectList.ToArray());
            }

            //Current Tile
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