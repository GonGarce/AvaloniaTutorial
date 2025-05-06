using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Avalonia.MusicStore.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia.MusicStore.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase, IDisposable
{
    private const int DebounceDelay = 500; // Typing wait time
    private readonly Subject<string?> _searchTextSubject = new(); // Search term changes stream
    public readonly Subject<AlbumViewModel> BuySubject = new();
    private readonly IDisposable _searchSubscription;

    public MusicStoreViewModel()
    {
        _searchSubscription = _searchTextSubject
            .Throttle(TimeSpan.FromMilliseconds(DebounceDelay)) // Wait to stop typing
            //.Where(text => !string.IsNullOrWhiteSpace(text)) // Exclude empty searches
            .DistinctUntilChanged() // Only search when term has changed
            .Subscribe(Search);
    }

    private CancellationTokenSource? _cancellationTokenSource;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearSearchCommand))]
    private string? _searchText;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuyMusicCommand))]
    private AlbumViewModel? _selectedAlbum;

    [ObservableProperty]
    private AlbumViewModel[] _searchResults = [];

    private bool CanBuy => SelectedAlbum != null;

    [RelayCommand(CanExecute = nameof(CanBuy))]
    private void BuyMusic()
    {
        if (SelectedAlbum is not null)
        {
            BuySubject.OnNext(SelectedAlbum);
        }
    }
    
    private bool CanClear => !string.IsNullOrWhiteSpace(SearchText);
    
    [RelayCommand(CanExecute = nameof(CanClear))]
    private void ClearSearch()
    {
        SearchText = string.Empty;
        SearchResults = [];
        SelectedAlbum = null;
    }

    partial void OnSearchTextChanged(string? value)
    {
        // Every time search text changes notify the subject
        _searchTextSubject.OnNext(value);
    }

    private async void Search(string? query)
    {
        IsBusy = true;
        SearchResults = [];

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        if (!string.IsNullOrWhiteSpace(query))
        {
            var albums = await AlbumService.SearchAsync(query);
            SearchResults = albums.Select(album => new AlbumViewModel(album)).ToArray();

            if (!cancellationToken.IsCancellationRequested)
            {
                LoadCovers(cancellationToken);
            }
        }

        IsBusy = false;
    }

    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults)
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