
using PROG_POE_PART_2.UserControls;
using PROG_POE_PART_2.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace PROG_POE_PART_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Creating instances of the user controls
            var homeScreenControl = new HomeScreenUserControl();
            var eventsControl = new EventsAndAnnouncementsUserControl(homeScreenControl);

            // Assuming you have a placeholder in your XAML for these controls (like a Grid or StackPanel)
            var mainContent = this.FindName("MainContent") as Panel;
            if (mainContent != null)
            {
                mainContent.Children.Add(homeScreenControl);
                mainContent.Children.Add(eventsControl);
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Setting tooltip visibility to visible
            if (sender is ListViewItem item)
            {
                ToolTip tt = item.ToolTip as ToolTip;
                if (tt != null)
                {
                    tt.IsOpen = true;
                }
            }
        }
        //A method to close the application
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // A method to navigate to the EventsAndAnnouncements window
        private void NavigateToEventsAndAnnouncements(object sender, RoutedEventArgs e)
        {
            EventsAndAnnouncements eventsAndAnnouncements = new EventsAndAnnouncements();
            this.Close();
            eventsAndAnnouncements.Show();
        }
        // A method to navigate to the HomeScreen window
        private void NavigateToHomeScreen(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        // A method to navigate to the ReportIssue window
        private void NavigateToReportIssue(object sender, RoutedEventArgs e)
        {
            ReportIssue reportIssue = new ReportIssue();
            this.Close();
            reportIssue.Show();
        }
        // A method to navigate to the Community window
        private void NavigateToCommunity(object sender, RoutedEventArgs e)
        {
            Community community = new Community();
            this.Close();
            community.Show();
        }
    }
}

