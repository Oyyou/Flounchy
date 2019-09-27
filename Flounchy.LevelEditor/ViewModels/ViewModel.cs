using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.LevelEditor.ViewModels
{
  public class ViewModel : INotifyPropertyChanged
  {
    public virtual event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChange(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;

      if (handler != null)
      {
        try
        {
          handler(this, new PropertyChangedEventArgs(propertyName));
        }
        catch
        {
          Console.WriteLine("Yup, we'll just pretend this never happened");
        }
      }
    }
  }
}
