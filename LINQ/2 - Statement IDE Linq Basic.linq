<Query Kind="Statements" />

//  Statement IDE

//  You can have multiple queries written in this IDE environment
//  You can execuate a query individually by highlighting the desired
//  query first
//  BY DEFUALT executing the file in this environment will execute
//  ALL queries (statement) top to bottom.

//IMPORTANT:  Query Syntax
//  Queries in this environnment MUST be wrtten using the
//  C# langauge grammer for a statement.  This mean that
//  each statement must end in a semi-colon.
//  Result MUST be placed in a receiving variable.
//  To display the results, use the LinqPad method .Dump();

//  It appears that Method syntax does NOT need a semi-colon on the 
//  query..
//  Does not need result placed in a receiving variable
//  HOWEVER it does need the .Dump() method to display results.
//  This mabye a bug in LinqPad 7)

//  Find all albums release in 2000.
//  Display the entire album records

//  Query Syntax 
int paramYear = 1990;  //  image this is a parameter on a method signature.
var selectQ = from x in Albums
			  where x.ReleaseYear == paramYear  //  we need two equals signs for a compare (C# grammar/syntax) 
			  select x;
selectQ.Dump();  //  image that this is the return statement in a method.

//  Method Syntax
paramYear = 2000;
var selectM = Albums
	.Where(x => x.ReleaseYear == paramYear)
	.Select(x => x);
selectM.Dump();

//  Also written without saving to variable
Albums
   .Where(x => x.ReleaseYear == paramYear)
   .Select(x => x).Dump();