using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolver
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //grid[row][column]
        private readonly short[][] grid =
        {
            new short[] { 0,0,0,0,0,0,0,0,0 },//1
            new short[] { 0,0,0,0,0,0,0,0,0 },//2
            new short[] { 0,0,0,0,0,0,0,0,0 },//3
            new short[] { 0,0,0,0,0,0,0,0,0 },//4
            new short[] { 0,0,0,0,0,0,0,0,0 },//5
            new short[] { 0,0,0,0,0,0,0,0,0 },//6
            new short[] { 0,0,0,0,0,0,0,0,0 },//7
            new short[] { 0,0,0,0,0,0,0,0,0 },//8
            new short[] { 0,0,0,0,0,0,0,0,0 } //9
        };
        private readonly Button[][] bGrid = new Button[9][];
        private bool TryingToSolve = false;
        private readonly Thread solveThread;

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtonGrid();
            RefreshGrid();
            solveThread = new Thread(SolveThread);
            solveThread.Start();
        }

        #region Initializing
        private void InitializeButtonGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                bGrid[i] = new Button[9];
            }
            #region Don't look at this.
            #region Seriously.
            #region Don't.
            bGrid[0][0] = b00;
            bGrid[0][1] = b01;
            bGrid[0][2] = b02;
            bGrid[0][3] = b03;
            bGrid[0][4] = b04;
            bGrid[0][5] = b05;
            bGrid[0][6] = b06;
            bGrid[0][7] = b07;
            bGrid[0][8] = b08;
            bGrid[1][0] = b10;
            bGrid[1][1] = b11;
            bGrid[1][2] = b12;
            bGrid[1][3] = b13;
            bGrid[1][4] = b14;
            bGrid[1][5] = b15;
            bGrid[1][6] = b16;
            bGrid[1][7] = b17;
            bGrid[1][8] = b18;
            bGrid[2][0] = b20;
            bGrid[2][1] = b21;
            bGrid[2][2] = b22;
            bGrid[2][3] = b23;
            bGrid[2][4] = b24;
            bGrid[2][5] = b25;
            bGrid[2][6] = b26;
            bGrid[2][7] = b27;
            bGrid[2][8] = b28;
            bGrid[3][0] = b30;
            bGrid[3][1] = b31;
            bGrid[3][2] = b32;
            bGrid[3][3] = b33;
            bGrid[3][4] = b34;
            bGrid[3][5] = b35;
            bGrid[3][6] = b36;
            bGrid[3][7] = b37;
            bGrid[3][8] = b38;
            bGrid[4][0] = b40;
            bGrid[4][1] = b41;
            bGrid[4][2] = b42;
            bGrid[4][3] = b43;
            bGrid[4][4] = b44;
            bGrid[4][5] = b45;
            bGrid[4][6] = b46;
            bGrid[4][7] = b47;
            bGrid[4][8] = b48;
            bGrid[5][0] = b50;
            bGrid[5][1] = b51;
            bGrid[5][2] = b52;
            bGrid[5][3] = b53;
            bGrid[5][4] = b54;
            bGrid[5][5] = b55;
            bGrid[5][6] = b56;
            bGrid[5][7] = b57;
            bGrid[5][8] = b58;
            bGrid[6][0] = b60;
            bGrid[6][1] = b61;
            bGrid[6][2] = b62;
            bGrid[6][3] = b63;
            bGrid[6][4] = b64;
            bGrid[6][5] = b65;
            bGrid[6][6] = b66;
            bGrid[6][7] = b67;
            bGrid[6][8] = b68;
            bGrid[7][0] = b70;
            bGrid[7][1] = b71;
            bGrid[7][2] = b72;
            bGrid[7][3] = b73;
            bGrid[7][4] = b74;
            bGrid[7][5] = b75;
            bGrid[7][6] = b76;
            bGrid[7][7] = b77;
            bGrid[7][8] = b78;
            bGrid[8][0] = b80;
            bGrid[8][1] = b81;
            bGrid[8][2] = b82;
            bGrid[8][3] = b83;
            bGrid[8][4] = b84;
            bGrid[8][5] = b85;
            bGrid[8][6] = b86;
            bGrid[8][7] = b87;
            bGrid[8][8] = b88;
            #endregion
            #endregion
            #endregion
        }
        #endregion

        #region Testing
        private bool ExistsInRow(short row, short num)
        {
            return (grid[row].Contains(num));
        }

        private bool ExistsInColumn(short column, short num)
        {
            short[] f =
            {
                grid[0][column],
                grid[1][column],
                grid[2][column],
                grid[3][column],
                grid[4][column],
                grid[5][column],
                grid[6][column],
                grid[7][column],
                grid[8][column]
            };
            return f.Contains(num);
        }

        private bool ExistsInBox(Tuple<short, short> box, short num)
        {
            List<short> boxList = new List<short>();
            int rowStart = box.Item1 * 3;
            int rowEnd = (box.Item1 + 1) * 3 - 1;
            int columnStart = box.Item2 * 3;
            int columnEnd = (box.Item2 + 1) * 3 - 1;
            for (int r = rowStart; r <= rowEnd; r++)
            {
                for (int c = columnStart; c <= columnEnd; c++)
                {
                    boxList.Add(grid[r][c]);
                }
            }
            return boxList.Contains(num);
        }

        private bool CanBePlaced(short num, short row, short col)
        {
            if (num == 0) return true;
            if (ExistsInRow(row, num)) return false;
            if (ExistsInColumn(col, num)) return false;
            if (ExistsInBox(GetBox(row, col), num)) return false;
            return true;
        }
        #endregion

        #region Getting stuff
        private Tuple<short, short> GetBox(short row, short column)
        {
            short sRow = Convert.ToInt16(row / 3);
            short sColumn = Convert.ToInt16(column / 3);
            return new Tuple<short, short>(sRow, sColumn);
        }
        #endregion

        #region Command Methods
        public void Save_Command(object sender, RoutedEventArgs e)
        {
            TryingToSolve = !TryingToSolve;
        }
        #endregion

        #region Thread Methods
        private void SolveThread()
        {
            while (true)
            {
                while (TryingToSolve)
                {
                    for (short r = 0; r < 9; r++)
                    {
                        for (short c = 0; c < 9; c++)
                        {
                            if (grid[r][c] != 0) continue;
                            List<short> possible = GetPossibleNums(r, c);
                            if (possible.Count < 1)
                            {
                                MessageBox.Show("Impossible to solve.");
                                TryingToSolve = false;
                            }
                            else if (possible.Count == 1)
                            {
                                grid[r][c] = possible[0];
                            }
                            Application.Current.Dispatcher.Invoke(RefreshGrid);
                        }
                        TrySolvingRow(r);
                        Application.Current.Dispatcher.Invoke(RefreshGrid);
                        TrySolvingColumn(r);
                        Application.Current.Dispatcher.Invoke(RefreshGrid);
                    }
                    TrySolvingBoxes();
                    Application.Current.Dispatcher.Invoke(RefreshGrid);
                }
                Thread.Sleep(500);
            }
        }
        #endregion

        #region Solving Methods
        private List<short> GetPossibleNums(short row, short column)
        {
            List<short> answer = new List<short>();
            for (short i = 1; i <= 9; i++)
            {
                if (CanBePlaced(i, row, column)) answer.Add(i);
            }
            return answer;
        }

        private void TrySolvingRow(short row)
        {
            List<List<short>> possible = new List<List<short>>();
            for (short p = 0; p < 9; p++)
            {
                possible.Add(new List<short>());
            }
            for (short c = 0; c < 9; c++)
            {
                if (grid[row][c] != 0) continue;
                possible[c].AddRange(GetPossibleNums(row, c));
            }
            for (short i = 0; i < 9; i++)
            {
                if (possible.FindAll(x => x.Contains(i)).Count == 1)
                {
                    short col = Convert.ToInt16(possible.IndexOf(possible.Find(x => x.Contains(i))));
                    grid[row][col] = i;
                }
            }
        }

        private void TrySolvingColumn(short col)
        {
            List<List<short>> possible = new List<List<short>>();
            for (short p = 0; p < 9; p++)
            {
                possible.Add(new List<short>());
            }
            for (short r = 0; r < 9; r++)
            {
                if (grid[r][col] != 0) continue;
                possible[r].AddRange(GetPossibleNums(r, col));
            }
            for (short i = 0; i < 9; i++)
            {
                if (possible.FindAll(x => x.Contains(i)).Count == 1)
                {
                    short row = Convert.ToInt16(possible.IndexOf(possible.Find(x => x.Contains(i))));
                    grid[row][col] = i;
                }
            }
        }

        private void TrySolvingBoxes()
        {
            List<List<short>> possible = new List<List<short>>();
            for (short p = 0; p < 9; p++)
            {
                possible.Add(new List<short>());
            }
            for (short f = 0; f < 9; f++)
            {
                short row = Convert.ToInt16(f / 3);
                short col = Convert.ToInt16(f % 3);
                if (grid[row][col] != 0) continue;
                possible[f].AddRange(GetPossibleNums(row, col));
            }
            for (short i = 0; i < 9; i++)
            {
                if (possible.FindAll(x => x.Contains(i)).Count == 1)
                {
                    short row = Convert.ToInt16(possible.IndexOf(possible.Find(x => x.Contains(i))) / 3);
                    short col = Convert.ToInt16(possible.IndexOf(possible.Find(x => x.Contains(i))) % 3);
                    grid[row][col] = i;
                }
            }
        }
        #endregion

        #region Button Methods
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            if (Regex.IsMatch(name, @"^b\d\d$"))
            {
                short row = Convert.ToInt16(name.Substring(1).Remove(1));
                short column = Convert.ToInt16(name.Substring(2));
                HandleButton(row, column);
            }
        }

        private void HandleButton(short row, short column)
        {
            ChooseNumberDialog cnd = new ChooseNumberDialog(grid[row][column]);
            if (cnd.ShowDialog() == true)
            {
                if (CanBePlaced(cnd.Number, row, column))
                    grid[row][column] = cnd.Number;
                else MessageBox.Show("Ungültig");
            }
            RefreshGrid();
        }
        #endregion

        #region Display stuff
        private void RefreshGrid()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    bGrid[r][c].Content = grid[r][c] == 0 ? "" : grid[r][c].ToString();
                }
            }
        }
        #endregion

        #region Window stuff
        private void Window_Closed(object sender, EventArgs e)
        {
            solveThread.Abort();
        }
        #endregion
    }
}
