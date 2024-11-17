using PROG_POE_PART_2.Classes; // Importing the necessary classes from the PROG_POE_PART_2 namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace PROG_POE_PART_2.UserControls
{
    // UserControl for handling Service Request functionality
    public partial class ServiceRequestControl : UserControl
    {
        //creating a private BinarySearchTree object
        private BinarySearchTree serviceRequestTree = ReportIssueControl.ServiceRequestTree; 

        public ServiceRequestControl()
        {
            // Initializing the user control
            InitializeComponent();
            // Populating the list view with service requests
            PopulateListView(); 
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // Method to populate the ListView with all the service requests from the binary search tree
        private void PopulateListView()
        {
            // Getting all service requests from the tree
            var requests = GetAllRequests(serviceRequestTree.Root);
            // Assigning icons to the service requests
            requests.ForEach(AssignIcons); 
            listViewServiceRequests.ItemsSource = requests; 
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // Method to retrieve all service requests from the binary search tree using in-order traversal
        private List<ServiceRequest> GetAllRequests(ServiceRequestNode node)
        {
            // List to store the service requests
            var requests = new List<ServiceRequest>();
            // Stack to store the nodes
            var stack = new Stack<ServiceRequestNode>(); 
            var currentNode = node;

            // Traversing while there are nodes to visit
            while (currentNode != null || stack.Any()) 
            {
                while (currentNode != null) 
                {
                    // Pushing the current node to the stack
                    stack.Push(currentNode);
                    // Moving to the left child
                    currentNode = currentNode.Left; 
                }
                // Popping the node from the stack
                currentNode = stack.Pop();
                // Adding the service request to the list
                requests.Add(currentNode.Data); 
                currentNode = currentNode.Right; 
            }

            return requests; 
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // Method to assign icons to service requests
        private void AssignIcons(ServiceRequest request)
        {
            // Assigning the video icon to all videos associated with the service request
            request.Videos = request.Videos.Select(v => "pack://application:,,,/Images/VideoIcon.png").ToList();
        }

        // Event handler for the Search button click
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Try to parse the search ID input as an integer
            if (int.TryParse(txtSearchId.Text, out int id))
            {
                // Searching for the service request with the entered ID
                var result = serviceRequestTree.Search(id); 
                if (result != null) 
                {
                    // Assign icons to the service request
                    AssignIcons(result);
                    // Displaying the service request in the ListView
                    listViewServiceRequests.ItemsSource = new List<ServiceRequest> { result }; 
                }
                else
                {
                    // Show an error message if the service request is not found
                    MessageBox.Show("Service request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Show an error message if the entered ID is not a valid number
                MessageBox.Show("Please enter a valid numeric ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // Event handler for the Filter button click
        // Filters the service requests based on their status
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected status from the combo box
            var selectedStatus = (cmbFilterStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            var requests = GetAllRequests(serviceRequestTree.Root); // Get all service requests from the tree

            if (selectedStatus != "All") // If a specific status is selected
            {
                // Filter the service requests by the selected status
                requests = requests.Where(r => r.Status == selectedStatus).ToList();
            }

            requests.ForEach(AssignIcons); // Assign icons to the filtered service requests
            listViewServiceRequests.ItemsSource = requests; // Set the ListView data source to the filtered requests
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // Event handler for when the user clicks on a document path (TextBlock)
        private void DocumentTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Check if the sender is a TextBlock (document path)
            if (sender is TextBlock textBlock)
            {
                // Getting the document path from the TextBlock
                string documentPath = textBlock.Text; 
                try
                {
                    // Try to open the document using the default application
                    Process.Start(new ProcessStartInfo(documentPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    // Show an error message if the document cannot be opened
                    MessageBox.Show($"Unable to open document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
