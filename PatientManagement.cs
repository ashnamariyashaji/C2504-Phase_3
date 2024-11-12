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
        //public DateTime DOB { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Slot { get; set; } // Morning or Evening
        public DateTime BookingDate { get; set; }
        public string AppointmentDate { get; set; }
   
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
    <StackPanel Background="AliceBlue" Width="643" Height="444">
        <TextBlock Text="Appointments to Confirm" FontWeight="Bold" FontSize="16" Margin="10"/>

        <DataGrid Name="PatientsGrid" ItemsSource="{Binding Patients}" AutoGenerateColumns="True" 
                  SelectionMode="Single" Height="229" Margin="10"/>
        <Button x:Name="btnApproveAppointment" Content="Give Appointment" Click="btnApproveAppointment_Click" Width="138" />
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
            else
            {
                MessageBox.Show("Please Select patient details");
            }
        }
    }
}



PatientDashboard.xaml
--------------------------
<UserControl x:Class="PatientManagementApp.Views.PatientDashboard"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel >
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
    <Grid Width="383">
        <Border HorizontalAlignment="Center" VerticalAlignment="Center" BorderBrush="Gray" BorderThickness="2" Padding="20" Width="363">
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
            if (DataContext is IPatientViewModel viewModel)
            {
                var patient = new Patient
                {
                    Name = NameTextBox.Text,
                    Age = int.Parse(AgeTextBox.Text),
                    //DOB = DOBPicker.SelectedDate ?? DateTime.Now,
                    DOB = DOBPicker.SelectedDate.HasValue ? DOBPicker.SelectedDate.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"),
                    Address = AddressTextBox.Text,
                    Slot = SlotComboBox.Text,
                    //AppointmentDate = BookingDatePicker.SelectedDate ?? DateTime.Now,
                    AppointmentDate = BookingDatePicker.SelectedDate.HasValue ? BookingDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"),
                    BookingDate = DateTime.Now
                };
                viewModel.RegisterPatient(patient);

            
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
        <StackPanel x:Name="NavigationPanel" Orientation="Vertical" HorizontalAlignment="Left" VerticalAlignment="Top" Margin="10">
            <Button x:Name="btnShowRegistration" Content="Register Patient" Margin="10 20" Click="btnShowRegistration_Click"/>
            <Button x:Name="btnShowAppointmentConfirmation" Content="Appointment Confirmation" Margin="10 20" Click="btnShowAppointmentConfirmation_Click"/>
            <Button x:Name="btnShowDashboard" Content="Patient Dashboard" Margin="10 20" Click="btnShowDashboard_Click"/>
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
           NavigationPanel.Visibility = Visibility.Visible;

        }

        private void btnShowDashboard_Click(object sender, RoutedEventArgs e)
        {
            //ContentArea.Content = new Views.PatientDashboard { DataContext = _viewModel };
            var dashboard = new PatientDashboard { DataContext = _viewModel };
            dashboard.BackToMainRequested += Dasboard_BackToMainRequested;
            ContentArea.Content = dashboard;
           NavigationPanel.Visibility = Visibility.Collapsed;
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




------------------------

PatientRegistration
        var registration = new PatientRegistration();

        // Create instances of AppointmentConfirmation and PatientDashboard, passing the registration instance
        var appointmentConfirmation = new AppointmentConfirmation(registration);
        var patientDashboard = new PatientDashboard(registration);

        // Register a patient
        registration.RegisterPatient("John Doe");
    }
}





