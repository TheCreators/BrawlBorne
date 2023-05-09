using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattleFieldGenerator
    {
        public BattleField BattleField;
        private double _wallDensity = 0.05;
        private double _bushesDensity = 0.03;
        private int _maxBushSize = 3;
        private int _potsCount = 9;
        public BattleFieldGenerator(BattleField battleField)
        {
            this.BattleField = battleField;
        }

        // Этап 1
        public BattleFieldGenerator GenerateExternalWalls()
        {
            for (int i = 0; i < BattleField.Cols; i++)
            {
                BattleField[i, 0] = 2;
            }

            for (int i = 0; i < BattleField.Rows; i++)
            {
                BattleField[0, i] = 2;
            }

            return this;
        }

        public BattleFieldGenerator GenerateWalls()
        {
            Random random = new Random();

            for (int i = 1; i < BattleField.Cols; i++)
            {
                double lengthWall = 0;
                for (int j = 1; j < BattleField.Rows; j++)
                {
                    double wallChance = _wallDensity + lengthWall;
                    if (BattleField[i - 1, j] == 1)
                    {
                        wallChance += 0.2;
                    }

                    if (random.NextDouble() < wallChance)
                    {
                        BattleField[i, j] = 1;
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
                        BattleField[i, j] = 0;
                    }
                }
            }


            return this;
        }

        public BattleFieldGenerator DeleteSingleWalls()
        {
            for (int i = 1; i < BattleField.Cols - 1; i++)
            {
                for (int j = 1; j < BattleField.Rows - 1; j++)
                {
                    if (BattleField[i, j] == 1)
                    {
                        if (BattleField[i - 1, j] == 0 && BattleField[i, j - 1] == 0 && BattleField[i + 1, j] == 0 && BattleField[i, j + 1] == 0)
                        {
                            BattleField[i, j] = 0;
                        }
                    }
                }
            }

            return this;
        }

        public BattleFieldGenerator AddBushes()
        {
            
            int rows = BattleField.Cols;
            int columns = BattleField.Rows;
            int cellsCount = rows * columns;

            int bushesCount = (int) (_bushesDensity * cellsCount);

            Random rnd = new Random();
            for (int i = 0; i < bushesCount; i++)
            {
                int row = rnd.Next(rows);
                int column = rnd.Next(columns);


                int bushSize = rnd.Next(_maxBushSize - 1) + 2;
                for (int j = 0; j < bushSize; j++)
                {
                    for (int k = 0; k < bushSize; k++)
                    {
                        if (row + j < rows && column + k < columns && BattleField[row + j, column + k] == 0)
                        {
                            BattleField[row + j, column + k] = 3;
                        }
                    }
                }
            }

            return this;
        }

        public BattleFieldGenerator AddPots()
        {
            Random rnd = new Random();
            
            int middleX = BattleField.Cols / 2;
            int middleY = BattleField.Rows / 2;
            int potsOnField = 0;
            while (_potsCount > potsOnField)
            {
                int potPositionX = potsOnField % 2 == 0 ? middleX : 0;
                int potPositionY = (potsOnField % 4 + (_potsCount + 1) % 2) % 4 < 2 ? middleY : 0;
                while (true)
                {
                    int x = potPositionX + rnd.Next(middleX);
                    int y = potPositionY + +rnd.Next(middleY);
                    if (BattleField[x, y] == 0)
                    {
                        BattleField[x, y] = 5;
                        potsOnField++;
                        break;
                    }
                }
            }

            return this;
        }
        
        public BattleFieldGenerator AddObstacles()
        {
            for (int i = 0; i <= BattleField.Cols - 5; i++) 
            {
                for (int j = 0; j <= BattleField.Rows - 5; j++) 
                {
                    bool isSquareZero = true;
                    for (int k = i; k < i + 5; k++)
                    {
                        for (int l = j; l < j + 5; l++)
                        {
                            if (BattleField[k, l] != 0) 
                            {
                                isSquareZero = false;
                                break; 
                            }
                        }
                        if (!isSquareZero) 
                            break;
                    }

                    if (isSquareZero)
                    {
                        BattleField[i, j] = 6;
                        i += 5;
                        j += 5;
                    }
                }
            }
            return this;
        }

        public BattleField BuildMap()
        {
            int rows = BattleField.Rows;
            int cols = BattleField.Cols;

            int[,] symmetricMatrix = new int[rows * 2, cols * 2];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    symmetricMatrix[i, j] = BattleField[i, j];
                    symmetricMatrix[rows * 2 - i - 1, j] = BattleField[i, j];
                    symmetricMatrix[i, cols * 2 - j - 1] = BattleField[i, j];
                    symmetricMatrix[rows * 2 - i - 1, cols * 2 - j - 1] = BattleField[i, j];
                }
            }

            return new BattleField(symmetricMatrix);
        }
    }
