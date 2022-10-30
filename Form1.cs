using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    unsafe public partial class Form1 : Form
    {
                   
        int* pointer;
        int[,] array = new int[9, 9];
        int[,] cleararray = new int[9, 9];
        Sudoku cell = new Sudoku();
        Sudoku[,] cells = new Sudoku[9, 9];
             
        public Form1()
        {
            InitializeComponent();
            int terminator = 0;
            pointer = &terminator;

            WindowState = FormWindowState.Maximized;

            array = cell.getArray();

            copyArray(array, cleararray);

      
            panel4.Location = new Point(
            this.ClientSize.Width /2 - panel1.Size.Width-100 ,
            this.ClientSize.Height / 2 - panel1.Size.Height+320 );          

            label3.Location = new Point(this.panel3.Width / 2 - label3.Size.Width / 2,
            this.panel3.Height - label3.Size.Height-115);

            createCells();

        }
      
        public void createCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                  
                    cells[i, j] = new Sudoku();
                    cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 20);
                    cells[i, j].Size = new Size(panel1.Width / 9, panel1.Height / 9);
                    cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                    cells[i, j].Location = new Point(i * panel1.Width / 9, j * panel1.Height / 9);
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    cells[i, j].FlatStyle = FlatStyle.Flat;
                    cells[i, j].FlatAppearance.BorderColor = Color.Black;
                    cells[i, j].Text = array[j, i].ToString();


                    //Remove comments to allow number input in the table
                    //cells[i, j].X = i;
                    //cells[i, j].Y = j;                   
                    //cells[i, j].KeyPress += cell_keyPressed;

                    panel1.Controls.Add(cells[i, j]);

                }
            }
        }
       

        private void updateCells(Sudoku[,] cells , int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {

                    array[j, i] = Int32.Parse(cells[i, j].Text);
                    

                }
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
                               
            var timer = new Stopwatch();          
            bool stepbystep=false;
            button1.Enabled = false;
            button2.Enabled = false;
            *pointer = 0;
            Console.WriteLine(*pointer);
            timer.Start();          
            cell.solveSudoku(array, cells, pointer, label2, stepbystep);
            timer.Stop();
            label2.Text = "Time Elapsed: "+timer.ElapsedMilliseconds.ToString() + " ms";
            label2.Location = new Point(this.panel3.Width / 2 - label2.Size.Width / 2,
            this.panel3.Height / 2 - label2.Size.Height / 2);
            
        }
        private void button2_Click(object sender, EventArgs e)
        {

            bool stepbystep = true;
            button1.Enabled=false;
            button2.Enabled = false;
            *pointer = 0;
            Console.WriteLine(*pointer);
            label2.Text = "Number " + 3 + " Row: " + 2 + " Column: " + 2.ToString();
            label2.Location = new Point(this.panel3.Width / 2 - label2.Size.Width / 2,
            this.panel3.Height / 2 - label2.Size.Height / 2);
            
            cell.solveSudoku(array, cells, pointer,label2, stepbystep);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            *pointer = 1;
            button1.Enabled = true;
            button2.Enabled = true;
            label2.Text = "";

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {

                    cells[i, j].Text = cleararray[j, i].ToString();

                }        
            }
        
            copyArray(cleararray, array);
        }

        public void copyArray(int[,] array1, int[,] array2)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {

                  array2[i,j]=array1[i,j];  

                }
            }

        }

        //Remove comments to allow number input in the table + (line 350 in class1.cs)
        /*
        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            var cell = sender as Sudoku;
         
            if (cell.IsLocked)
                return;

            int value;
           
            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                
                if (value == 0)
                    cell.Clear();
                else
                    cell.Text = value.ToString();

                cell.ForeColor = SystemColors.ControlDarkDark;
            }
           
        }*/
    }
}
