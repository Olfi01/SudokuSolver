using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    internal static class Extensions
    {
        public static void CopyTo(this short[][] from, short[][] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                for (int j = 0; j < from[i].Length; j++)
                {
                    to[i][j] = from[i][j];
                }
            }
        }
    }
}
