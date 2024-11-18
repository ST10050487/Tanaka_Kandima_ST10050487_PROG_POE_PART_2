using PROG_POE_PART_2.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PROG_POE_PART_2.UserControls
{
    /// <summary>
    /// Interaction logic for CommunityControl.xaml
    /// </summary>
    public partial class CommunityControl : UserControl
    {
        // Store ratings
        private Hashtable issueRatings = new Hashtable();
        private BinarySearchTreeIssues issueTree = ReportIssueControl.IssueTree;

        public CommunityControl()
        {
            InitializeComponent();
            LoadIssues();
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // A method to load issues from the IssueManager and bind to the ListView
        private void LoadIssues()
        {
            // Fetching issues from the binary search tree and bind to ListView
            var sortedIssues = issueTree.InOrderTraversal();
            Debug.WriteLine($"Number of issues loaded: {sortedIssues.Count}");
            foreach (var issue in sortedIssues)
            {
                Debug.WriteLine($"Loaded issue: {issue.Description}");
            }
            issuesListView.ItemsSource = sortedIssues;

            // Check if there are any issues and update the visibility of the message
            if (sortedIssues == null || !sortedIssues.Any())
            {
                noIssuesMessage.Visibility = Visibility.Visible;
                issuesListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                noIssuesMessage.Visibility = Visibility.Collapsed;
                issuesListView.Visibility = Visibility.Visible;
            }
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // A method to move the window
        private void OnSubmitComment(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnSubmitComment called");

            var button = sender as Button;
            if (button == null)
            {
                Debug.WriteLine("Button is null, exiting method.");
                return;
            }

            // Get the associated issue via the button's DataContext
            var issue = button.DataContext as Issue;
            if (issue == null)
            {
                Debug.WriteLine("Issue is null, exiting method.");
                return;
            }

            // Get the parent StackPanel of the button (this will contain the TextBox for comments)
            var stackPanel = button.Parent as StackPanel;
            if (stackPanel == null)
            {
                Debug.WriteLine("StackPanel is null, exiting method.");
                return;
            }

            // Find the commentTextBox within the StackPanel
            var commentTextBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "commentTextBox");
            if (commentTextBox == null)
            {
                Debug.WriteLine("TextBox not found.");
                return;
            }

            // Add comment if the TextBox is not empty
            if (!string.IsNullOrEmpty(commentTextBox.Text))
            {
                issue.AddComment(commentTextBox.Text);
                commentTextBox.Text = ""; // Clear TextBox
                Debug.WriteLine("Comment added and TextBox cleared.");
            }

            // Refresh the ListView to show the new comment
            issuesListView.Items.Refresh();
        }

        //************************************************************************************NAKA*********************************************************************************************//
        // A method to move the window
        private void OnRatingChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                Debug.WriteLine("ComboBox is null, exiting method.");
                return;
            }

            // Get the associated issue via the ComboBox's DataContext
            var issue = comboBox.DataContext as Issue;
            if (issue == null)
            {
                Debug.WriteLine("Issue is null, exiting method.");
                return;
            }

            // Capture the selected rating value
            var selectedRating = comboBox.SelectedItem as ComboBoxItem;
            if (selectedRating != null)
            {
                int ratingValue;
                if (int.TryParse(selectedRating.Content.ToString(), out ratingValue))
                {
                    // Check if the new priority value is unique
                    if (issueTree.InOrderTraversal().Any(i => i.Priority == ratingValue))
                    {
                        MessageBox.Show("Duplicate priorities are not allowed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Store the rating in the issue object itself
                    issue.Priority = ratingValue;
                    Debug.WriteLine($"Rating for issue '{issue.Description}' set to {ratingValue}");
                }
            }

            // Refresh the ListView to show the updated priorities
            LoadIssues();
        }

        // Event handler for when the user clicks on a document path (TextBlock)
        private void DocumentTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Checking if the sender is a TextBlock (document path)
            if (sender is TextBlock textBlock)
            {
                // Getting the document path from the TextBlock's DataContext
                if (textBlock.DataContext is DocumentItem documentItem)
                {
                    try
                    {
                        // Trying to open the document using the default application
                        Process.Start(new ProcessStartInfo(documentItem.Path) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        // Showing an error message if the document cannot be opened
                        MessageBox.Show($"Unable to open document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Event handler for when the user clicks on a video name (TextBlock)
        private void VideoTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Checking if the sender is a TextBlock (video path)
            if (sender is TextBlock textBlock)
            {
                // Getting the video path from the TextBlock's DataContext
                if (textBlock.DataContext is string videoPath)
                {
                    try
                    {
                        // Trying to open the video using the default application
                        Process.Start(new ProcessStartInfo(videoPath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        // Showing an error message if the video cannot be opened
                        MessageBox.Show($"Unable to open video: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
