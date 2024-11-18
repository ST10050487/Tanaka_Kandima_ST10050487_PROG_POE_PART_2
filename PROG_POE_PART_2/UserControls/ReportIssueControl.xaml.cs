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
        // List to store paths of videos
        List<string> videosList = new List<string>();
        // Shared collection of service requests
        public static List<ServiceRequest> SharedServiceRequests = new List<ServiceRequest>();
        //****************************************************************NAKA*********************************************************//
        //Creating an instance of the Validations class
        Validations validate = new Validations();
        //Creating an instance of the BinarySearchTree class
        public static BinarySearchTree ServiceRequestTree = new BinarySearchTree();
        //Creating an instance of the BinarySearchTreeIssues class
        public static BinarySearchTreeIssues IssueTree = new BinarySearchTreeIssues();
        //****************************************************************NAKA*********************************************************//
        // A method to submit the report
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            GetUserInput();
            if ((validate.ValidateLocation(location) == true) && (validate.ValidateCategory(category) == true) && (validate.ValidateDescription(description) == true))
            {
                // Create a new Issue object and set its properties
                Issue issue = new Issue(location, category, description, picturesList)
                {
                    Documents = documentsList.Select(doc => new DocumentItem
                    {
                        Name = System.IO.Path.GetFileName(doc),
                        Icon = GetIconForExtension(System.IO.Path.GetExtension(doc)),
                        Path = doc
                    }).ToList(),
                    Videos = new List<string>(videosList)
                };

                // Add the issue to the IssueManager
                IssueManager.Issues.Add(issue);

                // Create a new ServiceRequest
                var newRequest = new ServiceRequest(SharedServiceRequests.Count + 1, location, description, "Pending", DateTime.Now, null);
                newRequest.Images.AddRange(picturesList);
                newRequest.Documents.AddRange(documentsList.Select(doc => new DocumentItem
                {
                    Name = System.IO.Path.GetFileName(doc),
                    Icon = GetIconForExtension(System.IO.Path.GetExtension(doc)),
                    Path = doc
                }));
                newRequest.Videos.AddRange(videosList);
                newRequest.AssignRandomStatus();

                // Add the new request to the shared collection and binary search tree
                SharedServiceRequests.Add(newRequest);
                ServiceRequestTree.Insert(newRequest);

                // Add the issue to the binary search tree
                IssueTree.Insert(issue);

                Debug.WriteLine($"Issue inserted: {issue.Description}");
                informationlbl.Foreground = new SolidColorBrush(Colors.Green);
                informationlbl.Content = "Report Submitted Successfully";
                ClearBtn_Click(sender, e);
                MessageBox.Show("Report Submitted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear the videosList after submission
                videosList.Clear();
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Document files (*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt)|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt|Video files (*.mp4;*.avi;*.mov)|*.mp4;*.avi;*.mov|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string[] filePaths = openFileDialog.FileNames;

                Pictures.Children.Clear();
                picturesList.Clear();
                documentsList.Clear();
                UploadProgressBar.Value = 0;

                UploadProgressBar.Maximum = filePaths.Length;

                Task.Run(() => ProcessFiles(filePaths));
            }
        }
        //****************************************************************NAKA*********************************************************//
        // Method to process files and update progress bar
        private void ProcessFiles(string[] filePaths)
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                string extension = System.IO.Path.GetExtension(filePath).ToLower();

                Dispatcher.Invoke(() =>
                {
                    StackPanel itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
                    TextBlock fileNameTextBlock = new TextBlock
                    {
                        Text = System.IO.Path.GetFileName(filePath),
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5)
                    };

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                    {
                        // Handle image files
                        Image image = new Image
                        {
                            Source = new BitmapImage(new Uri(filePath)),
                            Width = 100,
                            Height = 100,
                            Margin = new Thickness(5)
                        };
                        Button removeButton = new Button
                        {
                            Content = "Remove",
                            Tag = filePath,
                            Margin = new Thickness(5),
                            Background = new SolidColorBrush(Colors.Red)
                        };
                        removeButton.Click += RemoveItem_Click;
                        itemPanel.Children.Add(image);
                        itemPanel.Children.Add(fileNameTextBlock);
                        itemPanel.Children.Add(removeButton);
                        Pictures.Children.Add(itemPanel);
                        picturesList.Add(filePath);
                    }
                    else if (extension == ".pdf" || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".xls" || extension == ".xlsx")
                    {
                        // Handle document files
                        DisplayDocumentIcon(filePath, extension);
                    }
                    else if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                    {
                        // Handle video files
                        Image videoIcon = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Images/VideoIcon.png")),
                            Width = 50,
                            Height = 50,
                            Margin = new Thickness(5)
                        };

                        fileNameTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                        fileNameTextBlock.MouseLeftButtonUp += (s, ev) =>
                        {
                            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                        };

                        Button removeButton = new Button
                        {
                            Content = "Remove",
                            Tag = filePath,
                            Margin = new Thickness(5),
                            Background = new SolidColorBrush(Colors.Red)
                        };
                        removeButton.Click += RemoveItem_Click;

                        itemPanel.Children.Add(videoIcon);
                        itemPanel.Children.Add(fileNameTextBlock);
                        itemPanel.Children.Add(removeButton);
                        Pictures.Children.Add(itemPanel);
                        videosList.Add(filePath); // Add video path to the list
                    }

                    UploadProgressBar.Visibility = Visibility.Visible;
                    UploadProgressBar.Value = i + 1;
                    uploadMessagelbl.Content = $"Uploaded {i + 1} of {filePaths.Length} files";
                });
            }
        }


        //****************************************************************NAKA*********************************************************//
        // Method to display document icon
        private void DisplayDocumentIcon(string filePath, string extension)
        {
            StackPanel docPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            Image icon = new Image
            {
                Width = 50,
                Height = 50,
                Margin = new Thickness(5)
            };

            if (extension == ".pdf")
            {
                icon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/PdfIcon.png"));
            }
            else if (extension == ".doc" || extension == ".docx")
            {
                icon.Source = new BitmapImage(new Uri("pack://application:,,,/PROG_POE_PART_2;component/Images/WordIcon.png"));
            }
            else
            {
                icon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/DocumentsIcon.png"));
            }

            TextBlock documentLabel = new TextBlock
            {
                Text = System.IO.Path.GetFileName(filePath),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Blue),
                Margin = new Thickness(5, 0, 0, 0)
            };

            documentLabel.MouseLeftButtonUp += (s, ev) =>
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            };

            Button removeButton = new Button
            {
                Content = "Remove",
                Tag = filePath,
                Margin = new Thickness(5),
                Background = new SolidColorBrush(Colors.Red)
            };
            removeButton.Click += RemoveItem_Click;

            docPanel.Children.Add(icon);
            docPanel.Children.Add(documentLabel);
            docPanel.Children.Add(removeButton);

            Pictures.Children.Add(docPanel);
            documentsList.Add(filePath);
        }
        //****************************************************************NAKA*********************************************************//
        // Method to remove an item from the list
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            Button removeButton = sender as Button;
            if (removeButton != null)
            {
                string filePath = removeButton.Tag as string;
                if (!string.IsNullOrEmpty(filePath))
                {
                    picturesList.Remove(filePath);
                    documentsList.Remove(filePath);
                    UpdatePicturesDisplay();
                }
            }
        }
        //****************************************************************NAKA*********************************************************//
        // Method to update the pictures display
        private void UpdatePicturesDisplay()
        {
            Pictures.Children.Clear();
            foreach (var filePath in picturesList)
            {
                StackPanel itemPanel = new StackPanel { Orientation = Orientation.Horizontal };
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(filePath)),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5)
                };
                Button removeButton = new Button
                {
                    Content = "Remove",
                    Tag = filePath,
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush(Colors.Red)
                };
                removeButton.Click += RemoveItem_Click;
                itemPanel.Children.Add(image);
                itemPanel.Children.Add(removeButton);
                Pictures.Children.Add(itemPanel);
            }
            foreach (var filePath in documentsList)
            {
                StackPanel itemPanel = new StackPanel { Orientation = Orientation.Horizontal };
                TextBlock textBlock = new TextBlock
                {
                    Text = System.IO.Path.GetFileName(filePath),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5)
                };
                Button removeButton = new Button
                {
                    Content = "Remove",
                    Tag = filePath,
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush(Colors.Red)
                };
                removeButton.Click += RemoveItem_Click;
                itemPanel.Children.Add(textBlock);
                itemPanel.Children.Add(removeButton);
                Pictures.Children.Add(itemPanel);
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
            if (informationlbl != null)
            {
                if (string.IsNullOrEmpty(informationlbl.Content?.ToString()) || informationlbl.Content.ToString().Equals(""))
                {
                    Information();
                }
                else
                {
                    informationlbl.Content = "";
                }
            }
            else
            {
                Information();
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
        private string GetIconForExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "pack://application:,,,/Images/PdfIcon.png";
                case ".doc":
                case ".docx":
                    return "pack://application:,,,/Images/WordIcon.png";
                case ".xls":
                case ".xlsx":
                    return "pack://application:,,,/Images/ExcelIcon.png";
                case ".txt":
                    return "pack://application:,,,/Images/TextIcon.png";
                default:
                    return "pack://application:,,,/Images/DefaultIcon.png";
            }
        }
        //****************************************************************NAKA*********************************************************//
    }
}