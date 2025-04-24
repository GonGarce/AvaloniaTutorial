using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.MusicStore.Models;

namespace Avalonia.MusicStore.Services;

public class CoverCacheService
{
    private static HttpClient s_httpClient = new();

    public static async Task<Stream> LoadCoverAsync(Album album)
    {
        if (File.Exists(album.CachePath + ".bmp"))
        {
            return File.OpenRead(album.CachePath + ".bmp");
        }
        else
        {
            var data = await s_httpClient.GetByteArrayAsync(album.CoverUrl);
            return new MemoryStream(data);
        }
    }
}