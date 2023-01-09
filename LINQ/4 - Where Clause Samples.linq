<Query Kind="Expression" />

//  Where Clause 

//  Filter clause 
//  the conditions are set up as you would in C# 
//  Beware that LINQPad may NOT like some C# syntax (DateTime) 
//  Beware that LINQ is converted to SQL , which might not 
//  like certain C# syntax because it could not be converted/ 

//  Syntax 
//  Query 
//  Where condition [logical operator (and/or) condition 2 ...] 
//  Method 
//  Notice that the method syntax makes use of the Lambda expressions. 
//  .Where(Lambda expression) 
//  .Where(x => condition [logical operator (and/or) condition 2...] 

//  Find all albums that were released in the year 2000 
//  Query Syntax 
from x in Albums 
where x.ReleaseYear == 2000 
select x 

//  Method Syntax 
Albums 
    .Where(x => x.ReleaseYear == 2000) 
    .Select(x => x) 


//  Find all albums released in the 90s (1990 - 1999) 
//  Display all of the albums records 
Albums 
    .Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear < 2000) 
    .Select(x => x) 

// Find all of the albums for the artist Queen 
Albums 
    .Where(x => x.Artist.Name.Equals("Queen"))  //  Artist.Name == "Queen" 
    .Select(x => x)

//Concern:  The artist’s name is on another table 
//          In SQL, your select query would need an inner join. 
//          In Linq, you DO NOT want to specify your join unless absolutely 
//            necessary.  Instead, use the "navigational properties" of your entity to  
//            generate the relationship 

//  .Equals() is an exact match of a string.  You could also use the "==" 
//        in SQL = or like "String" 
//	.Contains() is a partial string match. 
//        in SQL like "%string%" 
//	For numerics use your relative operators (==, >, <, <=, >=, !=) 

//  Find all albums where the producer (release label) is unknown (Null) 
Albums
	.Where(x => x.ReleaseLabel == null) //  (release label) is unknown (Null) 
	.Select(x => x)