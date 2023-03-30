#nullable disable
using PlaylistManagementSystem.DAL;
using PlaylistManagementSystem.Entities;
using PlaylistManagementSystem.Paginator;
using PlaylistManagementSystem.ViewModels;
using System.Diagnostics;

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
        public async Task<List<PlaylistTrackView>> FetchPlaylist(string userName,
            string playlistName)
        {
            //  Business Rules
            //	These are processing rules that need to be satisfied
            //		for valid data
            //		rule:	playlist name cannot be empty
            //		rule:	playlist must exist in the database
            //					(will be handle on webpage)

            if (string.IsNullOrWhiteSpace(playlistName))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }


            return _playlistManagementContext.PlaylistTracks
                .Where(x => x.Playlist.UserName == userName
                            && x.Playlist.Name == playlistName)
                .Select(x => new PlaylistTrackView
                {
                    TrackId = x.TrackId,
                    SongName = x.Track.Name,
                    TrackNumber = x.TrackNumber,
                    Milliseconds = x.Track.Milliseconds
                }).OrderBy(x => x.TrackNumber)
                .ToList();
        }

        //  fetch artist or album tracks using the paginator
        public Task<PagedResult<TrackSelectionView>> FetchArtistOrAlbumTracks(string searchType, string searchValue,
            int page, int pageSize, string sortColumn, string sortDirection)
        {
           //   Business Rule:
           //   These are processing rules that need to be satisfied for valid data.
           //       rule:   search value cannot be empty
           if (string.IsNullOrWhiteSpace(searchValue))
           {
               throw new ArgumentNullException("search value is missing");
           }
           //   Task,FromResult() creates a finished task that holds a value in its Result property.
           return Task.FromResult(_playlistManagementContext.Tracks
               .Where(x => searchType == "Artist"
                   ? x.Album.Artist.Name.Contains(searchValue)
                   : x.Album.Title.Contains(searchValue))
               .Select(x => new TrackSelectionView
               {
                   TrackId = x.TrackId,
                   SongName = x.Name,
                   AlbumTitle = x.Album.Title,
                   ArtistName = x.Album.Artist.Name,
                   Milliseconds = x.Milliseconds,
                   Price = x.UnitPrice
               }).AsQueryable()
               .OrderBy(sortColumn, sortDirection)  // custom sort extension to sort on a string representing a column
               .ToPagedResult(page, pageSize));
        }

        //  add track
        public void AddTrack(string userName, string playlistName, int trackId)
        {
            //	create local variables
            //	check to ensure that the track has not been removed from the catelog/library
            bool trackExist = false;
            Playlist playlist = null;
            int trackNumber = 0;
            bool playlistTrackExist = false;
            PlaylistTrack playlistTrack = null;

            #region Business Logic and Parameter Exceptions
            //	create a List<Exception> to contain all discovered errors
            List<Exception> errorList = new List<Exception>();

            //	Business Rules
            //		rule:	a track can only exist once on a playlist
            //		rule:	each track on a playlist is assigned a continous track number
            //		rule:	playlist name cannot be empty
            //		rule:	track must exist in the tracks table

            //	If the business rules are passed, conside the data valid:
            //		a)	stage your transaction work (Adds, Updates, Deletes)
            //		b)	execute a SINGLE .SaveChanges() - commits to database.

            //	parameter validation
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("User name is missing");
            }

            if (string.IsNullOrWhiteSpace(playlistName))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            #endregion

            //	check that the incoming data exists
            trackExist = _playlistManagementContext.Tracks
                            .Where(x => x.TrackId == trackId)
                            .Select(x => x.TrackId)
                            .Any();

            if (!trackExist)
            {
                throw new ArgumentNullException("Selected track no longer is on the system. Refresh track table");
            }

            //	Business Process
            //	Check if the playlist exist
            playlist = _playlistManagementContext.Playlists
                        .Where(x => x.Name == playlistName
                                && x.UserName == userName)
                        .FirstOrDefault();

            //	does not exist
            if (playlist == null)
            {
                playlist = new Playlist()
                {
                    Name = playlistName,
                    UserName = userName
                };

                //	add the playlist to the PlayLists collection
                _playlistManagementContext.Playlists.Add(playlist);
                trackNumber = 1;
            }
            else
            {
                //	playlist aready exist
                //	rule:	unique tracks on the playlist
                playlistTrackExist = _playlistManagementContext.PlaylistTracks
                                        .Any(x => x.Playlist.Name == playlistName
                                                && x.Playlist.UserName == userName
                                                && x.TrackId == trackId);

                if (playlistTrackExist)
                {
                    var songName = _playlistManagementContext.Tracks
                                    .Where(x => x.TrackId == trackId)
                                    .Select(x => x.Name)
                                    .FirstOrDefault();

                    //	rule violation
                    errorList.Add(new Exception($"Selected track ({songName}) is already on the playlist"));
                }
                else
                {
                    trackNumber = _playlistManagementContext.PlaylistTracks
                                    .Where(x => x.Playlist.Name == playlistName
                                            && x.Playlist.UserName == userName)
                                    .Count();
                    //	increment this by 1
                    trackNumber++;
                }
            }

            //	add the track to the playlist
            //	create an instance for the playlist track
            playlistTrack = new PlaylistTrack();

            //	load values
            playlistTrack.TrackId = trackId;
            playlistTrack.TrackNumber = trackNumber;

            //	What about the second part of the primary key:  PlaylistId?
            //	IF playlist exists, then we know the id:  playlist.PlaylistId
            //	But if the playlist is NEW, we DO NOT KNOW the id

            //	In the sitution of a NEW playlist, even though we have created the 
            //		playlist instance (see above), it is ONLY stage (In memory)
            //	This means that the actual SQL record has NOT yet been created.
            //	This means that the IDENTITY value for the new playlist DOES NOT yest exists.
            //	The value on the playlist instance (playlist) is zero(0).
            //		Therefore, we have a serious problem.

            //	Solution
            //	It is build into the Entity Framework software and is based on using the
            //		navigational property in the Playlist pointing to it's "child"

            //	Staging a typical Add in the past was to refernce the entity and
            //		use the enity.Add(xxx)
            //	If you use this statement, the playlistId would be zero (0)
            //		causing your transaction to ABORT.
            //	Why?	PKeys cannot be zero (0) (FKey to PKey problem)

            //	Instead, do the staging using the "parent.navChildProperty.Add(xxx)
            playlist.PlaylistTracks.Add(playlistTrack);

            //	Staging is complete.
            //	Commit the work (Transaction)
            //	Committing the work which needs a SaveChanges()
            //	A transaction has ONLY ONE SaveChanges()
            //	But, what if you have discoved errors during the business process???
            if (errorList.Count() > 0)
            {
                //  we need to clear the "track changes" otherwise we leave
                //      our entity system in flux
                _playlistManagementContext.ChangeTracker.Clear();
                // throw the list of business processing error(s)
                throw new AggregateException("Unable to add new track.  Check concerns", errorList);
            }
            else
            {
                //	consider data valid
                //	has passed business processing rules
                _playlistManagementContext.SaveChanges();
            }
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
