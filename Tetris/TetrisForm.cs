using System.Drawing;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        private Board _board;
        private (int y, int x) _boardInitCoord;
        private int _size;

        private Pen gridPen;
        private Brush brush;

        private int[] _masking;

        public TetrisForm()
        {
            InitializeComponent();
            gridPen = new Pen(Color.DarkGray, 0.1f);
            brush = new SolidBrush(Color.Azure);

            _board = new Board(24, 10)!;
            _size = 18;
            _boardInitCoord = (20, 20);

            _masking = new int[10];
            for(int i = 0; i < 10; i++)
            {
                _masking[i] = (1 << i);
            }
        }

        private void DrawUI(Graphics g)
        {
            List<Rectangle> placedRectList = new List<Rectangle>();
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    var rect = new Rectangle(
                        _boardInitCoord.x + x * _size,
                        _boardInitCoord.y + y * _size,
                        _size - 1, _size - 1);

                    g.DrawRectangle(gridPen, rect);


                    if ((_board.Placed[y] & _masking[x]) == _masking[x])
                    {
                        placedRectList.Add(rect);
                    }
                }
            }

            if (placedRectList.Count > 0)
            {
                g.FillRectangles(brush, placedRectList.ToArray());
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawUI(e.Graphics);
        }

    }
}