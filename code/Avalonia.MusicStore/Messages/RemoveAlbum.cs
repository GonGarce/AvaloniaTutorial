using Avalonia.MusicStore.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Avalonia.MusicStore.Messages;

public class RemoveAlbum(AlbumViewModel value) : ValueChangedMessage<AlbumViewModel>(value);