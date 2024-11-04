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

To achieve the validation requirements you've specified in your WPF application, we can enhance the PatientRegistration class to include real-time validation feedback. Below is the updated code that displays validation messages directly under each input field. The messages will show in red if the input is invalid.

Step 1: Updated XAML Code for PatientRegistration.xaml

Replace the contents of PatientRegistration.xaml with the following code:

<Window x:Class="PatientRegistrationApp.PatientRegistration"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Patient Registration" Height="400" Width="400">
    <Grid Margin="10">
        <StackPanel>
            <TextBlock Text="Patient Registration" FontSize="20" FontWeight="Bold" Margin="0,0,0,20" />

            <TextBlock Text="Name:" />
            <TextBox x:Name="NameTextBox" Margin="0,0,0,10" TextChanged="NameTextBox_TextChanged" />
            <TextBlock x:Name="NameErrorTextBlock" Foreground="Red" Visibility="Collapsed" />

            <TextBlock Text="Age:" />
            <TextBox x:Name="AgeTextBox" Margin="0,0,0,10" TextChanged="AgeTextBox_TextChanged" />
            <TextBlock x:Name="AgeErrorTextBlock" Foreground="Red" Visibility="Collapsed" />

            <TextBlock Text="Date of Birth:" />
            <DatePicker x:Name="DOBPicker" SelectedDateChanged="DOBPicker_SelectedDateChanged" Margin="0,0,0,10" />
            <TextBlock x:Name="DOBErrorTextBlock" Foreground="Red" Visibility="Collapsed" />

            <TextBlock Text="Address:" />
            <TextBox x:Name="AddressTextBox" Margin="0,0,0,10" />

            <TextBlock Text="Time Slot:" />
            <ComboBox x:Name="SlotComboBox" Margin="0,0,0,10">
                <ComboBoxItem Content="Morning" />
                <ComboBoxItem Content="Evening" />
            </ComboBox>

            <TextBlock Text="Booking Date:" />
            <DatePicker x:Name="BookingDatePicker" SelectedDateChanged="BookingDatePicker_SelectedDateChanged" Margin="0,0,0,10" />
            <TextBlock x:Name="BookingDateErrorTextBlock" Foreground="Red" Visibility="Collapsed" />

            <Button Content="Register" Click="btnRegister_Click" Width="100" />
        </StackPanel>
    </Grid>
</Window>

Step 2: Updated C# Code for PatientRegistration.xaml.cs

Replace the contents of PatientRegistration.xaml.cs with the following code:

using System;
using System.Linq;
using System.Windows;

namespace PatientRegistrationApp
{
    public partial class PatientRegistration : Window
    {
        public PatientRegistration()
        {
            InitializeComponent();
        }

        private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || !NameTextBox.Text.All(char.IsLetter))
            {
                NameErrorTextBlock.Text = "Enter name with alphabets only.";
                NameErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                NameErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void AgeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AgeTextBox.Text) || !int.TryParse(AgeTextBox.Text, out int age) || age < 0)
            {
                AgeErrorTextBlock.Text = "Enter a valid age (positive integer).";
                AgeErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                AgeErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void DOBPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DOBPicker.SelectedDate == null || DOBPicker.SelectedDate.Value >= DateTime.Now)
            {
                DOBErrorTextBlock.Text = "Date of Birth must be less than today.";
                DOBErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                DOBErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void BookingDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BookingDatePicker.SelectedDate == null || BookingDatePicker.SelectedDate.Value < DateTime.Now)
            {
                BookingDateErrorTextBlock.Text = "Booking Date must be today or later.";
                BookingDateErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                BookingDateErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Validate all fields before registration
            if (NameErrorTextBlock.Visibility == Visibility.Visible ||
                AgeErrorTextBlock.Visibility == Visibility.Visible ||
                DOBErrorTextBlock.Visibility == Visibility.Visible ||
                BookingDateErrorTextBlock.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Please fix the errors before proceeding.", "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new patient if all validations pass
            var patient = new Patient
            {
                Name = NameTextBox.Text,
                Age = int.Parse(AgeTextBox.Text),
                DOB = DOBPicker.SelectedDate.Value,
                Address = AddressTextBox.Text,
                Slot = SlotComboBox.Text,
                BookingDate = BookingDatePicker.SelectedDate.Value,
                AppointmentDate = DateTime.Now.AddDays(1) // Default to tomorrow
            };

            // Register the patient (this is where you'd normally save to a database or similar)
            RegisterPatient(patient);

            // Clear the fields after registration
            NameTextBox.Clear();
            AgeTextBox.Clear();
            DOBPicker.SelectedDate = null;
            AddressTextBox.Clear();
            SlotComboBox.SelectedIndex = -1;
            BookingDatePicker.SelectedDate = null;
        }

        private void RegisterPatient(Patient patient)
        {
            // Logic to register the patient (e.g., save to a database or show a success message)
            MessageBox.Show($"Patient {patient.Name} registered successfully!", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Slot { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}

Explanation of the Code Changes

1. Real-time Validation:

Name Validation: When the user types in the NameTextBox, the NameTextBox_TextChanged method checks if the input contains only letters. If not, an error message appears below the input box in red.

Age Validation: The AgeTextBox_TextChanged method checks for a valid positive integer input. If invalid, it displays an appropriate error message.

DOB Validation: The DOBPicker_SelectedDateChanged method ensures the selected date of birth is before today. If not, an error message is shown.

Booking Date Validation: The BookingDatePicker_SelectedDateChanged method checks that the booking date is today or later. If invalid, it shows an error message.



2. Visibility Control:

Each error message is managed using Visibility properties (e.g., Visible or Collapsed), ensuring the user sees immediate feedback as they enter data.



3. Register Button Logic:

Before creating a Patient, the button click handler checks that there are no visible error messages. If any errors are present, it alerts the user to fix them before proceeding.




Running the Application

1. Build and Run: Compile and run your application in Visual Studio.


2. Test Input: Enter invalid data to test the validations. Correct the inputs to see how the error messages update in real time.



This updated implementation meets your requirements by providing instant feedback for each input field, ensuring a more user-friendly experience.







