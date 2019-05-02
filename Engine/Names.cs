using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
  public static class Names
  {
    private static List<string> _values = new List<string>()
    {
      "Jimeh",
      "Flint",
      "Jeoff",
      "Sandlers",
      "Floin",
    };

    public static string GenerateName()
    {
      var random = new Random();

      return _values[random.Next(0, _values.Count)];
    }
  }
}
