using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Timers;

namespace wpfCalendar
{
    /// <summary>
    /// Interaction logic for CommandWindow.xaml
    /// </summary>
    public partial class CommandWindow : Window
    {
        //centralized timer
        Timer timer = new Timer();

        public CommandWindow()
        {
            InitializeComponent();
            WidgetFileManager.CreateDirectoryAndFiles();
            this.Loaded += CommandWindow_Loaded;
            this.Closing += CommandWindow_Closing;
        }

        private void CommandWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            //save all here, clear before saving
            WidgetFileManager.ClearFileContents();
            foreach (Window ownedWin in OwnedWindows)
            {
                if (ownedWin is CountDownWindow)
                {
                    ((CountDownWindow)ownedWin).SaveWindowInfo();
                }
            }
        }

        private void CommandWindow_Loaded(object sender, RoutedEventArgs e)
        {
            openWindows();

            timer.Interval = 1000; //1 sec updates
            timer.Elapsed += (s, ev) => 
            {
                this.Dispatcher.Invoke(() =>
                {
                    update();
                });
            };
            timer.Start();
        }

        private void update()
        {
            foreach (Window ownedWin in OwnedWindows)
            {
                if (ownedWin is CountDownWindow)
                {
                    if (((CountDownWindow)ownedWin).CountdownPage != null)
                    {
                        ((CountDownWindow)ownedWin).CountdownPage.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Determine which windows to open
        /// </summary>
        private void openWindows()
        {
            //if the date file is empty, make new window
            if (WidgetFileManager.IsFileEmpty())
            {
                CountDownWindow countDownWindow = new CountDownWindow();
                countDownWindow.Owner = this;
                countDownWindow.Show();
            } else
            {
                string[] dates = WidgetFileManager.GetDatesFromFile();
                string[] locations = WidgetFileManager.GetLocationsFromFile();

                DateTime tempDate;
                for (int i = 0; i < dates.Length; i++)
                {
                    DateTime.TryParse(dates[i], out tempDate);       
                    CountDownWindow countDownWindow = new CountDownWindow(tempDate, Point.Parse(locations[i]));
                    countDownWindow.Owner = this;
                    countDownWindow.Show();
                }
            }
        }
    }
}
