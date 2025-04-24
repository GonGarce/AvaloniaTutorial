using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.MusicStore.Models;
using Avalonia.MusicStore.ViewModels;

namespace Avalonia.MusicStore.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    // Stores a reference to the disposable in order to clean it up if needed
    IDisposable? _dialogInteractionDisposable;

    protected override void OnDataContextChanged(EventArgs e)
    {
        // Dispose any old handler
        _dialogInteractionDisposable?.Dispose();

        if (DataContext is MainWindowViewModel vm)
        {
            // register the interaction handler
            _dialogInteractionDisposable =
                vm.ShowDialog.RegisterHandler(InteractionHandler);
        }

        base.OnDataContextChanged(e);
    }
    
    private async Task<AlbumViewModel?> InteractionHandler(MusicStoreViewModel vm)
    {
        // Get a reference to our TopLevel (in our case the parent Window)
        //var topLevel = TopLevel.GetTopLevel(this);

        var dialog = new MusicStoreWindow();
        dialog.DataContext = vm;

        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        
        return result;
    }
}