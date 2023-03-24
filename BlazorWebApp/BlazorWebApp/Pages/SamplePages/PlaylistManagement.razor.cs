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

        #endregion
    }
}
