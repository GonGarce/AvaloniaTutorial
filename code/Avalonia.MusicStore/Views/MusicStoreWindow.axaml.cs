using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.MusicStore.ViewModels;

namespace Avalonia.MusicStore.Views;

public partial class MusicStoreWindow : Window
{
    public MusicStoreWindow()
    {
        InitializeComponent();
    }
    
    private IDisposable? _subscription;

    protected override void OnDataContextChanged(EventArgs e)
    {
        Debug.Print("MusicStoreWindow:OnDataContextChanged");
        _subscription?.Dispose();

        if (DataContext is MusicStoreViewModel vm)
        {
            _subscription = vm.BuySubject.Subscribe(Close);
        }

        base.OnDataContextChanged(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _subscription?.Dispose();
        base.OnUnloaded(e);
    }
}