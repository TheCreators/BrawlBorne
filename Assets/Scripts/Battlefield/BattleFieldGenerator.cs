using Random = System.Random;

namespace Battlefield
{
    public class BattleFieldGenerator
    {
        private BattleField _battleField;
        private readonly float _wallDensity;
        private readonly float _bushesDensity;
        private readonly int _maxBushSize;
        private readonly int _potsCount;
        private readonly int _heroesCount;
        private readonly int _spawnDistance;
        private readonly Random _random = new Random();

        public BattleFieldGenerator(BattleField battleField,float wallDensity,float bushesDensity,int maxBushSize,int potsCount , int heroesCount, int spawnDistance)
        {
            _battleField = battleField;
            _wallDensity = wallDensity;
            _bushesDensity = bushesDensity;
            _maxBushSize = maxBushSize;
            _potsCount = potsCount;
            _heroesCount = heroesCount;
            _spawnDistance = spawnDistance;
        }
        bool IsIndexValid(int row, int col) => row >= 0 && row < _battleField.Rows && col >= 0 && col < _battleField.Cols;

public bool HasGround()
        {
            int wallsCount = 0;
            for (int i = 0; i < _battleField.Rows; i++)
            {
                for (int j = 0; j < _battleField.Cols; j++)
                {
                    if (_battleField[i, j] == 1)
                    {
                        wallsCount++;
                    }
                }
            }
    
            if (4 * wallsCount / 3 > _battleField.Rows * _battleField.Cols)
            {
                return false;
            }

            return true;
        }
        public BattleFieldGenerator GenerateExternalWalls()
        {
            for (int i = 0; i < _battleField.Rows; i++)
            {
                _battleField[i, 0] = 2;
            }

            for (int i = 0; i < _battleField.Cols; i++)
            {
                _battleField[0, i] = 2;
            }

            return this;
        }

        public BattleFieldGenerator GenerateWalls()
        {
            for (int i = 1; i < _battleField.Rows; i++)
            {
                double lengthWall = 0;
                for (int j = 1; j < _battleField.Cols; j++)
                {
                    double wallChance = _wallDensity + lengthWall;
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
            for (int i = 1; i < _battleField.Rows - 1; i++)
            {
                for (int j = 1; j < _battleField.Cols - 1; j++)
                {
                    if (_battleField[i, j] == 1)
                    {
                        if (_battleField[i - 1, j] == 0 && _battleField[i, j - 1] == 0 && _battleField[i + 1, j] == 0 &&
                            _battleField[i, j + 1] == 0)
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

            int bushesCount = (int) (_bushesDensity * cellsCount);

            for (int i = 0; i < bushesCount; i++)
            {
                int row = _random.Next(rows);
                int column = _random.Next(columns);

                int bushSize = _random.Next(_maxBushSize - 1) + 2;
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
            int middleX = _battleField.Rows / 2;
            int middleY = _battleField.Cols / 2;
            int potsOnField = 0;
            while (_potsCount > potsOnField)
            {
                int potPositionX = potsOnField % 2 == 0 ? middleX : 0;
                int potPositionY = (potsOnField % 4 + (_potsCount + 1) % 2) % 4 < 2 ? middleY : 0;
                int checker = 0;
                while (checker < 50)
                {
                checker++;
                    int x = potPositionX + _random.Next(middleX);
                    int y = potPositionY + _random.Next(middleY);
                    if (_battleField[x, y] == 0 || _battleField[x, y] == 3)
                    {
                        _battleField[x, y] = 5;
                        break;
                    }
                }
                potsOnField++;
            }

            return this;
        }

        private void SpawnHero(int x, int y)
        {
            if (IsIndexValid(x, y))
            {
                while (_battleField[x, y] != 0 && _battleField[x, y] != 3)
                {
                    do
                    {
                        x += _random.Next(3) - 1;
                        y += _random.Next(3) - 1;
                    } while (!IsIndexValid(x, y));

                }
                _battleField[x, y] = 4;
            }
        }
        public BattleFieldGenerator AddHeroesSpots()
                {
                    int firstSpotX = _battleField.Rows / _spawnDistance;
                    int firstSpotY = _battleField.Cols / _spawnDistance;
                    int squareSizeX = _battleField.Rows - 2*firstSpotX;
                    int squareSizeY = _battleField.Cols -2* firstSpotY;
                    
                    int spacing = (squareSizeX+squareSizeY) * 2 / _heroesCount;
                    int x = 0, y = 0;
                    for (int i = 0; i < _heroesCount; i++)
                    {
                        SpawnHero(firstSpotX + x, firstSpotY + y);
                        if (y == 0)
                        {
                            x += spacing;
                            if (x >= squareSizeX)
                            {
                                y = x - squareSizeX;
                                x = squareSizeX;
                            }
                        }
                        else if (x == squareSizeX)
                        {
                            y += spacing;
                            if (y >= squareSizeY)
                            {
                                x = squareSizeX - (y - squareSizeY);
                                y = squareSizeY;
                            }
                        }
                        else if (y == squareSizeY)
                        {
                            x -= spacing;
                            if (x < 0)
                            {
                                y = squareSizeY + x;
                                x = 0;
                            }
                        }
                        else
        
                        {
                            y -= spacing;
                        }
                    }
        
                    return this;
                }
        public BattleFieldGenerator FillEmpties()
        {
            int rows = _battleField.Rows;
            int cols = _battleField.Cols;

            bool[,] visited = new bool[rows, cols];
            
            bool CanMove(int row, int col) => IsIndexValid(row, col) && _battleField[row, col] != 1 &&
                                              _battleField[row, col] != 2 && !visited[row, col];

            bool DFS(int row, int col)
            {
                visited[row, col] = true;

                bool canMoveLeft = CanMove(row, col - 1) && DFS(row, col - 1);
                bool canMoveRight = CanMove(row, col + 1) && DFS(row, col + 1);
                bool canMoveUp = CanMove(row - 1, col) && DFS(row - 1, col);
                bool canMoveDown = CanMove(row + 1, col) && DFS(row + 1, col);

                return canMoveLeft && canMoveRight && canMoveUp && canMoveDown;
            }

            int startRow = 0;
            int startCol = 0;
            for (int i = rows/2; i < rows; i++)
            {
                for (int j = cols/2; j < cols; j++)
                {
                    if (_battleField[i, j] == 0)
                    {
                        startRow = i;
                        startCol = j;
                        break;
                    }
                }
            }

            DFS(startRow, startCol);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (visited[i, j] == false && _battleField[i, j] != 2)
                    {
                        _battleField[i, j] = 1;
                    }
                }
            }

            return this;
        }

        public BattleFieldGenerator MakeSymmetric()
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

            _battleField = new BattleField(symmetricMatrix);
            return this;
        }

        public BattleField BuildMap()
        {
            return _battleField;
        }
    }
}