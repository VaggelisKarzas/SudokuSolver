using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public unsafe class Sudoku : Button
    {
        int[,] sudoku;
       
        public Sudoku()
        {
            this.sudoku = new int[,]
            { { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
              { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
              { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
              { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
              { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
              { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
              { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
              { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
              { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
            };
        }
        public bool isSudokuValid(int[,] array)
        {

            int length = array.GetLength(0);

            for (int x = 0; x < 100; x++)
            {
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (array[j, i] == 0)
                        {
                            for (int num = 1; num < 10; num++)
                            {
                                if (isSafe(array, num, i, j))
                                {
                                    array[j, i] = num;

                                    break;
                                }

                            }
                        }

                    }
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {                      
                    if (array[j, i] == 0)
                        return false;
                }
            }

                    return true;
        }

        public int[,] solveSudoku(int[,] sudoku, Sudoku[,] cells ,int* terminate, Label label,bool stepbytstep)
        {
            int length = sudoku.GetLength(0);

            while (!isSolution(sudoku))
            {                
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (sudoku[j, i] == 0)
                        {
                            for (int num = 1; num < 10; num++)
                            {
                                if (isSafe(sudoku, num, i, j))
                                {
                                    if (*terminate == 1)
                                        return sudoku;

                                    sudoku[j, i] = num;                                  
                                    cells[i, j].Text = sudoku[j, i].ToString();

                                    if (stepbytstep)
                                    {
                                        cells[i, j].ForeColor = System.Drawing.Color.Red;         
                                        label.Text = "Number: "+num.ToString() + " Row: " + (i + 1).ToString() + " Column: " + (j + 1).ToString();
                                        wait(2000);
                                        cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                                    }

                                    break;
                                }

                            }
                        }
                        
                        
                    }
                }
            }

            return sudoku;
        }

        public bool isUsedInRow(int[,] sudoku, int row, int num)
        {
            for (int i = 0; i < sudoku.GetLength(1); i++)
            {
                if (sudoku[i, row]==num)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isUsedInColumn(int[,] sudoku, int column, int num)
        {
            for (int i = 0; i < sudoku.GetLength(1); i++)
            {
                if (sudoku[column, i]==num)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void printSudoku(int[,] sudoku)
        {
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    Console.Write(sudoku[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }

        public bool isSafe(int[,] sudoku, int num, int row, int column)
        {
            int columnoffset1 = 0;
            int columnoffset2 = 0;
            int rowoffset1 = 0;
            int rowoffset2 = 0;

            int[,] temp = new int[3, 3]
            {
            {0,0,0},
            {0,0,0},
            {0,0,0}
            };

            int[,] temp2 = getNumbers(sudoku, row, column, num);

            getOffsets(&columnoffset1, &columnoffset2, column);
            getOffsets(&rowoffset1, &rowoffset2, row);

            if (isUsedInRow(sudoku, row + rowoffset1, num))
            {
                for (int i = 0; i < 3; i++)
                {
                    temp[i, (row + rowoffset1) % 3] = -1;
                }
            }

            if (isUsedInRow(sudoku, row + rowoffset2, num))
            {
                for (int i = 0; i < 3; i++)
                {
                    temp[i, (row + rowoffset2) % 3] = -1;
                }
            }

            if (isUsedInColumn(sudoku, column + columnoffset1, num))
            {
                for (int i = 0; i < 3; i++)
                {
                    temp[(column + columnoffset1) % 3, i] = -1;
                }
            }

            if (isUsedInColumn(sudoku, column + columnoffset2, num))
            {
                for (int i = 0; i < 3; i++)
                {
                    temp[(column + columnoffset2) % 3, i] = -1;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (temp2[i, j] != 0)
                    {
                        temp[i, j] = temp2[i, j];

                    }
                }
            }

            int valid = 0;
            bool exists = false;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (temp[i, j] == num)
                    {
                        exists = true;
                        goto NextStep;
                    }

                    if (temp[i, j] == 0)
                    {
                        valid++;
                    }

                }
            }

        NextStep:

            if (exists)
            {
                return false;
            }

            if (valid == 1)
            {
                return true;
            }

            return false;
        }

        public int position(int num)
        {

            num = num + 1;

            if (num % 3 == 1)
                return 0;
            else if (num % 3 == 2)
                return 1;
            else
                return 2;

        }

        public int[,] getNumbers(int[,] sudoku, int column, int row, int num)
        {
            int startrow = row - row % 3;
            int startcolumn = column - column % 3;
            int[,] temp = new int[3, 3]
            {
            {0,0,0},
            {0,0,0},
            {0,0,0}
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = sudoku[i + startrow, j + startcolumn];
                }
            }

            return temp;
        }

        public bool isSolution(int[,] sudoku)
        {
            int length = sudoku.GetLength(0) - 1;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (sudoku[i, j]== 0)
                        return false;

                }
            }

            return true;
        }

        public void getOffsets(int* offset1, int* offset2, int a)
        {
            switch (position(a))
            {
                case 0:
                    *offset1 = 1;
                    *offset2 = 2;
                    break;
                case 1:
                    *offset1 = 1;
                    *offset2 = -1;
                    break;
                case 2:
                    *offset1 = -1;
                    *offset2 = -2;
                    break;
            }
        }

        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        public int[,] getArray()
        {
            return sudoku;
        }

        // Remove comments to allow input
        /*
        public int Value { get; set; }
        public bool IsLocked { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Clear()
        {
            this.Text = string.Empty;
            this.IsLocked = false;
        }
        */

    }

}

