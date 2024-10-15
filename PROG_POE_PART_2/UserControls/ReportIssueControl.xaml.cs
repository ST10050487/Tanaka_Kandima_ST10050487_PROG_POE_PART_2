using Microsoft.Win32;
using PROG_POE_PART_2.Classes;
using System;
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
    /// Interaction logic for ReportIssueControl.xaml
    /// </summary>
    public partial class ReportIssueControl : UserControl
    {
        public ReportIssueControl()
        {
            //Initializing the component
            InitializeComponent();
            // Clearing the error messages when the user focuses on input fields
            locationTxt.GotFocus += (s, e) => { ClearLocationError(s, e); UpdateCompletionProgressBar(); };
            CategoryCmbx.GotFocus += (s, e) => { ClearCategoryError(s, e); UpdateCompletionProgressBar(); };
            descriptionTxt.GotFocus += (s, e) => { ClearDescriptionError(s, e); UpdateCompletionProgressBar(); };
            // Handling text changed events for real-time progress updates
            locationTxt.TextChanged += (s, e) => UpdateCompletionProgressBar();
            descriptionTxt.TextChanged += (s, e) => UpdateCompletionProgressBar();
            // Handling category selection change
            CategoryCmbx.SelectionChanged += (s, e) => { CheckCategory(); UpdateCompletionProgressBar(); };
            // Populating the Category combo box
            PopulateCategory();
            // Displaying the first item in the combo box
            CategoryCmbx.SelectedIndex = 0;
            // Hiding the upload bar
            UploadProgressBar.Visibility = Visibility.Hidden;
            // Initializing the completion progress bar
            CompletionPrograsssBar.Maximum = 3;
            CompletionPrograsssBar.Value = 0;
            // Making the completion progress bar visible
            CompletionPrograsssBar.Visibility = Visibility.Visible;
        }
        //****************************************************************NAKA*********************************************************//
        //Declaring Global Variables
        string location = "";
        string category = "";
        string description = "";
        //****************************************************************NAKA*********************************************************//
        //Creating A List of Issues
        public List<Issue> Issues = new List<Issue>();
        //Creating  List of Pictures
        private List<string> picturesList = new List<string>();
        // List to store paths of documents
        List<string> documentsList = new List<string>();
        //****************************************************************NAKA*********************************************************//
        //Creating an instance of the Validations class
        Validations validate = new Validations();
        //****************************************************************NAKA*********************************************************//
        // A method to submit the report
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            GetUserInput();
            if ((validate.ValidateLocation(location) == true) && (validate.ValidateCategory(category) == true) && (validate.ValidateDescription(description) == true))
            {
                Issue issue = new Issue(location, category, description, picturesList);
                IssueManager.Issues.Add(issue);
                informationlbl.Foreground = new SolidColorBrush(Colors.Green);
                informationlbl.Content = "Report Submitted Successfully";
                ClearBtn_Click(sender, e);
                MessageBox.Show("Report Submitted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                informationlbl.Foreground = new SolidColorBrush(Colors.Red);
                informationlbl.Content = "Report Not Submitted";
                MessageBox.Show("SUBMISSION FAILED", "ERROR⚠️", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //****************************************************************NAKA*********************************************************//
        // A method to get the user input
        public void GetUserInput()
        {
            //Calling the methods to get the user input
            GetLocation();
            CheckCategory();
            GetDescription();
        }
        //****************************************************************NAKA*********************************************************//
        //A method to get the location
        public void GetLocation()
        {
            if (validate.ValidateLocation(locationTxt.Text) == true)
            {
                location = locationTxt.Text;
            }
            else
            {
                locationErrorlbl.Foreground = new SolidColorBrush(Colors.Red);
                locationErrorlbl.Content = "Please Enter A Location";
            }
            UpdateCompletionProgressBar();
        }
        //****************************************************************NAKA*********************************************************//
        // A method to select the category
        private void CategoryCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            category = CategoryCmbx.SelectedItem.ToString();
            UpdateCompletionProgressBar();
        }
        //****************************************************************NAKA*********************************************************//
        // A method to validate category input
        public void CheckCategory()
        {
            if (validate.ValidateCategory(category) == false)
            {
                categoryErrorlbl.Foreground = new SolidColorBrush(Colors.Red);
                categoryErrorlbl.Content = "Please Select A Category";
            }
        }
        //****************************************************************NAKA*********************************************************//
        // A method to get the description
        public void GetDescription()
        {
            if (validate.ValidateDescription(descriptionTxt.Text) == true)
            {
                description = descriptionTxt.Text;
            }
            else
            {
                descriptionErrorlbl.Foreground = new SolidColorBrush(Colors.Red);
                descriptionErrorlbl.Content = "Please Enter A Description";
            }
            UpdateCompletionProgressBar();
        }
        //****************************************************************NAKA*********************************************************//
        //A method to get the Images
        private void AddPictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image and Document Files|*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt|All Files|*.*";

            // Use WPF's true/false check
            if (openFileDialog.ShowDialog() == true)
            {
                string[] filePaths = openFileDialog.FileNames;

                // Clear existing images, documents, and progress bar
                Pictures.Children.Clear();
                picturesList.Clear();
                documentsList.Clear();
                UploadProgressBar.Value = 0;

                // Set progress bar maximum value to the number of files to be processed
                UploadProgressBar.Maximum = filePaths.Length;

                // Process files asynchronously
                Task.Run(() => ProcessFiles(filePaths));
            }
        }
        //****************************************************************NAKA*********************************************************//
        // Method to process files and update progress bar
        // Method to process files and update progress bar
        private void ProcessFiles(string[] filePaths)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                string extension = System.IO.Path.GetExtension(filePath).ToLower();

                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                {
                    // Add image to the UI
                    Dispatcher.Invoke(() =>
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri(filePath));
                        image.Width = 100;
                        image.Height = 100;
                        image.Margin = new Thickness(5);

                        Pictures.Children.Add(image);
                        picturesList.Add(filePath);
                    });
                }
                else
                {
                    // Add document to the list
                    Dispatcher.Invoke(() =>
                    {
                        documentsList.Add(filePath);

                        TextBlock documentLabel = new TextBlock();
                        documentLabel.Text = System.IO.Path.GetFileName(filePath);
                        documentLabel.Margin = new Thickness(5);
                        documentLabel.Foreground = new SolidColorBrush(Colors.Blue);

                        documentLabel.MouseLeftButtonUp += (s, ev) => System.Diagnostics.Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                        Pictures.Children.Add(documentLabel);
                    });
                }
                // Making the progress bar visible
                Dispatcher.Invoke(() =>
                {
                    UploadProgressBar.Visibility = Visibility.Visible;
                });
                // Update the progress bar
                Dispatcher.Invoke(() =>
                {
                    UploadProgressBar.Value = i + 1;
                });
                // Displaying the number of files uploaded
                Dispatcher.Invoke(() =>
                {
                    uploadMessagelbl.Content = $"Uploaded {i + 1} of {filePaths.Length} files";
                });
            }
        }
        //****************************************************************NAKA*********************************************************//
        // A method to populate the Category combo box
        public void PopulateCategory()
        {
            CategoryCmbx.Items.Add("Roads");
            CategoryCmbx.Items.Add("Electricity");
            CategoryCmbx.Items.Add("Water");
            CategoryCmbx.Items.Add("Sanitation");
            CategoryCmbx.Items.Add("Housing");
            CategoryCmbx.Items.Add("Health");
            CategoryCmbx.Items.Add("Education");
            CategoryCmbx.Items.Add("Other");
        }
        //****************************************************************NAKA*********************************************************//
        // A method to clear all the input fields and error messages
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            //Clearing the input fields
            locationTxt.Text = "";
            CategoryCmbx.SelectedIndex = 1;
            descriptionTxt.Text = "";
            Pictures.Children.Clear();
            UploadProgressBar.Value = 0;
            //Clearing the error messages
            locationErrorlbl.Content = "";
            categoryErrorlbl.Content = "";
            descriptionErrorlbl.Content = "";
            informationlbl.Content = "";
            uploadMessagelbl.Content = "";
            //Clearing variables
            //Declaring Global Variables
            location = "";
            category = "";
            description = "";
            //Hide the upload bar
            UploadProgressBar.Visibility = Visibility.Hidden;
            //Hide the completion progress bar
            CompletionPrograsssBar.Visibility = Visibility.Hidden;
        }
        //****************************************************************NAKA*********************************************************//
        // A method to provide information about the reporting an issue
        public void Information()
        {
            informationlbl.Foreground = new SolidColorBrush(Colors.Black);
            informationlbl.Content = "Enter the name of the location where the problem is\n" +
                "Select the category the issue belongs to in the selection box\n" +
                "Add a description of the issue for clarity\n" +
                "Click the 'Add Pictures' button to upload images of \nthe issue [not compulsory]\n" +
                "Click the Question Mark again to hide Hints";
        }
        //****************************************************************NAKA*********************************************************//
        // A method to display and hide instructions on how submit a report
        private void InforBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(informationlbl.Content.ToString()))
            {
                Information();
            }
            else
            {
                informationlbl.Content = "";
            }
        }
        //****************************************************************NAKA*********************************************************//
        // An Event handler for clearing location error
        private void ClearLocationError(object sender, EventArgs e)
        {
            locationErrorlbl.Content = "";
            locationErrorlbl.Foreground = new SolidColorBrush(Colors.Black);
            locationTxt.Text = "";
        }
        //****************************************************************NAKA*********************************************************//
        // An Event handler for clearing category error
        private void ClearCategoryError(object sender, EventArgs e)
        {
            categoryErrorlbl.Content = "";
            categoryErrorlbl.Foreground = new SolidColorBrush(Colors.Black);
        }
        //****************************************************************NAKA*********************************************************//
        // An Event handler for clearing description error
        private void ClearDescriptionError(object sender, EventArgs e)
        {
            descriptionErrorlbl.Content = "";
            descriptionErrorlbl.Foreground = new SolidColorBrush(Colors.Black);
            descriptionTxt.Text = "";
        }
        //****************************************************************NAKA*********************************************************//
        // A method to update the completion progress bar
        private void UpdateCompletionProgressBar()
        {
            if (validate == null || descriptionTxt == null)
            {
                // Handle the null reference scenario
                return;
            }

            int completedFields = 0;

            // Check each input and increment if valid
            if (validate.ValidateLocation(locationTxt.Text))
                completedFields++;
            if (validate.ValidateCategory(category))
                completedFields++;
            if (validate.ValidateDescription(descriptionTxt.Text))
                completedFields++;

            // Update progress bar value
            CompletionPrograsssBar.Value = completedFields;
        }
    }
}
