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

namespace wpfCalendar
{
    /// <summary>
    /// Interaction logic for CountdownPage.xaml
    /// </summary>
    public partial class CountdownPage : Page
    {
        private CountDownWindow parentWindow;
        private CountDownManager countDownManager;

        public CountDownManager CountDownManager
        {
            get
            {
                return countDownManager;
            }
        }

        public CountdownPage(CountDownManager countDownManager, CountDownWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.countDownManager = countDownManager;
            Update();

            //initially hide buttons
            showButtons(false);
        }

        public void Update()
        {
            if (countDownManager != null)
            {
                txtDaysLeft.Text = Convert.ToString(countDownManager.DaysLeft);
                txtHr.Text = Convert.ToString(countDownManager.HoursLeft);
                txtMin.Text = Convert.ToString(countDownManager.MinutesLeft);
                txtSec.Text = Convert.ToString(countDownManager.SecondsLeft);
                txtMessage.Text = string.Format("to " + countDownManager.EndDate.ToString("dd MMMM"));
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.AddNewWindow();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgResult = MessageBox.Show("Are you sure to delete this?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msgResult == MessageBoxResult.Yes)
            {
                parentWindow.Close();
                //check if is last window
                if (parentWindow.Owner.OwnedWindows.Count == 0)
                {
                    parentWindow.Owner.Close();
                }
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.NavigateToSettingsPage();
        }

        private void Page_MouseEnter(object sender, MouseEventArgs e)
        {
            //show buttons
            showButtons(true);
        }

        private void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            showButtons(false);
        }

        private void showButtons(bool show)
        {
            Visibility visibility = show ? Visibility.Visible : Visibility.Hidden;

            btnAdd.Visibility = visibility;
            btnDelete.Visibility = visibility;
            btnSettings.Visibility = visibility;
        }
    }
}
