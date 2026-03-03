using CommunityToolkit.Mvvm.ComponentModel;

namespace MinorProject.Models;

public partial class TransformerModel : ObservableObject
{
    [ObservableProperty]
    private string _voltage = "0";

    [ObservableProperty]
    private string _oilTemperature = "0";

    [ObservableProperty]
    private string _pressure = "0";

    [ObservableProperty]
    private string _power = "0";
}