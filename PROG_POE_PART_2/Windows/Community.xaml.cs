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

namespace PROG_POE_PART_2.Windows
{
    /// <summary>
    /// Interaction logic for Community.xaml
    /// </summary>
    public partial class Community : Window
    {
        public Community()
        {
            InitializeComponent();
        }
        // A method to move the window
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
        //A method to display the tooltip
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
        //Closing the application
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //Navigating to the EventsAndAnnouncements window
        private void NavigateToEventsAndAnnouncements(object sender, RoutedEventArgs e)
        {
            EventsAndAnnouncements eventsAndAnnouncements = new EventsAndAnnouncements();
            this.Close();
            eventsAndAnnouncements.Show();
        }
        //A method to navigate to the HomeScreen window
        private void NavigateToHomeScreen(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        //A method to navigate to the ReportIssue window
        private void NavigateToReportIssue(object sender, RoutedEventArgs e)
        {
            ReportIssue reportIssue = new ReportIssue();
            this.Close();
            reportIssue.Show();
        }
        //A method to navigate to the Community window
        private void NavigateToCommunity(object sender, RoutedEventArgs e)
        {
            Community community = new Community();
            this.Close();
            community.Show();
        }
        //A method to navigate to the ServiceRequests window
        private void NavigateToServiceRequests(object sender, RoutedEventArgs e)
        {
            ServiceRequests serviceRequests = new ServiceRequests();
            this.Close();
            serviceRequests.Show();
        }

    }
}
