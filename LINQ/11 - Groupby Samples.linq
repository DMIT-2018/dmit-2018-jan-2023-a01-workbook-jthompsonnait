<Query Kind="Expression" />

//  Grouping 

//  When you create a group, it builds two (2) components 
//        a)  Key component (group by criteria values) 
//             reference this component using the group name.Key[.Property] 
//            (property, column, attribute, and values) 
//        b)    The data of the group (instances of the original collection) -> mini group. 

//	Ways to group 
//		a)  By a single property (column, field, attribute, value)  groupname.Key 
//		b)    By a set of properties (anonymous key set) groupname.Key,PropertyName 
//		c) By using an entity (x.nav property).  ***  Try to avoid *** 

//  Concept Processing 
//  We start with a "pile" of data (original collection) 
//  Specify the grouping criteria (value(s)) 
//		Result of the group operation will be to "place the data into smaller piles." 
//		(mini collections).  The piles are dependent on the grouping criteria value(s) 
//  The grouping criteria (property (ies), column, etc.) become the key. 
//  The individual instances are "the data in the smaller piles." 
//  The entire individual instances of the original collection are placed in the smaller piles 
//     (mini collections) 

//  Manipulating each of the "smaller piles" is now possible with your Linq commands. 

//  Grouping is different from Ordering 
//  Ordering is the re-sequencing of collections for display. 
//  Grouping re-organizes a collection into separate, usually smaller collections for processing. 

//  Display albums by release years 
//		This request does NOT need grouping 
//  	This request is a re-sequencing (ordering) of output (OrderBy) 
//  	This affects the display only. 
Albums
	.OrderBy(x => x.ReleaseYear)


//  Display albums grouped by ReleaseYear. 
//  NOT one display of albums but displays of the album for a specified value (ReleaseYear) 
//  Explicit request to break up the display into desired "piles" (collections) 

Albums
	.GroupBy(x => x.ReleaseYear)

//  Query Syntax 
from x in Albums
group x by x.ReleaseYear

//  processing on the created group of the Groupby method 
Albums
	.GroupBy(x => x.ReleaseYear)  //  This method returns a collection of "mini-collections" 
	.Select(eachgPile => new
	{
		Year = eachgPile.Key,
		NumberOfAlbums = eachgPile.Count()  //  processing of "mini-collection" data 
	})  //  The Select is procesing each mini collections one at a time 

//  Query Syntax 
//  Using this syntax, you MUST specify the name you wish to use to refer to the 
//		grouped (mini collections) collections 
//  After coding your group command, you MUST (are restricted to) use the name 
//		you have given your group collection. 

from a in Albums
	//orderby a.ReleaseYear:  Would be valid because "a" is in context 
	//orderby eachgPile.Key:  Would not be valid because grouping not specified yet.   
group a by a.ReleaseYear into eachgPile
//  orderby a.ReleaseYear:  Would be invalid because "a" is out of context, the group name is seachgPile 
orderby eachgPile.Key//:  Would be valid because "eachgPIle" is currently in context and Has your year. 
select new
{
	Year = eachgPile.Key, //  Key Component 
	NumberofAlbums = eachgPile.Count()  //  processing of "mini-collection" data 
}

//  Use a multiple set of criteria (properties) to for the group 
//   also include a nested query to report on the "mini-collection" (smaller piles) 
//   of the grouped data. 

//  Display albums group by release label, and release year. 
//  Display the release year and the number of albums.   
//  List only the years with two or mor albums released 
//  For each album, display the title, year of release and count of tracks,. 

//  Original collection (large pile of data:  Albums 
//  Filtering cannot be decided until the groups are created. 
//  Grouping:  ReleaseLabel, ReleaseYear (anonymous key set: object) 
//  Now filtering can be done on the group:  group.Count >= 2 
//  Report the year and number of albums 
//  Nested query to report details per album: Title, Year, # of tracks. 

Albums
	.GroupBy(a => new { a.ReleaseLabel, a.ReleaseYear })//  Creating anonymous key set 
	.Where(albumGroup => albumGroup.Count() >= 2)
	.OrderBy(albumGroup => albumGroup.Key.ReleaseLabel)
	.Select(album => new
	{
		Label = album.Key.ReleaseLabel,
		Year = album.Key.ReleaseYear,
		NumberOfAlbums = album.Count(),
		AlbumsGroupItem = album    //  smaller detail (mini collection) 
							.Select(albumInstance => new
							{
								Title = albumInstance.Title,
								Year = albumInstance.ReleaseYear,
								NumberOfTracks = albumInstance.Tracks.Count()//  Broken 
							})
	})



