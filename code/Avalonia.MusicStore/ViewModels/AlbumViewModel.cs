using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Models;
using Avalonia.MusicStore.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.MusicStore.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
    public Album Album { get; }
    
    private readonly CacheService _cacheService = CacheService.Instance;
    
    public AlbumViewModel(Album album)
    {
        Album = album;
    }

    public string Artist => Album.Artist;

    public string Title => Album.Title;
    
    [ObservableProperty]
    private Bitmap? _cover;
}