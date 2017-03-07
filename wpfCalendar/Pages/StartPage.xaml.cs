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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        CountDownWindow parentWindow;
        private Frame windowFrame;

        public StartPage(Frame windowFrame, CountDownWindow parentWindow)
        {
            InitializeComponent();
            this.windowFrame = windowFrame;
            this.parentWindow = parentWindow;
        }

        //reads the date, saves to window
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            DateTime endDate;

            if (!DateTime.TryParse(datePicker.Text, out endDate))
            {
                MessageBox.Show("Please Enter a Valid Date.");
                return;
            }
            else if (Math.Round((endDate - DateTime.Now).TotalDays) < 0)
            {
                MessageBox.Show("Please Enter a Date After " + DateTime.Now.ToString("dd MMMM yy"));
                return;
            }

            parentWindow.NavigateToCountdownPage(new CountDownManager(endDate));           
        }
    }
}
