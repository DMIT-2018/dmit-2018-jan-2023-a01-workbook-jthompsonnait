<Query Kind="Program" />

void Main()
{
	//  Program IDE
	//  You can have multiple queries written in this IDE environment
	//  This environment works "like" a console application 

	//  This allows one to complete pre-test components that can be 
	//  moved directly into your backend application (class library)

	//IMPORTANT:  Query Syntax
	//  Queries in this environment MUST be written using the
	//  C# langauge grammar for a statement.  This means that
	//  each statement must end in a semi-colon.
	//  Result MUST be placed in a receiving variable.
	//  To display the results, use the LINQPad method .Dump();

	//  Query Syntax 
	//int paramYear = 1990;  //  image this is a parameter on a method signature.
	//var selectQ = from x in Albums
	//			  where x.ReleaseYear == paramYear  //  we need two equals signs for a compare (C# grammar/syntax) 
	//			  select x;
	//selectQ.Dump();  //  image that this is the return statement in a method.



	List<Albums> resultSQ = GetAllAlbumQ(2000);
	resultSQ.Dump();



	List<Albums> resultMQ = GetAllAlbumM(2000);
	resultMQ.Dump();

}

#region Methods wr
//  image this is a method in your BLL server.
List<Albums> GetAllAlbumQ(int paramYear)
{
	var resultQ = from x in Albums
				  where x.ReleaseYear == paramYear  //  we need two equals signs for a compare (C# grammar/syntax) 
				  select x;
	return resultQ.ToList();
}

// Method Syntax
List<Albums> GetAllAlbumM(int paramYear)
{
	var selectM = Albums
	.Where(x => x.ReleaseYear == paramYear)
	.Select(x => x);
	return selectM.ToList();
}