using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.MusicStore.Models;
using iTunesSearch.Library;

namespace Avalonia.MusicStore.Services;

public class AlbumService
{
    private static iTunesSearchManager _searchManager = new();
    private static HttpClient s_httpClient = new();
    
    public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
    {
        Debug.Print("SearchAsync");
        var query = await _searchManager.GetAlbumsAsync(searchTerm)
            .ConfigureAwait(false);
                
        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName, 
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
}