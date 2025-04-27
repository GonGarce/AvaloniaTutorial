using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.MusicStore.Models;

namespace Avalonia.MusicStore.Services;

public class CoverService
{
    private static CoverService? _instance;

    public static CoverService Instance => _instance ??= new CoverService();
    
    private static readonly HttpClient SHttpClient = new();
    private readonly CacheService _cacheService = CacheService.Instance;

    public async Task<Stream> LoadCoverBitmapAsync(Album album)
    {
        var cacheImage = _cacheService.LoadImage(album);
        if (cacheImage != null)
        {
            return cacheImage;
        }
        var data = await SHttpClient.GetByteArrayAsync(album.CoverUrl);
        return new MemoryStream(data);
    } 
}