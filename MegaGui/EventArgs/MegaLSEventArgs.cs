using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaGui.MegaEvent
{
  class MegaLSEventArgs : System.EventArgs
  {
    public MegaLSEventArgs()
    {
      OperationType = MegaOperation.LIST;
    }

    // Properties
    public readonly MegaOperation OperationType;
    public string Data;
  }
}
