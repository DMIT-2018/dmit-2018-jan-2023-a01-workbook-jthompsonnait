<Query Kind="Program">
  <Connection>
    <ID>3f77383b-7ae5-4f03-8c46-ddbe5b1af50d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>ChinookSept2018</Database>
    <DisplayName>ChinookSept2018</DisplayName>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

#load ".\ViewModels\*.cs"
using Chinook;
void Main()
{
	try
	{
		// PlaylistTrackServices_FetchArtistOrAlbumTracks
		// PlaylistTrackServices is the BLL class
		// FetchArtistOrAlbumTracks is the method name

		// create a placeholders for our parameters
		string searchType = "Album";
		string searchValue = "";
		List<TrackSelectionView> tracks = PlaylistTrackServices_FetchArtistOrAlbumTracks(searchType, searchValue);
		tracks.Dump();
	}
	catch(AggregateException ex)
	{
		foreach(var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch(ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
}

#region Methods
private Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}
#endregion


public List<TrackSelectionView> PlaylistTrackServices_FetchArtistOrAlbumTracks
					(string searchType, string searchValue)
{
	//  Business Rules
	//	These are processing rules that need to be satisfied for valid data.
	//		rule: search value value cannot be empty
	if (string.IsNullOrWhiteSpace(searchValue))
	{
		throw new ArgumentNullException("Search value name is missing");
	}
	return Tracks
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
			}).ToList();

}







