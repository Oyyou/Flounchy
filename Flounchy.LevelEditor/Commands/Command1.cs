using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Flounchy.LevelEditor.ViewModels;

namespace Flounchy.LevelEditor.Commands
{
  public class Command1 : ICommand
  {
    private ViewModel1 _viewModel;

    public Command1(ViewModel1 viewModel)
    {
      _viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event System.EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {

    }
  }
}
