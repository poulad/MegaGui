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
      mega.megalsOutputEvent += mega_OnEvent;
      mega.MegaLS();
    }

    void mega_OnEvent(object sender, MegaLSEventArgs e)
    {
      Dispatcher.BeginInvoke(new Action(delegate
      {
        outputTextBox.Text += string.Format("\n{0}--{1}", e.Data, e.OperationType);
      }), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
    }

  }

}
