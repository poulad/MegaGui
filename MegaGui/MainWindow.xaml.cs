using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MegaGui.MegaEvent;

namespace MegaGui
{

  public partial class MainWindow : Window
  {
    MegaTools mega;

    public MainWindow()
    {
      InitializeComponent();
      mega = new MegaTools();
    }

    private void button_Click(object sender, RoutedEventArgs e)
    {
      mega.OnFileInfoReady += mega_OnFileInfoReady;
      mega.megalsErrorReceivedEvent += mega_megalsErrorReceivedEvent;
      mega.MegaLS();
    }

    void mega_megalsErrorReceivedEvent(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
      Dispatcher.BeginInvoke(new Action(delegate
      {
        errorTextBox.Text += "\r\n" + e.Data;
      }), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
    }

    void mega_OnFileInfoReady(object sender, MegaLSEventArgs e)
    {
      Dispatcher.BeginInvoke(new Action(delegate
      {
        TreeViewItem child = new TreeViewItem();
        child.Header = e.Name;
        treeView.Items.Add(child);
        outputTextBox.Text += string.Format("\r\n-->{0}--{1}<--",e.Name, e.DateTime.ToString());
      }), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
    }

     

  }

}
