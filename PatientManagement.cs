//Model

Patient.cs
-----------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagementApp.Model
{
    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Slot { get; set; } // Morning or Evening
        public DateTime BookingDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string MedicalRecord { get; set; }
    }
}


//ViewModel

IPatientViewModel.cs
--------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientManagementApp.Model;

namespace PatientManagementApp.ViewModel
{
    public interface IPatientViewModel
    {
        ObservableCollection<Patient> Patients { get; }
        void RegisterPatient(Patient patient);
    }
}


PatientViewModel
---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientManagementApp.Model;

namespace PatientManagementApp.ViewModel
{
    public class PatientViewModel : IPatientViewModel
    {
        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();
        public ObservableCollection<Patient> ApprovedAppointments { get; } = new ObservableCollection<Patient>();

        public void RegisterPatient(Patient patient)
        {
            Patients.Add(patient);
            PatientRegistered?.Invoke(this, patient);
        }

        public void ApproveAppointment(Patient patient)
        {
            ApprovedAppointments.Add(patient);
            AppointmentConfirmed?.Invoke(this, patient);
        }

        public event EventHandler<Patient> PatientRegistered;
        public event EventHandler<Patient> AppointmentConfirmed;
    }
}




//Views

AppointmentConfirmation.xaml
------------------------------
<UserControl x:Class="PatientManagementApp.Views.AppointmentConfirmation"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel>
        <TextBlock Text="Appointments to Confirm" FontWeight="Bold" FontSize="16" Margin="10"/>

        <DataGrid Name="PatientsGrid" ItemsSource="{Binding Patients}" AutoGenerateColumns="True" 
                  SelectionMode="Single" Height="200" Margin="10"/>

        <Button x:Name="btnApproveAppointment" Content="Give Appointment" Margin="10" Click="btnApproveAppointment_Click"/>
    </StackPanel>
</UserControl>


AppointmentConfirmation.xaml.cs
----------------------------------------
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
using PatientManagementApp.Model;
using PatientManagementApp.ViewModel;

namespace PatientManagementApp.Views
{
    /// <summary>
    /// Interaction logic for AppointmentConfirmation.xaml
    /// </summary>
    public partial class AppointmentConfirmation : UserControl
    {
        public AppointmentConfirmation()
        {
            InitializeComponent();
        }

        private void btnApproveAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PatientViewModel viewModel && PatientsGrid.SelectedItem is Patient selectedPatient)
            {
                viewModel.ApproveAppointment(selectedPatient);
                MessageBox.Show($"Appointment for {selectedPatient.Name} approved!");
            }
        }
    }
}


PatientDashboard.xaml
--------------------------
<UserControl x:Class="PatientManagementApp.Views.PatientDashboard"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel Background="AliceBlue">
        <TextBlock Text="Approved Appointments" FontWeight="Bold" FontSize="16" Margin="10"/>

        <DataGrid ItemsSource="{Binding ApprovedAppointments}" AutoGenerateColumns="True" Height="200" Margin="10"/>
        <Button x:Name="btnBackToMain" Content="Exit" Click="btnBackToMain_Click" Width="57" HorizontalAlignment="Right"/>
    </StackPanel>
</UserControl>


PatientDashboard.xaml.cs
-----------------------------------
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

namespace PatientManagementApp.Views
{
    /// <summary>
    /// Interaction logic for PatientDashboard.xaml
    /// </summary>
    public partial class PatientDashboard : UserControl

    {
        public event EventHandler BackToMainRequested; 
        public PatientDashboard()
        {
            InitializeComponent();
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            BackToMainRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}




PatientRegistration.xaml
-----------------------------
<UserControl x:Class="PatientManagementApp.Views.PatientRegistration"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid>
        <Border HorizontalAlignment="Center" VerticalAlignment="Center" BorderBrush="Gray" BorderThickness="2" Padding="20" Width="273">
            <StackPanel>
                <TextBlock Text="Patient Registration" FontSize="20" FontWeight="Bold" TextAlignment="Center" Margin="0,0,0,20"/>

                <TextBlock Text="Name" Margin="0,10,0,5"/>
                <TextBox Name="NameTextBox"/>

                <TextBlock Text="Age" Margin="0,10,0,5"/>
                <TextBox Name="AgeTextBox"/>

                <TextBlock Text="Date of Birth" Margin="0,10,0,5"/>
                <DatePicker Name="DOBPicker"/>

                <TextBlock Text="Address" Margin="0,10,0,5"/>
                <TextBox Name="AddressTextBox"/>

                <TextBlock Text="Slot (Morning/Evening)" Margin="0,10,0,5"/>
                <ComboBox Name="SlotComboBox">
                    <ComboBoxItem Content="Morning"/>
                    <ComboBoxItem Content="Evening"/>
                </ComboBox>

                <TextBlock Text="Booking Date" Margin="0,10,0,5"/>
                <DatePicker Name="BookingDatePicker"/>

                <Button x:Name="btnRegister" Content="Register" Margin="0,20,0,0" Click="btnRegister_Click"/>
            </StackPanel>
        </Border>
    </Grid>
</UserControl>


PatientRegistration.xaml.cs
-----------------------------------------

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
using PatientManagementApp.Model;
using PatientManagementApp.ViewModel;

namespace PatientManagementApp.Views
{
    /// <summary>
    /// Interaction logic for PatientRegistration.xaml
    /// </summary>
    public partial class PatientRegistration : UserControl
    {
        public PatientRegistration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IPatientViewModel viewModel)
            {
                var patient = new Patient
                {
                    Name = NameTextBox.Text,
                    Age = int.Parse(AgeTextBox.Text),
                    DOB = DOBPicker.SelectedDate ?? DateTime.Now,
                    Address = AddressTextBox.Text,
                    Slot = SlotComboBox.Text,
                    BookingDate = BookingDatePicker.SelectedDate ?? DateTime.Now,
                    AppointmentDate = DateTime.Now.AddDays(1) // Default to tomorrow
                };
                viewModel.RegisterPatient(patient);

                NameTextBox.Clear();
                AgeTextBox.Clear();
                DOBPicker.SelectedDate = null;
                AddressTextBox.Clear();
                SlotComboBox.SelectedIndex = -1;
                BookingDatePicker.SelectedDate = null;  


                //var mainwindow = Window.GetWindow(this) as MainWindow;
                //mainwindow?.NavigateToMaainMenu();
            }
        }
    }
}


MainWindow.xaml
---------------------------
<Window x:Class="PatientManagementApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PatientManagementApp"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <StackPanel x:Name="NavigationPanel" Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Top" Margin="10">
            <Button x:Name="btnShowRegistration" Content="Register Patient" Margin="5" Click="btnShowRegistration_Click"/>
            <Button x:Name="btnShowAppointmentConfirmation" Content="Appointment Confirmation" Margin="5" Click="btnShowAppointmentConfirmation_Click"/>
            <Button x:Name="btnShowDashboard" Content="Patient Dashboard" Margin="5" Click="btnShowDashboard_Click"/>
        </StackPanel>

        <ContentControl Name="ContentArea" Grid.Row="1" Margin="10"/>
    </Grid>
</Window>


MainWindow.xaml.cs
------------------------------

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
using PatientManagementApp.ViewModel;
using PatientManagementApp.Views;

namespace PatientManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private readonly PatientViewModel _viewModel = new PatientViewModel();
        public MainWindow()
        {
            InitializeComponent();
            _viewModel.PatientRegistered += OnPatientRegistered;
            _viewModel.AppointmentConfirmed += OnAppointmentConfirmed;
        }

        private void btnShowRegistration_Click(object sender, RoutedEventArgs e)
        {
            //NavigationPanel.Visibility = Visibility.Collapsed;
            //ContentArea.Content = new Views.PatientRegistration { DataContext = _viewModel };
            var registration = new PatientRegistration { DataContext = _viewModel };
            ContentArea.Content = registration;
           // NavigationPanel.Visibility = Visibility.Collapsed;


        }

        private void btnShowAppointmentConfirmation_Click(object sender, RoutedEventArgs e)
        {
            //ContentArea.Content = new Views.AppointmentConfirmation { DataContext = _viewModel };
            var appointmentConfirmation = new AppointmentConfirmation { DataContext = _viewModel };
            ContentArea.Content = appointmentConfirmation;
           // NavigationPanel.Visibility = Visibility.Collapsed;

        }

        private void btnShowDashboard_Click(object sender, RoutedEventArgs e)
        {
            //ContentArea.Content = new Views.PatientDashboard { DataContext = _viewModel };
            var dashboard = new PatientDashboard { DataContext = _viewModel };
            dashboard.BackToMainRequested += Dasboard_BackToMainRequested;
            ContentArea.Content = dashboard;
            //NavigationPanel.Visibility = Visibility.Collapsed;
        }

        private void Dasboard_BackToMainRequested(object sender, EventArgs e)
        {
            NavigateBackToMainScreen();
        }
         private void OnPatientRegistered(object sender, PatientManagementApp.Model.Patient patient)
        {
            MessageBox.Show($"Patient {patient.Name} registered successfully!");
            NavigateBackToMainScreen();
        }

        private void OnAppointmentConfirmed(object sender, PatientManagementApp.Model.Patient patient)
        {
            MessageBox.Show($"Appointment for {patient.Name} confirmed and added to the Patient Dashboard!");
            NavigateBackToMainScreen();
        }

        private void NavigateBackToMainScreen()
        {
            ContentArea.Content = null;
            NavigationPanel.Visibility = Visibility.Visible;
        }
    }
}



------------


To add validations for the input fields in the Patient Registration form of your WPF application, you can use the following approach. This involves checking the user input and displaying appropriate error messages when the input is invalid. The validation will check if the fields are empty, if the name contains only letters, if the age is a valid integer, and if the dates are in a proper format.

Here's how to implement the validations:

Step 1: Update PatientRegistration.xaml.cs

You can modify the btnRegister_Click method to include validation checks for each field:

private void btnRegister_Click(object sender, RoutedEventArgs e)
{
    // Clear previous error messages
    string errorMessage = string.Empty;

    // Validate Name
    if (string.IsNullOrWhiteSpace(NameTextBox.Text) || !NameTextBox.Text.All(char.IsLetter))
    {
        errorMessage += "Please enter a valid name (letters only).\n";
    }

    // Validate Age
    if (string.IsNullOrWhiteSpace(AgeTextBox.Text) || !int.TryParse(AgeTextBox.Text, out int age) || age < 0)
    {
        errorMessage += "Please enter a valid age (positive integer).\n";
    }

    // Validate Date of Birth
    if (DOBPicker.SelectedDate == null)
    {
        errorMessage += "Please select a valid Date of Birth.\n";
    }

    // Validate Address
    if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
    {
        errorMessage += "Please enter an address.\n";
    }

    // Validate Slot
    if (SlotComboBox.SelectedIndex == -1)
    {
        errorMessage += "Please select a time slot (Morning/Evening).\n";
    }

    // Validate Booking Date
    if (BookingDatePicker.SelectedDate == null)
    {
        errorMessage += "Please select a valid Booking Date.\n";
    }

    // Show error messages if there are any
    if (!string.IsNullOrEmpty(errorMessage))
    {
        MessageBox.Show(errorMessage, "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }

    // Create a new patient if all validations pass
    var patient = new Patient
    {
        Name = NameTextBox.Text,
        Age = age,
        DOB = DOBPicker.SelectedDate.Value,
        Address = AddressTextBox.Text,
        Slot = SlotComboBox.Text,
        BookingDate = BookingDatePicker.SelectedDate.Value,
        AppointmentDate = DateTime.Now.AddDays(1) // Default to tomorrow
    };

    // Register the patient using the ViewModel
    if (DataContext is IPatientViewModel viewModel)
    {
        viewModel.RegisterPatient(patient);
    }

    // Clear the fields after registration
    NameTextBox.Clear();
    AgeTextBox.Clear();
    DOBPicker.SelectedDate = null;
    AddressTextBox.Clear();
    SlotComboBox.SelectedIndex = -1;
    BookingDatePicker.SelectedDate = null;  
}

Step 2: Explanation of Validations

1. Name Validation:

Checks if the name field is empty or contains any characters other than letters.



2. Age Validation:

Ensures that the age field is not empty, is a valid integer, and is a positive number.



3. Date of Birth Validation:

Validates that a date has been selected for the Date of Birth.



4. Address Validation:

Checks that the address field is not empty.



5. Slot Validation:

Ensures that a time slot has been selected from the ComboBox.



6. Booking Date Validation:

Validates that a booking date has been selected.




Step 3: User Interface Feedback

If any validation fails, a message box is shown detailing the specific errors, and the registration process is halted. This ensures that users are informed of what needs to be corrected before successfully registering a patient.

Step 4: Optional Enhancements

Consider adding visual cues (like red borders around fields) or using WPF Data Binding with IDataErrorInfo for more dynamic validation feedback as the user types. This can improve the user experience by providing real-time feedback.

This approach will ensure that users cannot proceed with invalid data, making your registration form robust and user-friendly.




