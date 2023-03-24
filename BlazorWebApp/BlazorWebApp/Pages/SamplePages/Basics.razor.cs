using Microsoft.AspNetCore.Components;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class Basics
    {
        //  We are now injecting our service into our class using the [Inject] attribute
        //  Before we would have used the page constructor to add it.
        //  public Basics(PlaylistTrackService playlistTrackService)
        //  {
        //      _playlistTrackService = playlistTrackService;
        //  }
        [Inject]
        protected PlaylistTrackServices? PlaylistTrackServices { get; set; }

        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        #region Fields
        private string myName = string.Empty;
        private int oddEven;
        private WorkingVersionView workingVersion = new();
        #endregion

        //  Method invoked when the component is ready to start having
        //      received it initial parameters from its parent in the render
        //      tree.
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Random rnd = new Random();
            int oddEven = rnd.Next(0, 25);
            if (oddEven % 2 == 0)
            {
                myName = $"James is even {oddEven}";
            }
            else
            {
                myName = null;
            }
        }
        private void RandomValue()
        {
            Random rnd = new Random();
            oddEven = rnd.Next(0, 25);
            if (oddEven % 2 == 0)
            {
                myName = $"James is even {oddEven}";
            }
            else
            {
                myName = null;
            }
        }

        private async Task GetDatabase()
        {
            workingVersion = PlaylistTrackServices.GetWorkingVersion();
            //  wait for the data to be retrieved before we update the label on the page
            await InvokeAsync(StateHasChanged);
        }
    }
}
