using PROG_POE_PART_2.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <summary>
    /// Interaction logic for CommunityControl.xaml
    /// </summary>
    public partial class CommunityControl : UserControl
    {
        // Store ratings
        private Hashtable issueRatings = new Hashtable(); 
        private CustomPriorityQueue<Issue> priorityIssues = new CustomPriorityQueue<Issue>();
        public CommunityControl()
        {
            InitializeComponent();
            LoadIssues();
        }
        private void LoadIssues()
        {
            // Fetching issues from the IssueManager and bind to ListView
            issuesListView.ItemsSource = IssueManager.Issues;

            // Check if there are any issues and update the visibility of the message
            if (IssueManager.Issues == null || !IssueManager.Issues.Any())
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
                    // Store the rating in the hashtable or directly in the issue
                    issueRatings[issue] = ratingValue; // Or store in issue object itself if preferred
                    Debug.WriteLine($"Rating for issue '{issue.Description}' set to {ratingValue}");
                }
            }
        }
    }
}
   
