<Query Kind="Program" />

void Main()
{
	//  Any and All

	//  There are 25 genres on files.
	//  Show genres that have tracks which are not on any playlist
	Console.WriteLine($"{Environment.NewLine}Show genres that have tracks which are not on any playlist");
	Genres
		.Where(g => g.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0))
		.Select(g => g).Dump();

	//  Show genres that have all tracks at least once on a playlist
	Console.WriteLine($"{Environment.NewLine}Show genres that have all tracks at least once on a playlist");
	Genres
	.Where(g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
	.Select(g => g).Dump();

	//  Compare the track collection of 2 people using ALl and Any
	//  Create a small anonymous collection for two people
	//  Roberta Almeida (AlmeidaR) and Michelle Brooks (BrooksM)

	var listA = PlaylistTracks
					.Where(x => x.Playlist.UserName.Equals("AlmeidaR"))
					.Select(x => new
					{
						Song = x.Track.Name,
						Genre = x.Track.Genre.Name,
						ID = x.TrackId,
						Artist = x.Track.Album.Artist.Name
					})
					.Distinct()  //  So we don't get duplicated tracks
					.OrderBy(x => x.Song)
					//.Dump()  // 110
					;

	var listB = PlaylistTracks
					.Where(x => x.Playlist.UserName.Equals("BrooksM"))
					.Select(x => new
					{
						Song = x.Track.Name,
						Genre = x.Track.Genre.Name,
						ID = x.TrackId,
						Artist = x.Track.Album.Artist.Name
					})
					.Distinct()  //  So we don't get duplicated tracks
					.OrderBy(x => x.Song)
					//.Dump()  // 88
					;
	//  List the tracks that BOTH Roberto and Michelle like
	//  Compare 2 collections together (List A and List B)
	//  Assume ListA is Roberta and ListB is Michelle
	//  ListA is the collection you wish to report on
	//  ListB is what your wish to compare ListA to (no reporting)

	Console.WriteLine($"{Environment.NewLine}Songs from AlmeidaR playlist that BrooksM also likes");
	listA
		.Where(la => listB.Any(lb => lb.ID == la.ID))
		//  listB find ANYthing from ListB ID's == ListA ID's
		.OrderBy(lista => lista.Song).Dump();

	Console.WriteLine($"{Environment.NewLine}Songs from BrooksM playlist that AlmeidaR also likes (Same as previous list)");
	listB
		.Where(lb => listA.Any(la => la.ID == lb.ID))
		//  listB find ANYthing from ListB ID's == ListA ID's
		.OrderBy(listb => listb.Song).Dump();

	//  What songs does listA like but listB does not
	Console.WriteLine($"{Environment.NewLine}Top 20 songs that AlmeidaR likes but BrooksM dones not");
	listA
		.Where(la => listB.All(lb => lb.ID != la.ID))
		//  listB find ANYthing from ListB ID's == ListA ID's
		.OrderBy(lista => lista.Song)
		.Take(20)
		.Dump();

	//  There maybe time that using a !Any() -> All(!relationship)
	//	 and a !All() -> Any(!relationship)

	//	Using All and Any in comparing 2 complex collections.
	//  If you collection is NOT a complex record (list of integer or string)
	//	 there is a Linq method call .Except that can be used to solve your Linq
	//    except

}



