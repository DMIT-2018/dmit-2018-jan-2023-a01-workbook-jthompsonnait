namespace PlaylistManagementSystem.ViewModels
{
	public class TrackSelectionView
	{
		public int TrackId { get; set; }
		public string SongName { get; set; }
		public string AlbumTitle { get; set; }
		public string ArtistName { get; set; }
		public int Milliseconds { get; set; }
		public decimal Price { get; set; }
        public string Length
        {
            get { return $"{(int)Milliseconds / 60 / 1000}:{Milliseconds / 1000 % 60}"; }
        }
    }
}