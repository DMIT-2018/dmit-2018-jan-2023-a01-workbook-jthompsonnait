#nullable disable
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages
{
    public partial class Index
    {
        #region Injections
        //  We are now injecting our service into our class using the [Inject] attribute.  Before we would have used the page constructor to add it,
        //  public Index(PlaylistTrackService playlistTrackService)
        //{
        //  _playlistTrackService = playlistTrackService
        // }
        [Inject]
        protected PlaylistTrackService? PlaylistTrackService { get; set; }
        #endregion

        //  Working Version View
        #region Fields
        private WorkingVersionView workingVersionView = new();
        #endregion

        //  method for retrieving our version information
        private async Task GetDatabase()
        {
            workingVersionView = PlaylistTrackService.GetWorkingVersion();
            //  waiting for the data to be retrieve before we update the label on the page
            await InvokeAsync(StateHasChanged);
        }
    }
}
