<Query Kind="Statements" />

// Using Navigational Properties and Anonymous data set (Collections) 

//  Reference: Student Note/Demo/ERestaurants/Linq - Query and Method 
//    Syntax/C# Expression 

//  Method Syntax 
//  Find all albums released in the 90s (1990 - 2000) 
//  Order the albums by ascending years and then alphabetically 
//  by album title 
//  Display the year, title, artist name, and release date 

//  Concerns:    a) not all properties of the album are to be displayed 
//                b) the order of the properties is to be displayed 
//                    in a different sequence than the definition of 
//                    the properties of the entity 
//                c) The artist’s name is not on the album table but 
//                    on the artist’s table 

//    Solution:    Use an anonymous data set. 
//    The anonymous instance is defined within the select by 
//     in the declared fields (properties) 
//    The order of the fields on the instance is defined during the 
//     specification of your code.  (How you list the fields is how 
//     they will be displayed). 

// Method Syntax 
Albums 
    .Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear< 2000) 
    .OrderBy(x => x.ReleaseYear) 		//  Note that we are sorting on the data field "ReleaseYear"
    .ThenByDescending(x => x.Title) 	//  Note that we are sorting on the data field "Title"
    .Select(x => new 
    { 
        Year = x.ReleaseYear, 
        AlbumTitle = x.Title, 			//  Rename to reflect the use of the ".ThenByDescending" for the next method "Sorting on the collection"
        Artist = x.Artist.Name, 
        Label = x.ReleaseLabel 
    } 
    ).Dump(); 

//  Sorting on the collection 
Albums
	.Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear < 2000)
	.Select(x => new
	{
		Year = x.ReleaseYear,
		AlbumTitle = x.Title,			//  Rename to reflect the use of the ".ThenByDescending" for the this method
		Artist = x.Artist.Name,
		Label = x.ReleaseLabel
	}
	).OrderBy(x => x.Year) 			//  Note that we are sorting on the anonymous field/property "Year"
	.ThenByDescending(x => x.AlbumTitle)	//  Note that we are sorting on the anonymous field/property "AlbumTitle"
	.Dump();


