namespace Battlefield
{
    public class BattleField
    {
        public int Rows { get; }
        public int Cols { get; }
        private readonly int[,] _field;

        public BattleField(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _field = new int[rows, cols];
        }

        public BattleField(int[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Cols = matrix.GetLength(1);
            _field = matrix;
        }

        public int this[int row, int col]
        {
            get => _field[row, col];
            set => _field[row, col] = value;
        }
    }
}
