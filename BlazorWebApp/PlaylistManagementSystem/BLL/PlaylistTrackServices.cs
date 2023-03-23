#nullable disable
using PlaylistManagementSystem.DAL;
using PlaylistManagementSystem.ViewModels;

namespace PlaylistManagementSystem.BLL
{
    public class PlaylistTrackServices
    {
        #region Fields

        private readonly PlaylistManagementContext _playlistManagementContext;
        #endregion

        internal PlaylistTrackServices(PlaylistManagementContext playlistManagementContext)
        {
            _playlistManagementContext = playlistManagementContext;
        }

        //  getting working version data
        public WorkingVersionView GetWorkingVersion()
        {
            return _playlistManagementContext.WorkingVersions
                .Select(x => new WorkingVersionView
                {
                    VersionId = x.VersionId,
                    Major = x.Major,
                    Minor = x.Minor,
                    Build = x.Build,
                    Revision = x.Revision,
                    AsOfDate = x.AsOfDate,
                    Comments = x.Comments
                }).FirstOrDefault();
        }

        // fetch playlist
        public List<PlaylistTrackView> FetchPlaylist(string userName,
            string playlistName)
        {
            return null;
        }

        //  fetch artist or album tracks
        public List<TrackSelectionView> FetchArtistOrAlbumTracks(string searchType,
            string searchValue)
        {
            return null;
        }

        //  add track
        public void AddTrack(string userName, string playlistName, int trackId)
        {

        }

        //  remove track(s)
        public void RemoveTracks(int playlistId, List<int> trackIds)
        {

        }

        //  move track(s)
        public void MoveTracks(int playlistId, List<MoveTrackView> moveTracks)
        {

        }

    }
}
