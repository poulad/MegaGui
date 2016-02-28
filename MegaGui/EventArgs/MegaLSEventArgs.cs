using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaGui.MegaEvent
{
  class MegaLSEventArgs : System.EventArgs
  {
    public MegaLSFileType Type;
    public string Name;
    public long Size;
    public DateTime DateTime;

    public MegaLSEventArgs()
    {
      Size = 0;
    }

  }
}
