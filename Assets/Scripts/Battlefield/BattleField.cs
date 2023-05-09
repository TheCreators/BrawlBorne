using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleField
{
    public int Rows { get; set; }
    public int Cols { get; set; }
    private int[,] field;

    public BattleField(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        field = new int[rows, cols];
    }

    public BattleField(int[,] matrix)
    {
        Rows = matrix.GetLength(0);
        Cols = matrix.GetLength(1);
        field = matrix;
    }

    public void PrintField()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write(field[i, j]);
                Console.Write(' ');
            }

            Console.WriteLine();
        }
    }

    public int this[int row, int col]
    {
        get { return field[row, col]; }
        set { field[row, col] = value; }
    }
}
