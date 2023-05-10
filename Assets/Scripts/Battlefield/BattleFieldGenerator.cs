using Random = System.Random;

namespace Battlefield
{
    public class BattleFieldGenerator
    {
        private readonly BattleField _battleField;
        private const double WallDensity = 0.05;
        private const double BushesDensity = 0.03;
        private const int MaxBushSize = 3;
        private const int PotsCount = 9;
        private readonly Random _random = new Random();

        public BattleFieldGenerator(BattleField battleField)
        {
            _battleField = battleField;
        }

        public BattleFieldGenerator GenerateExternalWalls()
        {
            for (int i = 0; i < _battleField.Cols; i++)
            {
                _battleField[i, 0] = 2;
            }

            for (int i = 0; i < _battleField.Rows; i++)
            {
                _battleField[0, i] = 2;
            }

            return this;
        }

        public BattleFieldGenerator GenerateWalls()
        {
            for (int i = 1; i < _battleField.Cols; i++)
            {
                double lengthWall = 0;
                for (int j = 1; j < _battleField.Rows; j++)
                {
                    double wallChance = WallDensity + lengthWall;
                    if (_battleField[i - 1, j] == 1)
                    {
                        wallChance += 0.2;
                    }

                    if (_random.NextDouble() < wallChance)
                    {
                        _battleField[i, j] = 1;
                        if (lengthWall <= 0)
                        {
                            lengthWall = 1;
                        }
                        else
                        {
                            lengthWall -= 0.3;
                        }
                    }
                    else
                    {
                        _battleField[i, j] = 0;
                    }
                }
            }


            return this;
        }

        public BattleFieldGenerator DeleteSingleWalls()
        {
            for (int i = 1; i < _battleField.Cols - 1; i++)
            {
                for (int j = 1; j < _battleField.Rows - 1; j++)
                {
                    if (_battleField[i, j] == 1)
                    {
                        if (_battleField[i - 1, j] == 0 && _battleField[i, j - 1] == 0 && _battleField[i + 1, j] == 0 && _battleField[i, j + 1] == 0)
                        {
                            _battleField[i, j] = 0;
                        }
                    }
                }
            }

            return this;
        }

        public BattleFieldGenerator AddBushes()
        {
            
            int rows = _battleField.Cols;
            int columns = _battleField.Rows;
            int cellsCount = rows * columns;

            int bushesCount = (int) (BushesDensity * cellsCount);

            for (int i = 0; i < bushesCount; i++)
            {
                int row = _random.Next(rows);
                int column = _random.Next(columns);
                
                int bushSize = _random.Next(MaxBushSize - 1) + 2;
                for (int j = 0; j < bushSize; j++)
                {
                    for (int k = 0; k < bushSize; k++)
                    {
                        if (row + j < rows && column + k < columns && _battleField[row + j, column + k] == 0)
                        {
                            _battleField[row + j, column + k] = 3;
                        }
                    }
                }
            }

            return this;
        }

        public BattleFieldGenerator AddPots()
        {
            int middleX = _battleField.Cols / 2;
            int middleY = _battleField.Rows / 2;
            int potsOnField = 0;
            while (PotsCount > potsOnField)
            {
                int potPositionX = potsOnField % 2 == 0 ? middleX : 0;
                int potPositionY = (potsOnField % 4 + (PotsCount + 1) % 2) % 4 < 2 ? middleY : 0;
                while (true)
                {
                    int x = potPositionX + _random.Next(middleX);
                    int y = potPositionY + _random.Next(middleY);
                    if (_battleField[x, y] == 0)
                    {
                        _battleField[x, y] = 5;
                        potsOnField++;
                        break;
                    }
                }
            }

            return this;
        }

        public BattleField BuildMap()
        {
            int rows = _battleField.Rows;
            int cols = _battleField.Cols;

            int[,] symmetricMatrix = new int[rows * 2, cols * 2];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    symmetricMatrix[i, j] = _battleField[i, j];
                    symmetricMatrix[rows * 2 - i - 1, j] = _battleField[i, j];
                    symmetricMatrix[i, cols * 2 - j - 1] = _battleField[i, j];
                    symmetricMatrix[rows * 2 - i - 1, cols * 2 - j - 1] = _battleField[i, j];
                }
            }

            return new BattleField(symmetricMatrix);
        }
    }
}
