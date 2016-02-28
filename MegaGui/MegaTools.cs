using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

using MegaGui.MegaEvent;

namespace MegaGui
{

  public enum MegaOperation
  {
    LIST, COPY, QUOTA, MKDIR, REMOVE, PUT, GET, DOWNLOAD, MOUNT, NOTHING = -1
  }

  public enum MegaLSFileType
  {
    FILE = 0, DIR = 1, ROOT = 2, INBOX = 3, TRASH = 4, CONTACTS = 9
  }

  class MegaTools
  {
    private Process _proc;
    private MegaOperation _currentOperation;

    public delegate void MegaEventHandler(object sender, MegaLSEventArgs args);
    public event MegaEventHandler OnFileInfoReady;
    public event DataReceivedEventHandler megalsErrorReceivedEvent;
    private MegaLSEventArgs _eArgs;

    private const string _megalsArgs = "--long";
    private Regex _megalsRegex;
    private Regex _whitespaceRegex;


    public MegaTools()
    {
      _proc = new Process();
      _proc.StartInfo.CreateNoWindow = true;
      _proc.StartInfo.UseShellExecute = false;
      _proc.StartInfo.RedirectStandardInput = true;
      _proc.StartInfo.RedirectStandardOutput = true;
      _proc.StartInfo.RedirectStandardError = true;
      _proc.EnableRaisingEvents = true;

      _proc.Exited += _proc_Exited;

      _currentOperation = MegaOperation.NOTHING;

      _whitespaceRegex = new Regex("\\s+");
      _megalsRegex = new Regex
        (@"\w+\s+\w+\s+(?<type>\d)\s+(?<size>\d+|-)\s+(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}\:\d{2}\:\d{2})\s+(?<name>.*)",
        RegexOptions.IgnoreCase);
    }

    void _proc_Exited(object sender, EventArgs e)
    {
      _currentOperation = MegaOperation.NOTHING;
    }

    public bool MegaLS()
    {
      if (_currentOperation != MegaOperation.NOTHING)
      {
        return false;
      }
      else
      {
        _currentOperation = MegaOperation.LIST;
      }

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
      if (e.Data == null || !_megalsRegex.IsMatch(e.Data))
      {
        return;
      }

      _eArgs = new MegaLSEventArgs();
      GroupCollection captured = _megalsRegex.Match(e.Data).Groups;
      _eArgs.Type = (MegaLSFileType)Convert.ToInt32(captured["type"].Value);
      if (captured["size"].Value != "-")
      {
        _eArgs.Size = Convert.ToInt64(captured["size"].Value);
      }
      _eArgs.DateTime = new DateTime(
        Convert.ToInt32(captured["date"].Value.Substring(0, 4)),
        Convert.ToInt32(captured["date"].Value.Substring(5, 2)),
        Convert.ToInt32(captured["date"].Value.Substring(8)),
        Convert.ToInt32(captured["time"].Value.Substring(0, 2)),
        Convert.ToInt32(captured["time"].Value.Substring(3, 2)),
        Convert.ToInt32(captured["time"].Value.Substring(6))
        );
      _eArgs.Name = captured["name"].Value;

      OnFileInfoReady(this, _eArgs);
    }

    private void MegaLS_ErrorDataReceived(object o, DataReceivedEventArgs e)
    {
      megalsErrorReceivedEvent(this, e);
    }


  }
}
