using System;

namespace MinorProject.Models;

public enum SystemState
{
    Normal,
    Emergency,
    Disabled,
    Repair
}

public enum CoolingState
{
    Natural,
    PumpOnly,
    PumpAndFan
}

public class DataPoint
{
    public DateTime Timestamp { get; set; }
    public double Power { get; set; }
    public double OilTemperature { get; set; }
    public double Voltage { get; set; }
    public double Pressure { get; set; }
    public SystemState SystemState { get; set; }
    public CoolingState CoolingState { get; set; }

    public DataPoint()
    {
        Timestamp = DateTime.UtcNow;
    }

    public DataPoint(double power, double oilTemperature, double voltage, double pressure, 
                     SystemState systemState, CoolingState coolingState)
    {
        Timestamp = DateTime.UtcNow;
        Power = power;
        OilTemperature = oilTemperature;
        Voltage = voltage;
        Pressure = pressure;
        SystemState = systemState;
        CoolingState = coolingState;
    }
}