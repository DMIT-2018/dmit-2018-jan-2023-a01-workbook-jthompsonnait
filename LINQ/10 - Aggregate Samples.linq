<Query Kind="Program" />

void Main() 
{ 
    //  Aggregates  
    //    .Count()                            Count the number of instances in a collection  
    //  .Sum() or .Sum(x => ...)            Sum(totals) a numeric field (numeric expression) in a collection  
    //  .Min() or .Min(x => ...)            Find the minimum value of a collection of values for a field  
    //  .Sum() or .Max(x => ...)              Find the maximum value of a collection of values for a field  
    //  .Average() or .Average(x => ...)    Find the average value of a collection of values for a field  

    //  IMPORTANT!!!  
    //  Aggregates work ONLY on a collection of values for a particular field (expressions)  
    //  Aggregate DO NOT WORK on a single row (not the same as a collection with one row)  

    //  Query Syntax  
    //  (From ...  
    //  ...  
    //  Select expression).aggregate();  

    //  Method Syntax  
    //  collection.aggregate(x => expression) Sum, Min, Max, Average  
    //  NOTE:  Count() does NOT use an expression  
    //  collection.Select(x => expression).Aggregate();  

    //  You can use multiple aggregates on a single column.  
    //  collection.Sum(x => expression).Min(x => expression)  

    //  Find the average play time (length) of tracks in our music collections  

    //  Thought Process:  
    //  average is an aggregate.  
    //  What is the collection:  a track is a member of the tracks table.  
    //  What the expression:  Field is in milliseconds representing the tracks  
    //   length (playtime)  

    //  Query Syntax  
    (from x in Tracks 
     select x.Milliseconds).Average().Dump(); 

    //  Method  Syntax  
    Tracks.Average(x => x.Milliseconds).Dump(); 
    Tracks.Select(x => x.Milliseconds).Average(); 

    //  Review process for getting data!!!!!!!!!!!!!!!!!!!!!!  
    //  List all albums of the 60s  
    //  Album Title, Artist, various aggregates for the albums containing tracks  

    // For each album, show the number of tracks, the longest playing track,  
    //  the shortest playing track, the total price of all tracks and the   
    //  average playing length of the album tracks.  

    //  Hint:  Albums have two navigation properties  
    //           Artist "points to" the single parent record.  
    //         Tracks "points to" the collection of child records {tracks]  
    //           of the albums.  

	//	We are going to go through the entire process for creating our query.

    //    1) Review the album table  
    Albums.Dump(); 

    //  2) Filter by the 60s  
    Albums 
        .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970) 
        .Select(x => x).Dump(); 

    //  3) Show the album title and artist  
    Albums 
        .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970) 
        .Select(x => new 
        { 
            AlbumTile = x.Title, 
            Artist = x.Artist.Name 
        }).Dump(); ; 

    //  4) Add the track count  
    Albums 
        .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970) 
        .Select(x => new 
        { 
            AlbumTile = x.Title, 
            Artist = x.Artist.Name, 
            TrackCount = x.Tracks.Count() 
        }).Dump(); ; 

    //  5) Filter by having tracks  
    Albums 
            .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970 
                && x.Tracks.Count() > 0) 
            //  couple more ways of getting tracks  
            //  && x.Tracks != null)  
            //  && x.Tracks.Any())  
            .Select(x => new 
            { 
                AlbumTile = x.Title, 
                Artist = x.Artist.Name, 
                TrackCount = x.Tracks.Count() 
            }).Dump(); ; 

    //  6) Find the longest track  
    Albums 
                .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970 
                    && x.Tracks.Count() > 0) 
                .Select(x => new 
                { 
                    AlbumTile = x.Title, 
                    Artist = x.Artist.Name, 
                    TrackCount = x.Tracks.Count(), 
                    LongestTrack = x.Tracks.Max(x => x.Milliseconds / 1000)  //  converting milliseconds into seconds  
                }).Dump(); ; 

    //  7) Find the shortest track  
    Albums 
                    .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970 
                        && x.Tracks.Count() > 0) 
                    .Select(x => new 
                    { 
                        AlbumTile = x.Title, 
                        Artist = x.Artist.Name, 
                        TrackCount = x.Tracks.Count(), 
                        LongestTrack = x.Tracks.Max(x => x.Milliseconds / 1000),//  converting milliseconds into seconds  
                        ShortestTrack = x.Tracks.Min(x => x.Milliseconds / 1000) 
                    }).Dump(); ; 

    //  8) Find the total price                      
    Albums 
                        .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970 
                            && x.Tracks.Count() > 0) 
                        .Select(x => new 
                        { 
                            AlbumTile = x.Title, 
                            Artist = x.Artist.Name, 
                            TrackCount = x.Tracks.Count(), 
                            LongestTrack = x.Tracks.Max(x => x.Milliseconds / 1000),//  converting milliseconds into seconds  
                            ShortestTrack = x.Tracks.Min(x => x.Milliseconds / 1000), 
                            TotalPrice = x.Tracks.Sum(x => x.UnitPrice), 
                            //  Total price using a select()  
                            TotalPriceSelect = x.Tracks.Select(x => x.UnitPrice).Sum() 
                        }).Dump(); ; 

    //  9) Find the average length          
    Albums 
                            .Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970 
                                && x.Tracks.Count() > 0) 
                            .Select(x => new 
                            {
								AlbumTile = x.Title,
								Artist = x.Artist.Name,
								TrackCount = x.Tracks.Count(),
								LongestTrack = x.Tracks.Max(x => x.Milliseconds / 1000),//  converting milliseconds into seconds  
								ShortestTrack = x.Tracks.Min(x => x.Milliseconds / 1000),
								TotalPrice = x.Tracks.Sum(x => x.UnitPrice),
								//  Total price using a select()  
								TotalPriceSelect = x.Tracks.Select(x => x.UnitPrice).Sum(),
								AverageTrack = x.Tracks.Average(x => x.Milliseconds / 1000)
							}).Dump();

	//  10) Query syntax within a method  
	Albums
						.Where(x => x.ReleaseYear >= 1960 && x.ReleaseYear < 1970
							&& x.Tracks.Count() > 0)
						.Select(x => new
						{
							AlbumTile = x.Title,
							Artist = x.Artist.Name,
							TrackCount = x.Tracks.Count(),
							//  using a query   
							LongestTrack = (from tr in x.Tracks
											select tr.Milliseconds / 1000).Max(),//  converting milliseconds into seconds                              
							ShortestTrack = (from tr in x.Tracks
											 select tr.Milliseconds / 1000).Min(),
							TotalPrice = (from tr in x.Tracks
										  select tr.UnitPrice).Sum(),
							AverageTrack = (from tr in x.Tracks
											select tr.Milliseconds / 1000).Average()
						}).Dump();
}

