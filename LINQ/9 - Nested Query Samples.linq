<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

//  Used to load the EmployeeView and CustomerView models from the classes folder
#load "..\LINQ\ViewModels\*.cs"

//  ***** Might have to paste the following and accept to add it to the query prpoerty *****
//  using System.Collections.Generic;

void Main()
{
	//  Nested Queries 
	//  Sometimes referred to as sub-queries 

	//  Simply put:  It is a query within a query.  [query in a query in a query etc.] 

	//  List all sales support employees showing their 
	//  FullName (last, first), Title, Phone # 
	//  For each employees show a list of their customers 
	//   that they support. 
	//  FullName (last, first), City, State/Prov. 

	//  This is a non nested query (Flatten List)
	Console.WriteLine("Top 20 Non Nested Query (Flatten List)");
	var results = Customers
			.Where(c => c.SupportRep.Title.Contains("Sales Support"))
			.OrderBy(c => c.SupportRep.LastName)
			.ThenBy(c => c.SupportRep.FirstName)
			.Select(c => new
			{
				SupportRepFullName = c.SupportRep.LastName + ", " + c.SupportRep.FirstName,
				Title = c.SupportRep.Title,
				Phone = c.SupportRep.Phone,
				FullName = c.LastName + ", " + c.FirstName,
				City = c.City,
				State = c.State
			}).ToList()
			.Take(20);
	results.Dump();

	//  However, what we really want is a nested query so that the support employee is only shown 
	//        once and their customers as children are shown underneath them.

	//	Smith, John Sales Support 7801234567		// This is the employee 
	//  	Kan, Jerry Edmonton Ab            		// Customer 
	//      Thom, Sue Devon Ab  					// Customer 
	//      Apple Mary Edmonton Ab  				// Customer 
	//      Low, Mike Calgary Ab  					// Customer 

	//  Smith, Bob  Chair 7805551212  				// Employees 
	//		Can, Mark Edmonton Ab  					// Customer 
	//      Freddy, Shelly Devon Ab					// Customer 
	//      Micro, Bill Edmonton Ab  				// Customer 
	//      Peterson, Cindy Calgary Ab  			// Customer 

	//  There appear to be two separate lists that needs to be 
	//   within one final dataset/collection 
	//		One for the employees 
	//  	One for the customers. 

	//  Concerns:  The list is intermixed!!!! 

	//  C# point of view in a class definition 
	//  A composite class can have a single occurring field AND use of other classes. 
	//  OTHER classes may be a single instance OR collection<T> 
	//  List<T>, IEnumerable<T>, IQueryable<T> is a collection with a difine datatype of <T> 

	//  ClassName 
	//  Property 
	//  Property 
	//  Collection<T> (set of records, but it is still a property) 
	Console.WriteLine("Nested Query");
	var nestedResults = Employees
		.Where(e => e.Title.Contains("Sales Support"))
		.Select(e => new EmployeeView
		{
			FullName = e.LastName + ", " + e.FirstName,
			Title = e.Title,
			Phone = e.Phone,
			Customers = Customers
							.Where(c => c.SupportRepId == e.EmployeeId)
							.Select(c => new CustomerView
							{
								FullName = c.LastName + ", " + c.FirstName,
								City = c.City,
								State = c.State
							}).ToList()
		});
	nestedResults.Dump();
}