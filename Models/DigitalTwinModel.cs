using CommunityToolkit.Mvvm.ComponentModel;

namespace MinorProject.Models;

public partial class DigitalTwinModel : ObservableObject
{
    public string Id { get; }

    [ObservableProperty] private string _name;

    public DigitalTwinModel(string id, string name)
    {
        Id = id;
        _name = name;
    }

    [ObservableProperty] private double _power;
    [ObservableProperty] private double _oilTemperature;
    [ObservableProperty] private double _voltage;
    [ObservableProperty] private double _pressure;
    [ObservableProperty] private string _statusText = "Статус: НОРМА";
    [ObservableProperty] private string _coolingText = "Охлаждение: Отключено";
    [ObservableProperty] private bool _isEmergency;
    [ObservableProperty] private bool _isPumpOn;
    [ObservableProperty] private bool _isFanOn;
    [ObservableProperty] private SystemState _systemState = SystemState.Normal;
    [ObservableProperty] private CoolingState _coolingState = CoolingState.Natural;
}
