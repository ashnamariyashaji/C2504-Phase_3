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
