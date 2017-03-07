using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfCalendar
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private CountDownWindow parentWindow;
        private string imageSource = "";

        public SettingsPage(CountDownWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.NavigateToCountdownPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            sldBackgroundOpacity.Value = parentWindow.rectBackground.Opacity;
            sldForegroundOpacity.Value = parentWindow.WindowFrame.Opacity;
        }

        private void sldBackgroundOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (parentWindow != null) parentWindow.rectBackground.Opacity = sldBackgroundOpacity.Value;
        }

        private void sldForegroundOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (parentWindow != null) parentWindow.WindowFrame.Opacity = sldForegroundOpacity.Value;
        }
        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //save the background and foreground settings
            Properties.Settings.Default.BackgroundOpacity = parentWindow.rectBackground.Opacity;
            Properties.Settings.Default.ForegroundOpacity = parentWindow.WindowFrame.Opacity;
            if (txtImageSource.Text != "")
            {
                copyAndSetImage(txtImageSource.Text);
                Properties.Settings.Default.ImageSource = imageSource;
            }
            if (datePicker.SelectedDate != null)
            {
                DateTime endDate;
                DateTime.TryParse(datePicker.Text, out endDate);

                if (Math.Round((endDate - DateTime.Now).TotalDays) < 0)
                {
                    MessageBox.Show("Please Enter a Date After " + DateTime.Now.ToString("dd MMMM yy"));
                    return;
                }

                parentWindow.CountdownPage.CountDownManager.ChangeDate(endDate);
            }
            Properties.Settings.Default.Save();
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {      
                txtImageSource.Text = openFileDialog.FileName;
            }
        }

        private void copyAndSetImage(string filename)
        {
            string src = WidgetFileManager.CopyFileToDocuments(filename);

            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(src)));
            brush.Stretch = Stretch.UniformToFill;
            parentWindow.rectBackground.Fill = brush;

            imageSource = src;
        }

        private void resetImageSourceText()
        {
            txtImageSource.Text = "";
        }
    }
}
