#nullable disable
using Microsoft.AspNetCore.Components;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.Paginator;
using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class PlaylistManagement
    {
        #region Inject

        [Inject]
        protected PlaylistTrackServices? PlaylistTrackServices { get; set; }
        #endregion

        #region Fields

        //  search pattern
        private string searchPattern { get; set; } = "Deep";
        // search type
        private string searchType { get; set; } = "Artist";
        //  play list name
        private string playlistName { get; set; } = "HansenB1";
        //  playlist Id
        private int playlistId { get; set; } = 13;
        //user name
        private string userName { get; set; } = "HansenB";

        //  feed back 
        private string feedBack { get; set; }

        #endregion

        protected List<PlaylistTrackView> Playlists { get; set; } = new();

        #region Paginator

        //  desired current page size
        private const int PAGE_SIZE = 10;
        //  sort column 
        protected string SortField { get; set; } = "Owner";
        //  sort direction
        protected string Direction { get; set; } = "desc";
        //  current page
        protected int CurrentPage { get; set; } = 1;

        //paginator collections of track selection view
        protected PagedResult<TrackSelectionView> PaginatorTrackSelection { get; set; } = new();

        //  method for sorting
        private async void Sort(string column)
        {
            Direction = SortField == column ? Direction == "asc" ? "desc" : "asc" : "asc";
            SortField = column;
            if (!string.IsNullOrWhiteSpace(searchPattern))
            {
                await FetchArtistOrAlbumTracks();
            }
        }

        //  set css class to display up and down arrows
        private string GetSortColumn(string x)
        {
            return x == SortField ? Direction == "desc" ? "desc" : "asc" : "";
        }
        #endregion

        private async Task FetchArtistOrAlbumTracks()
        {
            try
            {

                //  with Blazor, we would normally check if the user has enter in a value
                //      into the search pattern, but we will let the service do the error checking
                PaginatorTrackSelection = await PlaylistTrackServices.FetchArtistOrAlbumTracks(searchType,
                    searchPattern, CurrentPage, PAGE_SIZE, SortField,
                    Direction); //Note that the following line is necessary because otherwise
                //  Blazor needs to be refresh as it is not aware of the updated list
                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {

                feedBack = GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //having collected a number of errors
                //	each error should be dumped to a separate line
                foreach (var error in ex.InnerExceptions)
                {
                    feedBack = error.Message;
                }
            }
            catch (Exception ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
        }

        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        private async Task FetchPlaylist()
        {
            try
            {
                Playlists = await PlaylistTrackServices.FetchPlaylist
                                    (userName, playlistName);
                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {

                feedBack = GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //having collected a number of errors
                //	each error should be dumped to a separate line
                foreach (var error in ex.InnerExceptions)
                {
                    feedBack = error.Message;
                }
            }
            catch (Exception ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
        }

        private void RemoveTracks()
        {
            try
            {

            }
            catch (ArgumentNullException ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {

                feedBack = GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //having collected a number of errors
                //	each error should be dumped to a separate line
                foreach (var error in ex.InnerExceptions)
                {
                    feedBack = error.Message;
                }
            }
            catch (Exception ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
        }

        private void ReorderTracks()
        {
            try
            {

            }
            catch (ArgumentNullException ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {

                feedBack = GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //having collected a number of errors
                //	each error should be dumped to a separate line
                foreach (var error in ex.InnerExceptions)
                {
                    feedBack = error.Message;
                }
            }
            catch (Exception ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
        }



        private async Task AddTrackToPlaylist(int trackId)
        {
            try
            {
                PlaylistTrackServices.AddTrack(userName, playlistName, trackId);
                await FetchPlaylist();
            }
            catch (ArgumentNullException ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {

                feedBack = GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //having collected a number of errors
                //	each error should be dumped to a separate line
                foreach (var error in ex.InnerExceptions)
                {
                    feedBack = error.Message;
                }
            }
            catch (Exception ex)
            {
                feedBack = GetInnerException(ex).Message;
            }
        }
    }
}
