using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp
{
    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Slot { get; set; } 
        public DateTime BookingDate { get; set; }

    }
}




using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp
{
    public interface IPatient
    {
        ObservableCollection<Patient> Patients { get; }
        void RegisterPatient(Patient patient);
    }
}




using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp
{
    public class PatientViewModel : IPatient
    {
        public ObservableCollection<Patient> Patients { get; private set; }

        public PatientViewModel()
        {
            Patients = new ObservableCollection<Patient>();
        }

        public void RegisterPatient(Patient patient)
        {
            Patients.Add(patient);
            // Raise an event for notification
            OnPatientRegistered?.Invoke(this, patient);
        }

        public event EventHandler<Patient> OnPatientRegistered;
    }
}




<Window x:Class="PatientApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PatientApp"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Canvas>
        <TextBlock Text="Patient Management System" FontSize="29" Canvas.Left="200" Canvas.Top="10" FontWeight="Bold"/>
        <Button x:Name="btnRegistration" Content="Register Patient" Canvas.Left="10" Canvas.Top="80" Click="btnRegistration_Click" />
        <Button x:Name="btnAppointment" Content="Appointment Confirmation" Canvas.Left="10" Canvas.Top="130" Click="btnAppointment_Click" />
        <Button x:Name="btnDashboard" Content="Patient Dashboard" Canvas.Left="10" Canvas.Top="180" Click="btnDashboard_Click"/>
        <ContentControl x:Name="MainContent" Canvas.Left="200" Canvas.Top="60" Width="400" Height="300" HorizontalAlignment="Center" VerticalAlignment="Center">
        </ContentControl>
    </Canvas>
</Window>




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

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PatientViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new PatientViewModel();
            LoadInitialView();
        }
        private void LoadInitialView()
        {
            var registerControl = new PatientRegControl(_viewModel);
            registerControl.NavigateToAppointment += RegisterControl_NavigateToAppointment;
            MainContent.Content = registerControl;
        }
        private void RegisterControl_NavigateToAppointment()
        {
            var appointmentControl = new AppointmentConfirmationControl(_viewModel);
            appointmentControl.NavigateToDashboard += AppointmentControl_NavigateToDashboard;
            MainContent.Content = appointmentControl;
        }

        private void AppointmentControl_NavigateToDashboard()
        {
            var dashboardControl = new PatientDashboardControl(_viewModel);
           MainContent.Content = dashboardControl;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            var patientRegControl = new PatientRegControl(_viewModel);
            var mainWindow  = Window.GetWindow(this) as MainWindow;
            if(mainWindow != null)
            {
                mainWindow.MainContent.Content = patientRegControl;
            }
        }

        private void btnAppointment_Click(object sender, RoutedEventArgs e)
        {
            var appointmentConfirmationControl = new AppointmentConfirmationControl(_viewModel);
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = appointmentConfirmationControl;
            }
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            var patientDashboardControl = new PatientDashboardControl(_viewModel);
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = patientDashboardControl;
            }
        }
    }
}





<UserControl x:Class="PatientApp.PatientRegControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:PatientApp"
             mc:Ignorable="d" 
             d:DesignHeight="450" d:DesignWidth="800">
    <Canvas Width="400" Height="450">
        <TextBlock Text="Name" Canvas.Left="10" Canvas.Top="10"/>
        <TextBox x:Name="NameTextBox" Width="200" Canvas.Left="10" Canvas.Top="30"/>

        <TextBlock Text="Age" Canvas.Left="10" Canvas.Top="70"/>
        <TextBox x:Name="AgeTextBox" Width="200" Canvas.Left="10" Canvas.Top="90"/>

        <TextBlock Text="Date of Birth" Canvas.Left="10" Canvas.Top="130"/>
        <DatePicker x:Name="DOBPicker" Width="200" Canvas.Left="10" Canvas.Top="150"/>

        <TextBlock Text="Address" Canvas.Left="10" Canvas.Top="190"/>
        <TextBox x:Name="AddressTextBox" Width="200" Canvas.Left="10" Canvas.Top="210"/>

        <TextBlock Text="Slot" Canvas.Left="10" Canvas.Top="250"/>
        <ComboBox x:Name="SlotComboBox" Width="200" Canvas.Left="10" Canvas.Top="270">
            <ComboBoxItem Content="Morning"/>
            <ComboBoxItem Content="Evening"/>
        </ComboBox>

        <TextBlock Text="Booking Date" Canvas.Left="10" Canvas.Top="310"/>
        <DatePicker x:Name="BookingDate" Width="200" Canvas.Left="10" Canvas.Top="330"/>
        
        <Button x:Name="btnRegister" Content="Register" Width="100" Canvas.Left="10" Canvas.Top="380" Click="btnRegister_Click"/>
    </Canvas>
</UserControl>





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

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for PatientRegControl.xaml
    /// </summary>
    public partial class PatientRegControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event Action NavigateToAppointment;
       
        public PatientRegControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var patient = new Patient
            {
                Name = NameTextBox.Text,
                Age = int.Parse(AgeTextBox.Text),
                DateOfBirth = DOBPicker.SelectedDate.Value,
                Address = AddressTextBox.Text,
                Slot = SlotComboBox.SelectedItem.ToString(),
                BookingDate = DateTime.Now
            };
            _viewModel.RegisterPatient(patient);

            // Trigger navigation to appointment confirmation
            NavigateToAppointment?.Invoke();
        }
    }
}










<UserControl x:Class="PatientApp.AppointmentConfirmationControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:PatientApp"
             mc:Ignorable="d" 
             d:DesignHeight="450" d:DesignWidth="800">
    <StackPanel Margin="20">
        <TextBlock Text="Appointment Confirmation" FontSize="24" FontWeight="Bold" Margin="0,0,0,20"/>

        <ListBox x:Name="PatientsListBox" Height="200">
            <!-- Items will be added dynamically in the code-behind -->
        </ListBox>

        <Button x:Name="btnConfirm" Content="Confirm Appointment"  Margin="0,20,0,0" Click="btnConfirm_Click"/>
    </StackPanel>
</UserControl>





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

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for AppointmentConfirmationControl.xaml
    /// </summary>
    public partial class AppointmentConfirmationControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event Action NavigateToDashboard;
       
        
        public AppointmentConfirmationControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            LoadPatients();
        }
        private void LoadPatients()
        {
            // Assuming _viewModel.Patients is a List<Patient> that contains the registered patients
            foreach (var patient in _viewModel.Patients)
            {
                var checkBox = new CheckBox
                {
                    Content = $"{patient.Name} (Age: {patient.Age}, DOB: {patient.DateOfBirth.ToShortDateString()}, Address: {patient.Address})",
                    Tag = patient // Store the patient object in the Tag property for later use
                };
                PatientsListBox.Items.Add(checkBox);
            }
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatients = new List<Patient>();

            // Iterate through the CheckBoxes in the ListBox to find checked ones
            foreach (CheckBox checkBox in PatientsListBox.Items)
            {
                if (checkBox.IsChecked == true && checkBox.Tag is Patient patient)
                {
                    selectedPatients.Add(patient);
                }
            }
            NavigateToDashboard?.Invoke();
        }
    }
}







<UserControl x:Class="PatientApp.PatientDashboardControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:PatientApp"
             mc:Ignorable="d" 
             d:DesignHeight="450" d:DesignWidth="800">
    <StackPanel>
        <TextBlock Text="Patient Dashboard" FontSize="24" FontWeight="Bold"/>
        <DataGrid ItemsSource="{Binding Patients}" AutoGenerateColumns="True"/>
    </StackPanel>
</UserControl>




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

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for PatientDashboardControl.xaml
    /// </summary>
    public partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel _viewModel;
        
        public PatientDashboardControl(PatientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}
