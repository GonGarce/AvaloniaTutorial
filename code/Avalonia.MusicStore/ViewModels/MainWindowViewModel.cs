using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.MusicStore.Messages;
using CommunityToolkit.Mvvm.Input;
using Avalonia.MusicStore.Services;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        Dispatcher.UIThread.Post(LoadAlbums);

        WeakReferenceMessenger.Default.Register<RemoveAlbum>(this,
            (_, message) => { Albums.Remove(message.Value); });
    }

    private readonly CacheService _cacheService = CacheService.Instance;

    [RelayCommand]
    private async Task BuyMusic()
    {
        var store = new MusicStoreViewModel();

        var result = await ShowDialog.HandleAsync(store);
        if (result != null)
        {
            Albums.Add(result);
            result.ShowDelete = true;
            await result.SaveToDiskAsync();
        }
    }

    /// <summary>
    /// Gets an instance of our own Interaction class
    /// </summary>
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }

    public ObservableCollection<AlbumViewModel> Albums { get; set; } = [];

    private async void LoadAlbums()
    {
        var albums = await _cacheService.LoadCachedAsync();
        foreach (var album in albums)
        {
            AlbumViewModel vm = new AlbumViewModel(album);
            vm.ShowDelete = true;
            Albums.Add(vm);
            vm.LoadCover();
        }
    }
}