<Query Kind="Program">
  <Connection>
    <ID>393cf574-7941-44aa-8f03-8108842a8826</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>AMD-3900XT\MSSQL2K19</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook2018</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

//  Used to load the SongItemView and AlbumTrackView models from the classes folder
#load "..\LINQ\ViewModels\*.cs"
void Main()
{
	//  Table<Albums>
	Console.WriteLine("Table<Albums>");
	Albums.Dump();

	//  Conversion .ToList()
	//  List<Albums>
	Console.WriteLine($"{Environment.NewLine}List<Albums> (Top 10)");
	Albums.Take(10).ToList().Dump();

	//  Can also be created using "select"
	Console.WriteLine($"{Environment.NewLine}List<Albums> using 'Select' (Top 10)");
	Albums.Select(a => a).Take(10).ToList().Dump();

	//  Display all albums and their tracks
	//  Display the album title, artist name and album tracks
	//  For each track, show the song name and playtime in seconds
	//  Show only albums with 25 or more tracks.

	Console.WriteLine($"{Environment.NewLine}Display all albums that have 25 or more tracks");
	List<AlbumTrackView> albumList = Albums
					.Where(a => a.Tracks.Count() >= 25)
					.Select(a => new AlbumTrackView
					{
						Title = a.Title,
						Artist = a.Artist.Name,
						Songs = a.Tracks.Select(tr => new SongItemView
						{
							Song = tr.Name,
							PlayTime = tr.Milliseconds / 1000.0
						}).ToList()
					}).ToList().Dump();
	//	Typically if the albumlist was a var variable in your BLL method
	//		AND the method return datatype was a List<T>, one could on the
	//		return statement do: return albumnList.ToList();  (Saw in your 1517 course)

	//  Using FirstOrDefault()
	//  great for lookups that you expect 0, 1, or more instances retrun
	//  Find the first album of Deep Purple
	Console.WriteLine($"{Environment.NewLine}Find the first album of Deep Purple");
	string artistParam = "Deep Purple";
	var resultsFOD = Albums
						.Where(a => a.Artist.Name.Equals(artistParam))
						.Select(a => a)
						.OrderBy(a => a.ReleaseYear)
						.FirstOrDefault();

	//  check if there is a album found for Deep Purple
	if (resultsFOD != null)
		resultsFOD.Dump();
	else
		Console.WriteLine($"No albums found for artist {artistParam}");

	//  Using SingleOrDeafault()
	//  Expecting at most a single instance being return

	//  Find the album by albumID (sucess)
	Console.WriteLine($"{Environment.NewLine}Find a single album based on album ID (Sucess)");
	int albumID = 1;
	var resultSOD = Albums
						.Where(a => a.AlbumId == albumID)
						.Select(a => a)
						.SingleOrDefault()
						;

	if (resultSOD != null)
		resultSOD.Dump();
	else
		Console.WriteLine($"No albums found for id of {albumID}");

	//  Find the album by albumID (sucess)
	Console.WriteLine($"{Environment.NewLine}Find a single album based on album ID (Fail)");
	albumID = 10000000;
	resultSOD = Albums
					   .Where(a => a.AlbumId == albumID)
					   .Select(a => a)
					   .SingleOrDefault()
					   ;

	if (resultSOD != null)
		resultSOD.Dump();
	else
		Console.WriteLine($"No albums found for id of {albumID}");

	//  Distinct()
	//  Obtain a list of customer countries (Not Distinct)".
	Console.WriteLine($"{Environment.NewLine}List of top 20 countries (Not Distinct)");
	var resultDistinct = Customers
							.OrderBy(c => c.Country)
							.Select(c => c.Country)
							.Take(20);
	resultDistinct.Dump();

	//  Removes duplicated reported lines
	//  Obtain a list of customer countries (Distinct)".
	Console.WriteLine($"{Environment.NewLine}List of top 20 countries (Distinct)");
	resultDistinct = Customers
						   .OrderBy(c => c.Country)
						   .Select(c => c.Country)
						   .Distinct()
						   .Take(20);
	resultDistinct.Dump();

	// .Take() and Skip()
	//  In1517, when you wanted to use your paginator
	//		the query method was to return ONLY the needed records to display
	//	a) you passed in the pagesize and pagenumber
	//	b) the query was executed, returning all rows
	//	c) set your out parameter to the .Count of rows
	//  d) calculated the number of rows to skip (pageNumber -1) * pagesize
	//	e) on the return statement, against your collection, you used a .SKIP & .TAKE
	//	Return variableName.Skip(rowsSkipped).Take(pagesize).ToList()

	//  TakeWhile
	//  Does not work with a query (LINQ Pad error)
	// 	Albums.TakeWhile(x => x.Title.Length > 5).Dump();


	
}
