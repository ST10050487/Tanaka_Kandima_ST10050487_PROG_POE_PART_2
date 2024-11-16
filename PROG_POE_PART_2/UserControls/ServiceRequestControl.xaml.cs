using PROG_POE_PART_2.Classes;
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
    public partial class ServiceRequestControl : UserControl
    {
        // Reference to the BinarySearchTree holding all service requests
        private BinarySearchTree serviceRequestTree = ReportIssueControl.ServiceRequestTree;

        // Constructor initializes the user control and populates the ListView with service requests
        public ServiceRequestControl()
        {
            InitializeComponent();
            PopulateListView();
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Populates the ListView with all service requests from the BinarySearchTree.
        /// Uses in-order traversal to retrieve sorted requests and assigns icons to each request.
        /// </summary>
        private void PopulateListView()
        {
            // Retrieve all service requests from the tree
            var requests = GetAllRequests(serviceRequestTree.Root);

            // Assign icons to each request
            foreach (var request in requests)
            {
                AssignIcons(request);
            }

            // Bind the populated list of requests to the ListView
            listViewServiceRequests.ItemsSource = requests;
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Recursively retrieves all service requests from the BinarySearchTree in sorted order.
        /// </summary>
        /// <param name="node">The current node in the BinarySearchTree.</param>
        /// <returns>A list of all service requests in sorted order by their IDs.</returns>
        private List<ServiceRequest> GetAllRequests(ServiceRequestNode node)
        {
            var requests = new List<ServiceRequest>();

            // Perform in-order traversal: left -> root -> right
            if (node != null)
            {
                requests.AddRange(GetAllRequests(node.Left)); // Traverse the left subtree
                requests.Add(node.Data);                     // Add the current node's data
                requests.AddRange(GetAllRequests(node.Right)); // Traverse the right subtree
            }

            return requests;
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Assigns icons to the video files in a service request.
        /// This updates the video file paths to reference a default video icon image.
        /// </summary>
        /// <param name="request">The service request to update.</param>
        private void AssignIcons(ServiceRequest request)
        {
            // Update each video entry with the path to the video icon
            for (int i = 0; i < request.Videos.Count; i++)
            {
                request.Videos[i] = "pack://application:,,,/Images/VideoIcon.png";
            }
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Handles the click event for the search button. Searches for a service request by its ID
        /// and updates the ListView with the result.
        /// </summary>
        /// <param name="sender">The search button.</param>
        /// <param name="e">Event arguments.</param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Validate and parse the input ID
            if (int.TryParse(txtSearchId.Text, out int id))
            {
                // Search for the service request by ID
                var result = serviceRequestTree.Search(id);

                if (result != null)
                {
                    // If found, assign icons and display the request
                    AssignIcons(result);
                    listViewServiceRequests.ItemsSource = new List<ServiceRequest> { result };
                }
                else
                {
                    // Show an error message if the request is not found
                    MessageBox.Show("Service request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Show an error message if the input is invalid
                MessageBox.Show("Please enter a valid numeric ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Handles the click event for the filter button. Filters service requests based on their status
        /// and updates the ListView with the filtered results.
        /// </summary>
        /// <param name="sender">The filter button.</param>
        /// <param name="e">Event arguments.</param>
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected status from the ComboBox
            var selectedStatus = (cmbFilterStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedStatus == "All")
            {
                // If "All" is selected, display all service requests
                PopulateListView();
            }
            else
            {
                // Filter requests based on the selected status
                var requests = GetAllRequests(serviceRequestTree.Root)
                               .Where(r => r.Status == selectedStatus)
                               .ToList();

                // Assign icons to the filtered requests
                foreach (var request in requests)
                {
                    AssignIcons(request);
                }

                // Update the ListView with the filtered requests
                listViewServiceRequests.ItemsSource = requests;
            }
        }

        //****************************************************************NAKA*********************************************************//
        /// <summary>
        /// Handles the click event for the document text block. Attempts to open the document
        /// associated with the clicked text block.
        /// </summary>
        /// <param name="sender">The TextBlock representing a document.</param>
        /// <param name="e">Event arguments.</param>
        private void DocumentTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                // Get the document path from the TextBlock's text
                string documentPath = textBlock.Text;

                try
                {
                    // Attempt to open the document using the default application
                    Process.Start(new ProcessStartInfo(documentPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    // Show an error message if the document could not be opened
                    MessageBox.Show($"Unable to open document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
