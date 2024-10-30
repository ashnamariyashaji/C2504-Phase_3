// Models/Patient.cs
using System;

public class Patient
{
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string Slot { get; set; }  // Morning or Evening
    public DateTime BookingDate { get; set; }
}

// Models/AppointmentSlot.cs
using System;

public class AppointmentSlot
{
    public DateTime AppointmentDate { get; set; }
    public string Time { get; set; }
    public string PatientName { get; set; }
}

Here’s a foundational setup to create a WPF Patient Admission application using MVVM with user controls, event handling, and observable collections. This guide will outline the main components and some sample code for key parts.

Project Structure

1. Models: Define the data models.


2. ViewModels: Define the business logic and interactions.


3. Views: Define the UI with XAML for the main and child screens using UserControls.



Step 1: Create Model Classes

In the Models folder, create classes for Patient and AppointmentSlot.

// Models/Patient.cs
using System;

public class Patient
{
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string Slot { get; set; }  // Morning or Evening
    public DateTime BookingDate { get; set; }
}

// Models/AppointmentSlot.cs
using System;

public class AppointmentSlot
{
    public DateTime AppointmentDate { get; set; }
    public string Time { get; set; }
    public string PatientName { get; set; }
}

Step 2: Create an Interface for the ViewModel

Define an interface for the PatientViewModel.

// ViewModels/IPatientViewModel.cs
using System.Collections.ObjectModel;

public interface IPatientViewModel
{
    ObservableCollection<Patient> Patients { get; }
    ObservableCollection<AppointmentSlot> Appointments { get; }
    void AddPatient(Patient patient);
    void ConfirmAppointment(Patient patient);
}

Step 3: Implement the ViewModel

Implement PatientViewModel, inheriting from IPatientViewModel. Use an ObservableCollection to manage data and EventHandlers for notifications.

// ViewModels/PatientViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;

public class PatientViewModel : IPatientViewModel
{
    public ObservableCollection<Patient> Patients { get; private set; }
    public ObservableCollection<AppointmentSlot> Appointments { get; private set; }

    public event EventHandler<string> AppointmentConfirmed;

    public PatientViewModel()
    {
        Patients = new ObservableCollection<Patient>();
        Appointments = new ObservableCollection<AppointmentSlot>();
    }

    public void AddPatient(Patient patient)
    {
        Patients.Add(patient);
    }

    public void ConfirmAppointment(Patient patient)
    {
        var appointment = new AppointmentSlot
        {
            AppointmentDate = DateTime.Now,
            Time = patient.Slot,
            PatientName = patient.Name
        };
        Appointments.Add(appointment);

        // Trigger event
        AppointmentConfirmed?.Invoke(this, $"{patient.Name}'s appointment is confirmed on {appointment.AppointmentDate}");
    }
}

Step 4: Create the User Controls (XAML)

Patient Registration User Control

Create a PatientRegistration.xaml with fields for patient data input.

<!-- Views/PatientRegistration.xaml -->
<UserControl x:Class="YourNamespace.Views.PatientRegistration"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel>
        <TextBox x:Name="NameTextBox" PlaceholderText="Enter Name" />
        <TextBox x:Name="AgeTextBox" PlaceholderText="Enter Age" />
        <DatePicker x:Name="DOBPicker" />
        <TextBox x:Name="AddressTextBox" PlaceholderText="Enter Address" />
        <ComboBox x:Name="SlotComboBox">
            <ComboBoxItem Content="Morning" />
            <ComboBoxItem Content="Evening" />
        </ComboBox>
        <DatePicker x:Name="BookingDatePicker" />
        <Button Content="Register" Command="{Binding AddPatientCommand}" />
    </StackPanel>
</UserControl>

Patient Dashboard User Control

Displays patient details in a DataGrid.

<!-- Views/PatientDashboard.xaml -->
<UserControl x:Class="YourNamespace.Views.PatientDashboard"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <DataGrid ItemsSource="{Binding Patients}" AutoGenerateColumns="True" />
</UserControl>

Step 5: Main Window (Navigation)

The main window holds buttons to navigate between user controls.

<!-- MainWindow.xaml -->
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid>
        <StackPanel>
            <Button Content="Register Patient" Command="{Binding ShowPatientRegistrationCommand}" />
            <Button Content="View Dashboard" Command="{Binding ShowPatientDashboardCommand}" />
            <ContentControl Content="{Binding CurrentView}" />
        </StackPanel>
    </Grid>
</Window>

Step 6: Define Commands in the ViewModel

Add commands to handle navigation.

// In PatientViewModel
public ICommand ShowPatientRegistrationCommand { get; }
public ICommand ShowPatientDashboardCommand { get; }
public object CurrentView { get; private set; }

public PatientViewModel()
{
    ShowPatientRegistrationCommand = new RelayCommand(ShowPatientRegistration);
    ShowPatientDashboardCommand = new RelayCommand(ShowPatientDashboard);
    ShowPatientRegistration();
}

private void ShowPatientRegistration()
{
    CurrentView = new PatientRegistration();
    OnPropertyChanged(nameof(CurrentView));
}

private void ShowPatientDashboard()
{
    CurrentView = new PatientDashboard();
    OnPropertyChanged(nameof(CurrentView));
}

Step 7: RelayCommand for Commands

Create a RelayCommand class if not using a library.

// RelayCommand.cs
using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
    public void Execute(object parameter) => _execute();
    public event EventHandler CanExecuteChanged;
}

Step 8: Event Handling and Notification

In PatientDashboard.xaml.cs, listen for appointment confirmation notifications.

// Views/PatientDashboard.xaml.cs
public partial class PatientDashboard : UserControl
{
    public PatientDashboard(PatientViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.AppointmentConfirmed += OnAppointmentConfirmed;
    }

    private void OnAppointmentConfirmed(object sender, string message)
    {
        MessageBox.Show(message, "Appointment Confirmed");
    }
}

Step 9: Configure Data Binding and EventHandler

Bind the data to display in DataGrid after registration.

<!-- Views/PatientDashboard.xaml -->
<DataGrid ItemsSource="{Binding Patients}" AutoGenerateColumns="True" />

Step 10: Testing and Finalization

Now, launch the application, register a patient, and confirm the appointment to see data in the dashboard.

