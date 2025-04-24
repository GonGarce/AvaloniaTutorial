using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.MusicStore.Models;
using CommunityToolkit.Mvvm.Input;
using System.Reactive.Concurrency;
using Avalonia.Threading;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        Dispatcher.UIThread.Post(LoadAlbums);
    }

    [RelayCommand]
    private async Task BuyMusicAsync()
    {
        var store = new MusicStoreViewModel();

        var result = await ShowDialog.HandleAsync(store);
        if (result != null)
        {
            Albums.Add(result);
            await result.SaveToDiskAsync();
        }
    }
    
    /// <summary>
    /// Gets an instance of our own Interaction class
    /// </summary>
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
    
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();
    
    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        foreach (var album in Albums.ToList())
        {
            await album.LoadCover();
        }
    }
}