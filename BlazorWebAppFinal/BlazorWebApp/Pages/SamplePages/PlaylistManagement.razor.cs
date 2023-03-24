#nullable disable
using Microsoft.AspNetCore.Components;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.Paginator;
using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages;

public partial class PlaylistManagement
{
    #region Inject

    //  We are now injecting our service into our class using the [Inject] attribute.
    [Inject] protected PlaylistTrackService? PlaylistTrackService { get; set; }

    #endregion

    #region Fields

    private string searchPattern { get; set; } = "Deep";

    //  preset the search type so that the radio button has a default of "Artist"
    private string searchType { get; set; } = "Artist";
    private string playlistName { get; set; } = "HansenB1";
    private int playlistID { get; set; } = 13;
    private string userName { get; set; } = "HansenB";
    private string feedback { get; set; }

    #endregion

    protected List<PlaylistTrackView> Playlists { get; set; } = new();

    #region Paginator
    //  Desired CurrentPage size
    private const int PAGE_SIZE = 10;

    //  sort column used with the paginator
    protected string SortField { get; set; } = "Owner";

    //  sort direction for the paginator
    protected string Direction { get; set; } = "desc";

    //  current page for the paginator
    protected int CurrentPage { get; set; } = 1;

    //  paginator collection of track selection view
    protected PagedResult<TrackSelectionView> PaginatorTrackSelection { get; set; } = new();

    private async void Sort(string column)
    {
        Direction = SortField == column ? Direction == "asc" ? "desc" : "asc" : "asc";
        SortField = column;
        if (!string.IsNullOrWhiteSpace(searchPattern))
        {
            await FetchArtistOrAlbumTracks();
        }
    }

    // sets css class to display up and down arrows
    private string GetSortColumn(string x)
    {
        return x == SortField ? Direction == "desc" ? "desc" : "asc" : "";
    }

    #endregion

    private async Task FetchArtistOrAlbumTracks()
    {
        // we would normal check if the user has enter in a value into the search pattern but we will let the service do the error checking.
        PaginatorTrackSelection = await PlaylistTrackService.FetchArtistOrAlbumTracks(searchType, searchPattern, CurrentPage, PAGE_SIZE, SortField, Direction);        //Note that the following line is necessary because otherwise
        //Blazor would not recognize the state change and not refresh the UI
        await InvokeAsync(StateHasChanged);
    }


    private async Task FetchPlaylist()
    {
        Playlists = await PlaylistTrackService.FetchPlaylist(userName, playlistName);
        //Note that the following line is necessary because otherwise// Blazor would not recognize the state change and not refresh the UI
        await InvokeAsync(StateHasChanged);
    }


    private async Task AddTrackToPlaylist(int trackId)
    {
        try
        {
            PlaylistTrackService.AddTrack(userName, playlistName, trackId);
            await FetchPlaylist();
        }
        #region catch all exceptions
        catch (AggregateException ex)
        {
            foreach (var error in ex.InnerExceptions)
            {
                feedback = error.Message;
            }
        }

        catch (ArgumentNullException ex)
        {
            feedback = GetInnerException(ex).Message;
        }

        catch (Exception ex)
        {
            feedback = GetInnerException(ex).Message;
        }
        #endregion
    }



    private async Task RemoveTracks()
    {
        try
        {
            List<int> removeTracks = new();
            foreach (var playlist in Playlists)
            {
                if (playlist.Remove)
                {
                    removeTracks.Add(playlist.TrackId);
                }
            }
            PlaylistTrackService.RemoveTracks(playlistID, removeTracks);
            await FetchPlaylist();
        }
        #region catch all exceptions
        catch (AggregateException ex)
        {
            foreach (var error in ex.InnerExceptions)
            {
                feedback = error.Message;
            }
        }

        catch (ArgumentNullException ex)
        {
            feedback = GetInnerException(ex).Message;
        }

        catch (Exception ex)
        {
            feedback = GetInnerException(ex).Message;
        }
        #endregion
    }

private async Task ReorderTracks()
{
    try
    {
        List<MoveTrackView> moveTracks = new();
        foreach (var playlist in Playlists)
        {
            if (playlist.NewTrackNumber > 0)
            {
                moveTracks.Add(new MoveTrackView(){ TrackId = playlist.TrackId, TrackNumber = playlist.NewTrackNumber });
            }
        }
        PlaylistTrackService.MoveTrack(playlistID, moveTracks);
        await FetchPlaylist();
    }
    #region catch all exceptions
    catch (AggregateException ex)
    {
        foreach (var error in ex.InnerExceptions)
        {
            feedback = error.Message;
        }
    }

    catch (ArgumentNullException ex)
    {
        feedback = GetInnerException(ex).Message;
    }

    catch (Exception ex)
    {
        feedback = GetInnerException(ex).Message;
    }
    #endregion
}
    #region Methods
    private Exception GetInnerException(Exception ex)
    {
        while (ex.InnerException != null)
            ex = ex.InnerException;
        return ex;
    }
    #endregion

}