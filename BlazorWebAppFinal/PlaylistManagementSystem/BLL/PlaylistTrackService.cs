﻿#nullable disable
using PlaylistManagementSystem.DAL;
using PlaylistManagementSystem.Paginator;
using PlaylistManagementSystem.ViewModels;
using PlaylistManagementSystem.Entities;
using System.Diagnostics;

namespace PlaylistManagementSystem.BLL
{
    public class PlaylistTrackService
    {
        #region Fields
        private readonly PlaylistManagementContext _playlistManagementContext;
        #endregion

        internal PlaylistTrackService(PlaylistManagementContext playlistManagementContext)
        {
            _playlistManagementContext = playlistManagementContext;
        }

        //  Get Working Version data
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

        //  Fetch Playlist
        public async Task<List<PlaylistTrackView>> FetchPlaylist(string userName, string playlistName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("User name is missing");
            }
            if (string.IsNullOrWhiteSpace(playlistName))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }

            return _playlistManagementContext.PlaylistTracks
                .Where(x => x.Playlist.UserName == userName && x.Playlist.Name == playlistName)
                .Select(x => new PlaylistTrackView
                {
                    TrackId = x.TrackId,
                    TrackNumber = x.TrackNumber,
                    SongName = x.Track.Name,
                    Milliseconds = x.Track.Milliseconds
                }).OrderBy(x => x.TrackNumber)
                .ToList();
        }


        //  Fetch Artist or Album Tracks
        public List<TrackSelectionView> FetchArtistOrAlbumTracks(string searchType, string searchValue)
        {
            return null;
        }

        //  Fetch Artist or Album Tracks using the Paginator
        public Task<PagedResult<TrackSelectionView>> FetchArtistOrAlbumTracks(string searchType, string searchValue,
                                            int page, int pageSize, string sortColumn, string sortDirection)
        {
            //  Business Rules
            //	These are processing rules that need to be satisfied for valid data.
            //		rule: search value value cannot be empty
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                throw new ArgumentNullException("Search value name is missing");
            }
            //  Task.FromResult() creates a finished Task that holds a value in its Result property. 
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
                .OrderBy(sortColumn, sortDirection)     // custom sort extension to sort on a string representing a column
                .ToPagedResult(page, pageSize));

        }

        // Add Track
        public void AddTrack(string userName, string playlistName, int trackId)
        {
            //	create local variables
            //	Check to ensure that the track has not be removed from the catelog/libary
            bool trackExist = false;
            Playlist playlist = null;
            int trackNumber = 0;
            bool playlistTrackExist = false;
            PlaylistTrack playlistTrack = null;

            #region Business Logic and Parameter Exceptions

            //	create a list<Exception> to contain all discovered errors
            List<Exception> errorList = new List<Exception>();

            //	Buisness Rules
            //		rule:	a track can only exist once on a playlist
            //		rule:	each track on a playlist is assigned a continous track number
            //		rule:	playlist name cannot be empty
            //		rule:	track must exist in the tracks table

            //	If the business rules are passed, consider the data valid:
            //		a)	stage your tranaction work (Adds, Updates, Deletes)
            //		b)	execute a SINGLE .SaveChanges() - commits to database.

            //	paramter validation
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("User name is missing");
            }

            if (string.IsNullOrWhiteSpace(playlistName))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }

            #endregion

            //  check that the incoming data exist
            trackExist = _playlistManagementContext.Tracks
                .Where(x => x.TrackId == trackId)
                .Select(x => x.TrackId)
                .Any();
            if (!trackExist)
            {
                throw new ArgumentNullException("Selected track no longer is on the system.  Refresh track table");
            }

            //	Business Processing
            //	Check if the playlist exist.
            playlist = _playlistManagementContext.Playlists
                .Where(x => x.Name == playlistName
                            && x.UserName == userName)
                .FirstOrDefault();

            //  does not exist
            if (playlist == null)
            {
                playlist = new Playlist()
                {
                    Name = playlistName,
                    UserName = userName
                };
                //	stage (on in memory)
                _playlistManagementContext.Playlists.Add(playlist);
                trackNumber = 1;
            }
            else
            {
                //	playlist already exist
                //	rule:  unique tracks on the playlist
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
                    errorList.Add(new Exception($"Select track ({songName}) is already on the playlist"));
                }
                else
                {
                    //  get the current track counts
                    trackNumber = _playlistManagementContext.PlaylistTracks
                        .Where(x => x.Playlist.Name == playlistName
                                    && x.Playlist.UserName == userName)
                        .Count();
                    //  increment this by 1
                    trackNumber++;
                }
            }

            //	add the track to the playlist
            //	create an instance for the playlist track

            playlistTrack = new();

            //	load values
            playlistTrack.TrackId = trackId;
            playlistTrack.TrackNumber = trackNumber;

            //	What about the second part of the primary key:  PlaylistId?
            //	If playlist exists, then we know the id: playlist.PlaylistId
            //	But if the playlist is NEW, we DO NOT KNOW the id.

            //	In the sitution of a NEW playlist, even though we have created the 
            //		playlist instance (see above) it is ONLY stage!!!
            //	This means that the actual SQL record has NOT yet been created.
            //	This means that the IDENTITY value for the new playlist DOES NOT
            //		yet exists.
            //	The value on the playlist instance (playlist) is zero (0)
            //		Therefor, we have a serious problem.

            //	Solution
            //	It is build into the Entity Framework software and is based using
            //		the navigational property in the Playlist pointing to it's "child"

            //	Staging a typical Add in the past was to reference the entity and use the
            //		entity.Add(xxx)
            //	If you use this statement, the playlistId would be zero (0)
            //		causing your transaction to ABORT
            //	Why?	PKeys cannot be zero (0) (FKey to Pkey problem)

            //	INSTEAD, do the staging using the "parent.navChildProperty.Add(xxx)
            playlist.PlaylistTracks.Add(playlistTrack);

            //	Staging is complete.
            //	Commit the work (Transaction)
            //	Committing the work needs a .SaveChanges()
            //	A transaction has ONLY ONE .SaveChanges()
            //	But, what if you have discoved errors during the business process???
            if (errorList.Count() > 0)
            {
                //  we need to clear the "track changes" otherwise we leave our entity system in flux.
                _playlistManagementContext.ChangeTracker.Clear();
                throw new AggregateException("Unable to add new track.  Check concerns", errorList);
            }
            else
            {
                //	consider data valid
                //	has passed business processing rules
                _playlistManagementContext.SaveChanges();
            }
        }

        //  Remove Track(s)
        public void RemoveTracks(int playlistId, List<int> trackIds)
        {
            //	local variables
            PlaylistTrack playlistTrackToRemove = null;
            PlaylistTrack playlistTrackToRenumber = null;

            //	we need a container to hold x number of Exception messages
            List<Exception> errorlist = new List<System.Exception>();

            if (playlistId == 0)
            {
                throw new ArgumentNullException("No playlist ID was provided");
            }

            //var count = trackIds.Count();
            if (trackIds.Count() == 0)
            {
                throw new ArgumentNullException("No list of tracks were submitted");
            }

            //	obtain the tracks to keep
            //	create a query to extract the "keep" tracks from the incoming data
            //	we want all playlist tracks that are part of playlist and not in the collection
            //		if tracks that we are removing
            var keeplist = _playlistManagementContext.PlaylistTracks
                                .AsEnumerable()
                                .Where(x => x.PlaylistId == playlistId
                                        && trackIds.All(tid => tid != x.TrackId))
                                .OrderBy(x => x.TrackNumber).ToList();

            foreach (var id in trackIds)
            {
                playlistTrackToRemove = _playlistManagementContext.PlaylistTracks
                                            .Where(x => x.PlaylistId == playlistId
                                                && x.TrackId == id)
                                            .FirstOrDefault();
                if (playlistTrackToRemove != null)
                {
                    _playlistManagementContext.PlaylistTracks.Remove(playlistTrackToRemove);
                }
            }

            int tracknumber = 1;
            foreach (var item in keeplist)
            {
                playlistTrackToRenumber = _playlistManagementContext.PlaylistTracks
                                            .Where(x => x.PlaylistId == playlistId
                                            && x.TrackId == item.TrackId)
                                            .FirstOrDefault();
                if (playlistTrackToRenumber != null)
                {
                    playlistTrackToRenumber.TrackNumber = tracknumber;
                    _playlistManagementContext.PlaylistTracks.Update(playlistTrackToRenumber);

                    //	this library is not directly accessable by LinqPAD
                    //	EntityEntry<PlaylistTracks> updating = _context.Entry(playlistTrackToRenumber);
                    //	updating.State = EntityState.Modify;

                    tracknumber++;
                }
                else
                {
                    var songName = _playlistManagementContext.Tracks
                                    .Where(x => x.TrackId == item.TrackId)
                                    .Select(x => x.Name)
                                    .FirstOrDefault();
                    errorlist.Add(new Exception($"The track {songName} is no longer on file.  Please remove"));
                }
            }

            if (errorlist.Count() > 0)
            {
                //  we need to clear the "track changes" otherwise we leave our entity system in flux.
                _playlistManagementContext.ChangeTracker.Clear();
                throw new AggregateException("Unable to remove request tracks.  Check concerns", errorlist);
            }
            else
            {
                //  all work has been staged
                _playlistManagementContext.SaveChanges();
            }
        }

        //  Move Track
        public void MoveTrack(int playlistId, List<MoveTrackView> moveTracks)
        {
            //	local variables
            List<PlaylistTrack> scratchPadPlaylistTracks = null;
            int tracknumber = 0;

            //	we need a container to hold x number of Exception messages
            List<Exception> errorlist = new List<System.Exception>();

            if (playlistId == 0)
            {
                throw new ArgumentNullException("No playlist ID was provided");
            }

            //var count = trackIds.Count();
            if (moveTracks.Count() == 0)
            {
                throw new ArgumentNullException("No list of tracks were submitted");
            }

            //  check that we have items to move (track number greater than zero)
            int count = moveTracks
                .Where(x => x.TrackNumber > 0)
                .Count();

            if (count == 0)
            {
                throw new ArgumentNullException("No tracks were provided to be move");
            }

            //  check that we have items to move (track number greater than zero)
            count = moveTracks
               .Where(x => x.TrackNumber < 0)
               .Count();

            if (count > 0)
            {
                throw new ArgumentNullException("There are track number less than zero");
            }

            List<MoveTrackView> tracks = moveTracks
                                            .Where(x => x.TrackNumber > 0)
                                            .GroupBy(x => x.TrackNumber)
                                            .Where(gb => gb.Count() > 1)
                                            .Select(gb => new MoveTrackView
                                            {
                                                TrackId = 0,
                                                TrackNumber = gb.Key
                                            }).ToList();

            //	check for any duplicate track number
            foreach (var t in tracks)
            {
                errorlist.Add(new Exception($"Track number {t.TrackNumber} is used more than once"));
            }

            // reorder the tracks
            scratchPadPlaylistTracks = _playlistManagementContext.PlaylistTracks
                                        .Where(x => x.PlaylistId == playlistId)
                                        .OrderBy(x => x.TrackNumber)
                                        .Select(x => x).ToList();

            //	reset all of our track numbers to zero
            foreach (var playlistTrack in scratchPadPlaylistTracks)
            {
                playlistTrack.TrackNumber = 0;
            }

            //	update the playlist track numbers with move track numbers
            foreach (var moveTrack in moveTracks)
            {
                PlaylistTrack playlistTrack = scratchPadPlaylistTracks
                                                .Where(x => x.TrackId == moveTrack.TrackId)
                                                .Select(x => x).FirstOrDefault();

                //  check to see if the playlist track exist in the PlaylistTracks
                if (playlistTrack == null)
                {
                    var songName = _playlistManagementContext.Tracks
                                    .Where(x => x.TrackId == moveTrack.TrackId)
                                    .Select(x => x.Name)
                                    .FirstOrDefault();
                    errorlist.Add(new Exception($"The track {songName} cannot be found in your playlist.  Please refresh playlist"));
                }
                else
                {
                    playlistTrack.TrackNumber = moveTrack.TrackNumber;
                }
            }

            if (errorlist.Count() == 0)
            {
                foreach (var playlistTrack in scratchPadPlaylistTracks)
                {
                    bool wasFound = true;
                    //  only want to process those track numbers that are empty
                    if (playlistTrack.TrackNumber == 0)
                    {
                        while (wasFound)
                        {
                            //  we want to increment the track number and process until 
                            //		the value is not found in the scratchPadPlaylistTracks
                            tracknumber++;
                            wasFound = scratchPadPlaylistTracks
                                        .Where(x => x.TrackNumber == tracknumber)
                                        .Select(x => x)
                                        .Any();
                        }
                        playlistTrack.TrackNumber = tracknumber;
                    }
                }
            }

            if (errorlist.Count() > 0)
            {
                //  we need to clear the "track changes" otherwise we leave our entity system in flux.
                _playlistManagementContext.ChangeTracker.Clear();
                throw new AggregateException("Unable to remove request tracks.  Check Concerns", errorlist);
            }
            else
            {
                //	all work has been staged
                _playlistManagementContext.SaveChanges();
            }
        }

        public List<ExtendedTrackSelectionView> FetchInventory()
        {
            return _playlistManagementContext.Tracks
                .Take(20)
                .OrderBy(x => x.Name)
                .Select(x => new ExtendedTrackSelectionView
                {
                    TrackId = x.TrackId,
                    AlbumTitle = x.Album.Title,
                    ArtistName = x.Album.Artist.Name,
                    SongName = x.Name,
                    Price = x.UnitPrice,
                    Milliseconds = x.Milliseconds,
                    Quantity = 1,
                    Total = x.UnitPrice
                }).ToList();
        }
    }
}
