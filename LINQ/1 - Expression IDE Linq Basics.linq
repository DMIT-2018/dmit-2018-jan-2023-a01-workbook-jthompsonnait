<Query Kind="Expression" />

//  our code is using C# grammar/syntax   

//  Comments are made with slashes   
//  hotkey to comment ctrl+k,ctrl+c   
//  uncomment ctrl+k,ctrl+u   
//  alternatively use ctrl+/ as a toggle   

//  Expression IDE  
//  Single LINQ query statements without a semi-colon  
//  you can have multiple statements in your file but  
//  if you do so, you MUST highlight the statement to execute  

//  Executing:  use F5 or the green triangle on the query menu.  
//  If the query seems to be not ending, you can use the red square to terminate.  

//  to toggle your results on and off, use ctrl+r  

// Query Syntax   
//  uses a "SQL-like" syntax  
//  view the Student notes for examples  
from album in Albums   
select album 

//  Method Syntax   
Albums   
    .Select(x => x)  

//  Find all albums released in 2000.  
//  Display the entire records  

//  query  
from x in Albums 
where x.ReleaseYear == 2000  //  we need two equals signs for a compare (C# grammar/syntax)  
select x 

//  method syntax (Collections)  
//  uses C# method syntax (OOP language grammar)  
//  to execute a method on a collection, you need to use   
//  the access operator (dot operator)  
//  This results in the return of an other collection from the method !!==  
//  Method name starts with a capitals  
//  Methods contains contents as a delegate  
//  a delegate describes the action to be done.  

Albums
	.Where(x => x.ReleaseYear == 2000)
	.Select(x => x)

