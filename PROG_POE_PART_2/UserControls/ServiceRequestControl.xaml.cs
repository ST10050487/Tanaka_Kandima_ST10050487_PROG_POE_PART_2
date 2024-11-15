using PROG_POE_PART_2.Classes;
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

namespace PROG_POE_PART_2.UserControls
{
    public partial class ServiceRequestControl : UserControl
    {
        public List<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

        public ServiceRequestControl()
        {
            InitializeComponent();
            LoadDummyData();
            PopulateListView(ServiceRequests);
        }

        private void LoadDummyData()
        {
            ServiceRequests.Add(new ServiceRequest(1, "Printer Issue", "Fix printer", "Pending", DateTime.Now.AddHours(-5), null));
            ServiceRequests.Add(new ServiceRequest(2, "Workstation Setup", "Setup workstation", "In Progress", DateTime.Now.AddHours(-3), null));
            ServiceRequests.Add(new ServiceRequest(3, "Software Update", "Update software", "Completed", DateTime.Now.AddHours(-8), DateTime.Now.AddHours(-1)));
        }

        private void PopulateListView(List<ServiceRequest> requests)
        {
            listViewServiceRequests.ItemsSource = requests;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtSearchId.Text, out int id))
            {
                var result = ServiceRequests.Where(r => r.Id == id).ToList();
                PopulateListView(result);
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var selectedStatus = (cmbFilterStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedStatus == "All")
            {
                PopulateListView(ServiceRequests);
            }
            else
            {
                var filteredRequests = ServiceRequests.Where(r => r.Status == selectedStatus).ToList();
                PopulateListView(filteredRequests);
            }
        }
    }
}
