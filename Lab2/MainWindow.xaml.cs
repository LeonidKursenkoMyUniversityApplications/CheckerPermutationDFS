using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;
using DepthFirstSearch.Task;

namespace DepthFirstSearch
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CheckerField StartField { set; get; }
        public CheckerField FinalField { set; get; }
        private Dfs _dfs;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dfs = new Dfs();
            _dfs.Run();
            StartField = _dfs.StartField;
            CheckerGrid.DataContext = StartField;
            FinalField = _dfs.FinalField;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartField.InitPath();
            _dfs.Path = StartField.Path;
            _dfs.InitPath();
            _dfs.Step();
            StartField.InitStartFieldCells();
            MessageBox.Show("Path was founded. Length=" + _dfs.Path.Count);
        }
        
        private void Pause()
        {
            CheckerGrid.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate() { CheckerGrid.UpdateLayout(); }));
            Thread.Sleep(300);
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (var move in _dfs.Path)
            {
                _dfs.PathHistory.Add(move.ToString());
                move.Transfer();

                CheckerGrid.DataContext = null;
                CheckerGrid.DataContext = StartField;
                ResultRichTextBox.Document.Blocks.Clear();
                ResultRichTextBox.Document.Blocks.Add(new Paragraph(new Run(_dfs.GetPath)));
                Pause();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            StartField.InitStartFieldCells();
            _dfs.StartField = StartField;
            _dfs.PathHistory.Clear();
            CheckerGrid.DataContext = null;
            CheckerGrid.DataContext = StartField;
            ResultRichTextBox.Document.Blocks.Clear();
        }
    }
}
