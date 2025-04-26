using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.MusicStore.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Avalonia.MusicStore.ViewModels;

public partial class MusicStoreViewModel: ViewModelBase
{
    private const int DebounceDelay = 500; // Typing wait time
    private readonly Subject<string?> _searchTextSubject = new(); // Search term changes stream
    public readonly Subject<AlbumViewModel> BuySubject = new();

    public MusicStoreViewModel()
    {
        _searchTextSubject
            .Throttle(TimeSpan.FromMilliseconds(DebounceDelay)) // Wait to stop typing
            //.Where(text => !string.IsNullOrWhiteSpace(text)) // Exclude empty searches
            .DistinctUntilChanged() // Only search when term has changed
            .Subscribe(Search);
    }
    
    [ObservableProperty]
    private string? _searchText;
    
    [ObservableProperty]
    private bool _isBusy;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuyMusicCommand))]
    private AlbumViewModel? _selectedAlbum;
    
    [ObservableProperty]
    private IEnumerable<AlbumViewModel> _searchResults  = [];

    private bool CanBuy => SelectedAlbum != null;
    
    [RelayCommand(CanExecute = nameof(CanBuy))]
    private void BuyMusic()
    {
        if (SelectedAlbum is not null)
        {
            BuySubject.OnNext(SelectedAlbum);
        }
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
        
        if (!string.IsNullOrWhiteSpace(query))
        {
            var albums = await AlbumService.SearchAsync(query);
            SearchResults = albums.Select(album => new AlbumViewModel(album));
        }
        
        IsBusy = false;
    }
}