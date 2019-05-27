using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SudokuSolver
{
    /// <summary>
    /// Interaktionslogik für ChooseNumberDialog.xaml
    /// </summary>
    public partial class ChooseNumberDialog : Window
    {
        public short Number { get; set; }
        public ChooseNumberDialog(short currentNumber)
        {
            InitializeComponent();
            Number = currentNumber;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            if (Regex.IsMatch(name, @"^b\d$"))
            {
                Number = Convert.ToInt16(name.Substring(1));
                DialogResult = true;
            }
            else if (name == "cancel")
            {
                DialogResult = false;
            }
        }
    }
}
