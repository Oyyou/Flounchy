using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Flounchy.LevelEditor.Commands;

namespace Flounchy.LevelEditor.ViewModels
{
  public class ViewModel1 : ViewModel
  {

    #region Commands
    public ICommand Command1 { get; private set; }
    #endregion

    public ViewModel1()
    {
      InitializeCommands();
    }

    private void InitializeCommands()
    {
      Command1 = new Command1(this);
    }
  }
}
