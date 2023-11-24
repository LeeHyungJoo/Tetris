namespace Tetris
{
    public partial class TetrisForm : Form
    {
        private Board _board;
        private (int y, int x) _boardInitCoord;
        private int _size;

        public TetrisForm()
        {
            InitializeComponent();
            _board = new Board(24, 10)!;
            _size = 18;
            _boardInitCoord = (20, 20);
        }

        private void DrawUI(Graphics g)
        {
            Pen gridPen = new Pen(Color.DarkGray, 0.2f);
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    g.DrawRectangle(
                        gridPen,
                        _boardInitCoord.x + x * _size,
                        _boardInitCoord.y + y * _size,
                        _size, _size
                        );
                    
                }
            }
            gridPen.Dispose();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawUI(e.Graphics);
        }

    }
}