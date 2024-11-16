using PROG_POE_PART_2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PROG_POE_PART_2.UserControls
{
    public partial class ServiceRequestControl : UserControl
    {
        private BinarySearchTree serviceRequestTree = ReportIssueControl.ServiceRequestTree;

        public ServiceRequestControl()
        {
            InitializeComponent();
            PopulateListView();
        }

        private void PopulateListView()
        {
            var requests = GetAllRequests(serviceRequestTree.Root);
            listViewServiceRequests.ItemsSource = requests;
        }

        private List<ServiceRequest> GetAllRequests(ServiceRequestNode node)
        {
            var requests = new List<ServiceRequest>();
            if (node != null)
            {
                requests.AddRange(GetAllRequests(node.Left));
                requests.Add(node.Data);
                requests.AddRange(GetAllRequests(node.Right));
            }
            return requests;
        }
        private void btnFilter_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedStatus = (cmbFilterStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedStatus == "All")
            {
                PopulateListView();
            }
            else
            {
                var requests = GetAllRequests(serviceRequestTree.Root).Where(r => r.Status == selectedStatus).ToList();
                listViewServiceRequests.ItemsSource = requests;
            }
        }

        private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtSearchId.Text, out int id))
            {
                var result = serviceRequestTree.Search(id);
                if (result != null)
                {
                    listViewServiceRequests.ItemsSource = new List<ServiceRequest> { result };
                }
                else
                {
                    MessageBox.Show("Service request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
