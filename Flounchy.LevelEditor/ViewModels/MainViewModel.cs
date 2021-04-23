using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Flounchy.LevelEditor.Commands;
using Flounchy.LevelEditor.Models;

namespace Flounchy.LevelEditor.ViewModels
{
  public class MainViewModel : ViewModel
  {
    #region Commands
    public ICommand Command1 { get; private set; }
    #endregion

    private List<Folder> _folders = new List<Folder>();

    public List<Folder> Folders
    {
      get { return _folders; }
      set
      {
        _folders = value;
        OnPropertyChange("Folders");
      }
    }

    public MainViewModel()
    {
      InitializeCommands();

      var content = Directory.GetCurrentDirectory() + "\\Content";
      var folders = new List<Folder>();
      foreach (var directory in Directory.GetDirectories(content))
      {
        folders.Add(GetFolder(directory));
      }

      Folders = folders;
    }

    private Folder GetFolder(string directory)
    {
      return new Folder()
      {
        Name = Path.GetFileName(directory),
        Folders = Directory.GetDirectories(directory).Select(c => GetFolder(c)).ToList(),
        Images = Directory.GetFiles(directory, "*.png").Select(c => new Item() { Name = Path.GetFileName(c), Path = c, }).ToList(),
      };
    }

    private void InitializeCommands()
    {
      Command1 = new Command1(this);
    }
  }
}
