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

		//	show that both the playlist and track does not exist
		Console.WriteLine("Before Removing Track");
		PlaylistTrackServices_FetchPlaylist(userName, playlistName).Dump();
		PlaylistTrackServices_RemoveTracks(playlistId, trackIds);

		//	show that both the playlist and track now exist
		Console.WriteLine("After Removing Track");
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


public void PlaylistTrackServices_RemoveTracks(int playlistId, List<int> trackIds)
{
	
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
