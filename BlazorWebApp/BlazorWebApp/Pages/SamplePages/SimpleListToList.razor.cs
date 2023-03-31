using Microsoft.AspNetCore.Components;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class SimpleListToList: ComponentBase
    {
        #region Inject
        //  We are now injecting our service into our class using the [Inject] attribute.
        [Inject] protected PlaylistTrackServices? PlaylistTrackService { get; set; }
        #endregion

        #region Fields
        //  list of our current available songs
        private List<ExtendedTrackSelectionView> inventory { get; set; }

        //  shopping cart
        private List<ExtendedTrackSelectionView> shoppingCart { get; set; } = new();
        #endregion

        //  page load and retrieving inventory
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            inventory = PlaylistTrackService.FetchInventory();
        }

        //  add tracks from inventory to shopping cart
        private async Task AddTrackToCart(int trackId)
        {
            ExtendedTrackSelectionView track = inventory
                .Where(x => x.TrackId == trackId).Select(x => x)
                .FirstOrDefault();
            shoppingCart.Add(track);
            inventory.Remove(track);
            await InvokeAsync(StateHasChanged);
        }

        private async Task RemoveTrackFromCart(int trackId)
        {
            ExtendedTrackSelectionView track = shoppingCart
                .Where(x => x.TrackId == trackId).Select(x => x)
                .FirstOrDefault();
            track.Quantity = 1;
            track.Total = track.Price;
            inventory.Add(track);
            inventory = inventory.OrderBy(x => x.SongName)
                .Select(x => x).ToList();
            shoppingCart.Remove(track);
            await InvokeAsync(StateHasChanged);
        }

        private async Task RefreshTotal(int trackId)
        {
            ExtendedTrackSelectionView track = shoppingCart
                .Where(x => x.TrackId == trackId).Select(x => x)
                .FirstOrDefault();
            track.Total = track.Price * track.Quantity;
            await InvokeAsync(StateHasChanged);
        }

    }
}
