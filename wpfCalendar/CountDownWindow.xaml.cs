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
using System.Windows.Shapes;

namespace wpfCalendar
{
    /// <summary>
    /// Interaction logic for CountDownWindow.xaml
    /// </summary>
    public partial class CountDownWindow : Window
    {
        public StartPage StartPage;
        public CountdownPage CountdownPage;
        public SettingsPage SettingsPage;

        private void initialize()
        {
            InitializeComponent();

            //create 3 pages within this window to manage them
            StartPage = new StartPage(WindowFrame, this);
            SettingsPage = new SettingsPage(this);

            //load from settings
            rectBackground.Opacity = Properties.Settings.Default.BackgroundOpacity;
            WindowFrame.Opacity = Properties.Settings.Default.ForegroundOpacity;

            if (Properties.Settings.Default.ImageSource != "")
            {
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(Properties.Settings.Default.ImageSource)));
                    brush.Stretch = Stretch.UniformToFill;
                    rectBackground.Fill = brush;                    
                } catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message + "\nPlease add the image again");
                    Properties.Settings.Default.ImageSource = "";
                    Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Constructor for default windows
        /// </summary>
        public CountDownWindow()
        {
            initialize();
            WindowFrame.Navigate(StartPage);
        }

        /// <summary>
        /// Constructor for existing DateTimes
        /// </summary>
        public CountDownWindow(DateTime endDate, Point location)
        {
            initialize();
            this.Left = location.X; //set x pos
            this.Top = location.Y; //set y pos
            NavigateToCountdownPage(new CountDownManager(endDate), false); //removeLastPage = false
        }

        public void NavigateToSettingsPage()
        {
            WindowFrame.Navigate(SettingsPage);
        }

        //does not create instance, use already existing instance
        public void NavigateToCountdownPage()
        {
            WindowFrame.Navigate(CountdownPage);
        }

        //creates an instance of the countdown page and make it run
        public void NavigateToCountdownPage(CountDownManager countdownManager, bool removeLastPage = true)
        {
            CountdownPage = new CountdownPage(countdownManager, this); //create one countdown page, store into global var

            //if have to remove last page accessed (start page), remove it
            if (removeLastPage) WindowFrame.Navigated += WindowFrame_Navigated;
            WindowFrame.Navigate(CountdownPage);      
        }

        private void WindowFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            WindowFrame.RemoveBackEntry();
            WindowFrame.Navigated -= WindowFrame_Navigated;
        }

        //add new window when button is pressed
        public void AddNewWindow()
        {
            CountDownWindow newWindow = new CountDownWindow();
            newWindow.Show();
            newWindow.Owner = this.Owner;
        }

        public void SaveWindowInfo()
        {
            if (CountdownPage != null)
            {
                CountdownPage.CountDownManager.SaveDate();
                WidgetFileManager.saveLocationToFile(this.PointToScreen(new Point(0,0)));
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
