using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinorProject.Models;

namespace MinorProject.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public TemplatePageTViewModel InfoT1Vm { get; }
    public TemplatePageTViewModel InfoT2Vm { get; }
    public MainViewModel TableVm { get; }

    [ObservableProperty] private string _themeIcon = "🌙";

    public MainWindowViewModel()
    {
        var twin1 = new DigitalTwinModel("T1", "Трансформатор Т1");
        var twin2 = new DigitalTwinModel("T2", "Трансформатор Т2");

        InfoT1Vm = new TemplatePageTViewModel(twin1);
        InfoT2Vm = new TemplatePageTViewModel(twin2);
        TableVm = new MainViewModel(InfoT1Vm, InfoT2Vm);

        UpdateThemeIcon();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        if (Application.Current is null) return;

        var current = Application.Current.RequestedThemeVariant;
        Application.Current.RequestedThemeVariant =
            current == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;

        UpdateThemeIcon();
    }

    private void UpdateThemeIcon()
    {
        if (Application.Current is null) return;
        ThemeIcon = Application.Current.RequestedThemeVariant == ThemeVariant.Dark ? "☀" : "🌙";
    }
}
