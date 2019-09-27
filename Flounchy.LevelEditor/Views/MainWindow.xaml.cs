using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Flounchy.LevelEditor.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      var xCount = 1280 / 40;
      var yCount = 720 / 40;

      var brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

      for (int y = 0; y < yCount; y++)
      {
        for (int x = 0; x < xCount; x++)
        {
          var grid = new Grid()
          {
            Width = 40,
            Height = 40,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(x * 40, y * 40, 0, 0),
            Background = new SolidColorBrush(Colors.CornflowerBlue),
          };

          grid.MouseEnter += Grid_MouseEnter;
          grid.MouseLeave += Grid_MouseLeave;

          //if (brush.Color.R < 255)
          //  brush = new SolidColorBrush(Color.FromRgb((byte)(brush.Color.R + 1), brush.Color.G, brush.Color.B));
          //else if (brush.Color.G < 255)
          //  brush = new SolidColorBrush(Color.FromRgb(brush.Color.R, (byte)(brush.Color.G + 1), brush.Color.B));
          //else if (brush.Color.B < 255)
          //  brush = new SolidColorBrush(Color.FromRgb(brush.Color.R, brush.Color.G, (byte)(brush.Color.B + 1)));

          GameArea.Children.Add(grid);
        }
      }
    }

    private void Grid_MouseEnter(object sender, MouseEventArgs e)
    {
      var grid = sender as Grid;
    }

    private void Grid_MouseLeave(object sender, MouseEventArgs e)
    {
      var grid = sender as Grid;
    }
  }
}
