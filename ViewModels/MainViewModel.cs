using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MinorProject.Models;

namespace MinorProject.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public TransformerModel T1 { get; } = new TransformerModel();
    public TransformerModel T2 { get; } = new TransformerModel();

    public MainViewModel()
    {
        T1.Voltage = "220 кВ";
        T1.OilTemperature = "45 °C";
        T1.Pressure = "Норма";
        T1.Power = "15 МВт";

        T2.Voltage = "110 кВ";
        T2.OilTemperature = "50 °C";
        T2.Pressure = "Норма";
        T2.Power = "10 МВт";
        
        SimulateLiveUpdates();
    }
    
    private async void SimulateLiveUpdates()
    {
        while (true)
        {
            await Task.Delay(2000);
            T1.OilTemperature = $"{40 + System.Random.Shared.Next(0, 15)} °C"; // Меняем температуру
            T2.OilTemperature = $"{40 + System.Random.Shared.Next(0, 15)} °C"; // Меняем температуру
        }
    }
}