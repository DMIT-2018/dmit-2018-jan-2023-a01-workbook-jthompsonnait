<Query Kind="Statements" />

//  Sorting

//  There is a significant difference between the query and method syntax

//  Query syntax is very much like SQL
//  orderby field {[ascending (default) | descending} [, field
//  {[ascending] | descending}...]
//  ascending is the default.

//  Method syntax is a series of individual methods.
//  start with
//  .OrderBy(x => x.field)				//  ascending
//	.OrderByDescending(x => x.field)	//  descending

//  After one of these two beginning methods, if you have other field(s)
//  .ThenBy(x => x.field)				//  ascending
//  .ThenByDescending(x => x.field)		//	descending.

//  FInd all albums released in the 90s (1990 - 1999)
//  order by the albums in ascending years and then
//  alphabetically by album title.

//  Query syntax.
(from x in Albums
 where x.ReleaseYear >= 1990 && x.ReleaseYear < 2000
 orderby x.ReleaseYear, x.Title
 select x).Dump();

//  Method Syntax
Albums
	.Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear < 2000)
	.OrderBy(x => x.ReleaseYear)
	.ThenBy(x => x.Title)
	.Select(x => x).Dump();

//  Query syntax.
(from x in Albums
 where x.ReleaseYear >= 1990 && x.ReleaseYear < 2000
 orderby x.ReleaseYear, x.Title descending
 select x).Dump();

//  Method Syntax
Albums
	.Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear < 2000)
	.OrderBy(x => x.ReleaseYear)
	.ThenByDescending(x => x.Title)
	.Select(x => x).Dump();
