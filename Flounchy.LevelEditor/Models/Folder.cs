using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.LevelEditor.Models
{
  public class Item
  {
    public string Name { get; set; }
    public string Path { get; set; }
  }
  public class Folder
  {
    public string Name { get; set; }

    public List<Folder> Folders { get; set; }

    public List<Item> Images { get; set; }
  }
}
