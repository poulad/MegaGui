using System;
using System.Diagnostics;

using MegaGui.MegaEvent;

namespace MegaGui
{

  public enum MegaOperation
  {
    LIST, COPY, QUOTA, MKDIR, REMOVE, PUT, GET, DOWNLOAD, MOUNT
  }

  class MegaTools
  {
    private Process _proc;
    public delegate void MegaEventHandler(object sender, MegaLSEventArgs args);
    public event MegaEventHandler megalsOutputEvent;
    public event MegaEventHandler megalsErrorEvent;
    private MegaLSEventArgs _eArgs;
    private const string _megalsArgs = "--long --human";


    public MegaTools()
    {
      _proc = new Process();
      _proc.StartInfo.CreateNoWindow = true;
      _proc.StartInfo.UseShellExecute = false;
      _proc.StartInfo.RedirectStandardInput = true;
      _proc.StartInfo.RedirectStandardOutput = true;
      _proc.StartInfo.RedirectStandardError = true;
    }

    public bool MegaLS()
    {/*
      try
      {
        if(_proc.HasExited == false)
        {
          //return false;
        }
      }
      catch(Exception excp)
      {
        Console.WriteLine("megals command did not execute.\n" + excp.Message);
        //return false;
      }*/

      _proc.StartInfo.FileName = "megals";
      _proc.StartInfo.Arguments = _megalsArgs;

      _proc.OutputDataReceived += MegaLS_OutputDataReceived;
      _proc.ErrorDataReceived += MegaLS_ErrorDataReceived;

      bool hasStarted = _proc.Start();
      _proc.BeginOutputReadLine();
      _proc.BeginErrorReadLine();

      return hasStarted;
    }

    private void MegaLS_OutputDataReceived(object o, DataReceivedEventArgs e)
    {
      _eArgs = new MegaLSEventArgs();
      _eArgs.Data = e.Data;
      megalsOutputEvent(this, _eArgs);
    }

    private void MegaLS_ErrorDataReceived(object o, DataReceivedEventArgs e)
    {
      _eArgs = new MegaLSEventArgs();
      _eArgs.Data = e.Data;
      megalsErrorEvent(this, _eArgs);
    }


  }
}
