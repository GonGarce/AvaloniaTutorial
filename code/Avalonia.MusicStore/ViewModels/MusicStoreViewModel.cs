using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.MusicStore.Models;
using Avalonia.MusicStore.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia.MusicStore.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase, IDisposable
{
    private readonly int _debounceDelay = 500; // Tiempo de espera en milisegundos para el debounce
    private readonly Subject<string?> _searchTextSubject = new(); // Stream de cambios para SearchText
    private readonly IDisposable _searchSubscription;
    public readonly Subject<AlbumViewModel> BuySubject = new();
    
    public MusicStoreViewModel()
    {
        // Nos suscribimos al Subject para manejar los cambios con debounce
        _searchSubscription = _searchTextSubject
            .Throttle(TimeSpan.FromMilliseconds(_debounceDelay)) // Aplicamos el debounce
            .Where(text => !string.IsNullOrWhiteSpace(text)) // Filtramos valores vacíos
            .DistinctUntilChanged() // Evitamos emitir valores duplicados consecutivos
            .Subscribe(Search);
    }

    [ObservableProperty]
    private string? _searchText;
    
    [ObservableProperty]
    private bool _isBusy;
    
    [ObservableProperty]
    private AlbumViewModel? _selectedAlbum;
    
    public ObservableCollection<AlbumViewModel> SearchResults  { get; } = new();
    
    private CancellationTokenSource? _cancellationTokenSource;
    
    partial void OnSearchTextChanged(string? oldValue, string? newValue)
    {
        // Cada vez que cambia SearchText, notificamos al Subject
        _searchTextSubject.OnNext(newValue);
    }

    [RelayCommand]
    private void BuyMusic()
    {
        if (SelectedAlbum is not null)
        {
            BuySubject.OnNext(SelectedAlbum);
        }
    }

    private async void Search(string? query)
    {
        IsBusy = true;
        SearchResults.Clear();
        
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        
        if (!string.IsNullOrWhiteSpace(query))
        {
            var albums = await AlbumService.SearchAsync(query);

            foreach (var album in albums)
            {
                var vm = new AlbumViewModel(album);
                SearchResults.Add(vm);
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                LoadCovers(cancellationToken);
            }
        }
        
        IsBusy = false;
    }
    
    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults.ToList())
        {
            await album.LoadCover();

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
    
    public void Dispose()
    {
        // Liberamos la suscripción y el Subject al finalizar
        _searchSubscription.Dispose();
        _searchTextSubject.Dispose();
    }
}