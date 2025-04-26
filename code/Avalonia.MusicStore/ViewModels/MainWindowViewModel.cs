using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Avalonia.MusicStore.Services;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
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
            await _cacheService.SaveAsync(result.Album);
        }
    }
    
    /// <summary>
    /// Gets an instance of our own Interaction class
    /// </summary>
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
    
    public ObservableCollection<AlbumViewModel> Albums { get; set; } = [];
}