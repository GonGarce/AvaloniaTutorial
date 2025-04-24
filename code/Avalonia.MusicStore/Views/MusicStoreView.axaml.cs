using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.MusicStore.ViewModels;

namespace Avalonia.MusicStore.Views;

public partial class MusicStoreView : UserControl
{
    public MusicStoreView()
    {
        InitializeComponent();
    }
    
    private MusicStoreViewModel? _vm;

    protected override void OnDataContextChanged(EventArgs e)
    {
        Debug.Print("MusicStoreView:OnDataContextChanged");
        _vm?.Dispose();

        if (DataContext is MusicStoreViewModel vm)
        {
            _vm = vm;
        }

        base.OnDataContextChanged(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _vm?.Dispose();
        base.OnUnloaded(e);
    }
}