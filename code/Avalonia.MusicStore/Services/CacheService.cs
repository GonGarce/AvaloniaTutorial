using System.Collections.Generic;
using Avalonia.MusicStore.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.Services;

public class CacheService
{
    private const string DataExtension = ".json";
    private const string ImageExtension = ".bmp";

    private static string CachePath(Album album) => $"./cache/{album.Artist}_{album.Title}";

    private static CacheService? _instance;

    public static CacheService Instance => _instance ??= new CacheService();

    public async Task SaveAsync(Album album)
    {
        EnsureDirectoryExists();

        await using var fs = File.OpenWrite(CachePath(album) + DataExtension);
        await SaveToStreamAsync(album, fs);
    }

    public async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        EnsureDirectoryExists();

        var results = new List<Album>();
        foreach (var file in Directory.EnumerateFiles("./cache"))
        {
            if (!string.Equals(new DirectoryInfo(file).Extension, ".json")) continue;
            
            await using var fs = File.OpenRead(file);
            results.Add(await LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
    
    public Stream SaveCoverBitmapStream(Album album)
    {
        return File.OpenWrite(CachePath(album) + ImageExtension);
    }
    
    public FileStream? LoadImage(Album album)
    {
        return File.Exists(CachePath(album)+ ".bmp") ? File.OpenRead(CachePath(album) + ".bmp") : null;
    }
    
    private async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists("./cache")) Directory.CreateDirectory("./cache");
    }

    private async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }
}