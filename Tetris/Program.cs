namespace Tetris
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TileFactory.Instance.MakeData();

            ApplicationConfiguration.Initialize();
            Application.Run(new TetrisForm());
        }
    }
}