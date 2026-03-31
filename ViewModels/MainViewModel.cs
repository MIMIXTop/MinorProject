using System;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinorProject.Models;
using MinorProject.Services;

namespace MinorProject.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public TransformerModel T1 { get; } = new();
    public TransformerModel T2 { get; } = new();

    private readonly TemplatePageTViewModel _infoT1;
    private readonly TemplatePageTViewModel _infoT2;
    private readonly LoadBalancerService _balancer = new();

    // --- Общий вход мощности ---
    [ObservableProperty] private double _totalDemand = 50;
    [ObservableProperty] private bool _isAutoBalancing = true;

    // --- Настройки балансировщика ---
    [ObservableProperty] private double _balancerMaxT1 = 80;
    [ObservableProperty] private double _balancerMaxT2 = 80;
    [ObservableProperty] private double _t2Threshold = 60;
    [ObservableProperty] private double _maxTotalDemand = 160;
    [ObservableProperty] private DistributionStrategy _distributionStrategy = DistributionStrategy.T1First;

    // --- Текстовые представления стратегии ---
    public string StrategyText => DistributionStrategy switch
    {
        DistributionStrategy.T1First => "Т1 → Т2 (приоритет Т1)",
        DistributionStrategy.T2First => "Т2 → Т1 (приоритет Т2)",
        DistributionStrategy.Proportional => "Пропорционально",
        _ => "—"
    };

    // --- Распределение ---
    [ObservableProperty] private string _t1AllocatedText = "0 МВА";
    [ObservableProperty] private string _t2AllocatedText = "0 МВА";
    [ObservableProperty] private string _distributionText = string.Empty;
    [ObservableProperty] private string _failoverWarning = string.Empty;

    // --- Суммарные показатели ---
    [ObservableProperty] private string _totalPower = "—";
    [ObservableProperty] private string _avgTemperature = "—";

    // --- Цветовая индикация ---
    [ObservableProperty] private IBrush _t1StatusBrush = Brushes.Green;
    [ObservableProperty] private IBrush _t2StatusBrush = Brushes.Green;
    [ObservableProperty] private string _t1StatusIcon = "●";
    [ObservableProperty] private string _t2StatusIcon = "●";

    // --- Настройки для UI (TextBox) ---
    [ObservableProperty] private string _editBalancerMaxT1 = "80";
    [ObservableProperty] private string _editBalancerMaxT2 = "80";
    [ObservableProperty] private string _editT2Threshold = "60";
    [ObservableProperty] private string _editMaxTotalDemand = "160";
    [ObservableProperty] private string _balancerSettingsError = string.Empty;
    [ObservableProperty] private string _balancerSettingsSuccess = string.Empty;

    public MainViewModel(TemplatePageTViewModel infoT1, TemplatePageTViewModel infoT2)
    {
        _infoT1 = infoT1;
        _infoT2 = infoT2;

        SimulateLiveUpdates();
    }

    private async void SimulateLiveUpdates()
    {
        while (true)
        {
            await Task.Delay(500);

            Dispatcher.UIThread.Post(() =>
            {
                // --- Балансировка ---
                if (IsAutoBalancing)
                {
                    var (t1p, t2p) = _balancer.Distribute(
                        TotalDemand,
                        _infoT1.IsAvailable, _infoT2.IsAvailable,
                        _infoT1.IsManualMode, _infoT2.IsManualMode,
                        _infoT1.OperatorPower, _infoT2.OperatorPower,
                        BalancerMaxT1, BalancerMaxT2,
                        T2Threshold,
                        DistributionStrategy
                    );

                    if (!_infoT1.IsManualMode)
                        _infoT1.OperatorPower = t1p;
                    if (!_infoT2.IsManualMode)
                        _infoT2.OperatorPower = t2p;

                    _infoT1.SetAllocatedPower(t1p);
                    _infoT2.SetAllocatedPower(t2p);

                    T1AllocatedText = $"{t1p:F1} МВА";
                    T2AllocatedText = $"{t2p:F1} МВА";
                    DistributionText = $"Т1: {t1p:F1} / Т2: {t2p:F1} МВА";

                    // Предупреждение при failover
                    bool t1Down = !_infoT1.IsAvailable;
                    bool t2Down = !_infoT2.IsAvailable;
                    if (t1Down && !t2Down)
                        FailoverWarning = "⚠ Т1 недоступен — вся нагрузка на Т2";
                    else if (t2Down && !t1Down)
                        FailoverWarning = "⚠ Т2 недоступен — вся нагрузка на Т1";
                    else if (t1Down && t2Down)
                        FailoverWarning = "⚠ Оба трансформатора недоступны!";
                    else
                        FailoverWarning = string.Empty;
                }

                // --- Т1 ---
                T1.Power = $"{_infoT1.CurrentPower:F1} МВА";
                T1.OilTemperature = $"{_infoT1.CurrentOilTemp:F1} °C";
                T1.Voltage = $"{_infoT1.CurrentVoltage:F0} В";
                T1.Pressure = $"{_infoT1.CurrentPressure:F1} атм";

                if (_infoT1.Twin.IsEmergency)
                    T1StatusBrush = Brushes.Red;
                else if (_infoT1.Twin.SystemState == SystemState.Disabled)
                    T1StatusBrush = Brushes.Gray;
                else
                    T1StatusBrush = Brushes.Green;

                T1StatusIcon = _infoT1.Twin.IsEmergency ? "⚠" : "●";

                // --- Т2 ---
                T2.Power = $"{_infoT2.CurrentPower:F1} МВА";
                T2.OilTemperature = $"{_infoT2.CurrentOilTemp:F1} °C";
                T2.Voltage = $"{_infoT2.CurrentVoltage:F0} В";
                T2.Pressure = $"{_infoT2.CurrentPressure:F1} атм";

                if (_infoT2.Twin.IsEmergency)
                    T2StatusBrush = Brushes.Red;
                else if (_infoT2.Twin.SystemState == SystemState.Disabled)
                    T2StatusBrush = Brushes.Gray;
                else
                    T2StatusBrush = Brushes.Green;

                T2StatusIcon = _infoT2.Twin.IsEmergency ? "⚠" : "●";

                // --- Суммарные данные ---
                double p1 = _infoT1.CurrentPower;
                double p2 = _infoT2.CurrentPower;
                double t1 = _infoT1.CurrentOilTemp;
                double t2 = _infoT2.CurrentOilTemp;

                TotalPower = $"{p1 + p2:F1} МВА";
                AvgTemperature = $"{(t1 + t2) / 2:F1} °C";
            });
        }
    }

    // --- Команды ---
    [RelayCommand]
    private void SetStrategyT1First() => SetStrategy(DistributionStrategy.T1First);

    [RelayCommand]
    private void SetStrategyT2First() => SetStrategy(DistributionStrategy.T2First);

    [RelayCommand]
    private void SetStrategyProportional() => SetStrategy(DistributionStrategy.Proportional);

    private void SetStrategy(DistributionStrategy s)
    {
        DistributionStrategy = s;
        OnPropertyChanged(nameof(StrategyText));
    }

    [RelayCommand]
    private void ApplyBalancerSettings()
    {
        BalancerSettingsError = string.Empty;
        BalancerSettingsSuccess = string.Empty;

        if (!double.TryParse(EditBalancerMaxT1, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var maxT1) || maxT1 <= 0)
        {
            BalancerSettingsError = "Неверный лимит Т1";
            return;
        }
        if (!double.TryParse(EditBalancerMaxT2, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var maxT2) || maxT2 <= 0)
        {
            BalancerSettingsError = "Неверный лимит Т2";
            return;
        }
        if (!double.TryParse(EditT2Threshold, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var threshold) || threshold < 0)
        {
            BalancerSettingsError = "Неверный порог подключения Т2";
            return;
        }
        if (!double.TryParse(EditMaxTotalDemand, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var maxDemand) || maxDemand <= 0)
        {
            BalancerSettingsError = "Неверный макс. лимит потребления";
            return;
        }

        BalancerMaxT1 = maxT1;
        BalancerMaxT2 = maxT2;
        T2Threshold = threshold;
        MaxTotalDemand = maxDemand;

        BalancerSettingsSuccess = "Настройки балансировщика применены";
    }
}
