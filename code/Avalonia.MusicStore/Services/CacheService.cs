using Avalonia.MusicStore.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.Services;

public class CacheService
{
    private const string DataExtension = ".json";

    private static string CachePath(Album album) => $"./cache/{album.Artist}_{album.Title}";

    private static CacheService? _instance;

    public static CacheService Instance => _instance ??= new CacheService();
    
    public async Task SaveAsync(Album album)
    {
        if (!Directory.Exists("./cache"))
        {
            Directory.CreateDirectory("./cache");
        }

        await using var fs = File.OpenWrite(CachePath(album) + DataExtension);
        await SaveToStreamAsync(album, fs);
    }

    private async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }
}