using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KineticEnergy
{
    /// <summary>
    /// Group 1 Assignment 6
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ViewModel VM = ViewModel.Instance;
        readonly BallManager BM = BallManager.Instance;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        private void BallSelected(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            BM.SelectBall(listBox.SelectedIndex);
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]").IsMatch(e.Text);
        }
    }
}
