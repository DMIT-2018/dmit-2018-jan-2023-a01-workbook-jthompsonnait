//  Classes for holding our data.  
//	Note:	We are using the suffix "View" for our naming convention. 
//			The suffix "View" is used in "View Model" that we will be talking about later in the term
using System.Collections.Generic;

public class EmployeeView
{
	public string FullName { get; set; }
	public string Title { get; set; }
	public string Phone { get; set; }
	public List<CustomerView> Customers { get; set; }
}
