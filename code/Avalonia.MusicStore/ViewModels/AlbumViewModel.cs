using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Messages;
using Avalonia.MusicStore.Models;
using Avalonia.MusicStore.Services;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.MusicStore.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
    private Album Album { get; }
    
    private readonly CacheService _cacheService = CacheService.Instance;
    private readonly CoverService _coverService = CoverService.Instance;
    
    public AlbumViewModel(Album album)
    {
        Album = album;
    }

    public string Artist => Album.Artist;

    public string Title => Album.Title;
    
    [ObservableProperty]
    private Bitmap? _cover;
    
    [ObservableProperty]
    private bool _showDelete;

    [RelayCommand]
    private void Delete()
    {
        _cacheService.Remove(Album);
        WeakReferenceMessenger.Default.Send(new RemoveAlbum(this));
    }
    
    public async Task LoadCover()
    {
        await using var imageStream = await _coverService.LoadCoverBitmapAsync(Album);
        Cover = Bitmap.DecodeToWidth(imageStream, 400);;
    }
    
    public async Task SaveToDiskAsync()
    {
        await _cacheService.SaveAsync(Album);

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using var fs = _cacheService.SaveCoverBitmapStream(Album);
                bitmap.Save(fs);
            });
        }
    }
}