<Query Kind="Program">
  <Connection>
    <ID>6908ebe3-e58f-4ab6-b1c6-6d010450e634</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.</Server>
    <Database>ChinookSept2018</Database>
    <DisplayName>ChinookEntity</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
      <EFVersion>6.0.10</EFVersion>
      <TrustServerCertificate>True</TrustServerCertificate>
    </DriverData>
  </Connection>
</Query>

#load ".\ViewModels\*.cs"
using Chinook;
void Main()
{
	try
	{
		//  This is the DRIVER area.
		//  code and test the RemoveTrack
		//	The command method will receive no model but will receive
		//		individual arguments: playlistId, List<trackId>

		//	793	A Castle Full of Rascals
		//	822	A Twist In THe Tail
		//	543	Burn
		//	756	Child in Time

		string userName = "HansenB";
		string playlistName = "Jan23A01";

		int playlistId = Playlists
						.Where(x => x.UserName == userName
								&& x.Name == playlistName)
						.Select(x => x.PlaylistId).FirstOrDefault();

		if (playlistId == 0)
		{
			throw new ArgumentNullException($"No playlist exist for {playlistName}");
		}

		List<MoveTrackView> moveTracks = new();
		moveTracks.Add(new MoveTrackView() { TrackId = 822, TrackNumber = 1 });
		moveTracks.Add(new MoveTrackView() { TrackId = 756, TrackNumber = 2 });
		moveTracks.Add(new MoveTrackView() { TrackId = 543, TrackNumber = 3 });
		moveTracks.Add(new MoveTrackView() { TrackId = 793, TrackNumber = 4 });


		//	show that both the playlist and track does not exist
		Console.WriteLine("Before Moving Track");
		PlaylistTrackServices_FetchPlaylist(userName, playlistName).Dump();
		PlaylistTrackServices_MoveTracks(playlistId, moveTracks);

		//	show that both the playlist and track now exist
		Console.WriteLine("After Moving Track");
		PlaylistTrackServices_FetchPlaylist(userName, playlistName).Dump();
	}


	#region catch all exceptions
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
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


public void PlaylistTrackServices_MoveTracks(int playlistId, List<MoveTrackView> moveTracks)
{
	//	local variables
	List<PlaylistTracks> scratchPadPlaylistTracks = null;
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
	scratchPadPlaylistTracks = PlaylistTracks
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
		PlaylistTracks playlistTrack = PlaylistTracks
										.Where(x => x.TrackId == moveTrack.TrackId)
										.Select(x => x).FirstOrDefault();

		//  check to see if the playlist track exist in the PlaylistTracks
		if (playlistTrack == null)
		{
			var songName = Tracks
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
		throw new AggregateException("Unable to remove request tracks.  Check Concerns", errorlist);
	}
	else
	{
		//	all work has been staged
		SaveChanges();

	}


	if (errorlist.Count() > 0)
	{
		throw new AggregateException("Unable to remove request tracks.  Check concerns", errorlist);
	}
	else
	{
		//  all work has been staged
		SaveChanges();
	}
}



public List<PlaylistTrackView> PlaylistTrackServices_FetchPlaylist(string userName, string playlistName)
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


	return PlaylistTracks
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
