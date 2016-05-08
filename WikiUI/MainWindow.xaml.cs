using System;
using System.Windows;

namespace WikiUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var onerror = (Action<string>)(message =>
            {
                if (this.Dispatcher.CheckAccess())
                {
                    MessageBox.Show(this, message);
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(this, message);
                    }));
                }
            });
            var mvm = new MainViewModel(onerror);
            DataContext = mvm;
        }
    }
}
