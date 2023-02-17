<Query Kind="Program">
  <Connection>
    <ID>53e8eb59-28e3-4a1d-80e2-c0d6c039f5d0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WestWind</Database>
    <DisplayName>WestWind</DisplayName>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	OrderDetails
	.OrderBy(od => od.Order.Customer.CompanyName)
	.Select(od => new
	{
		Name = od.Order.Customer.CompanyName,
		OrderID = od.OrderID,
		Date = od.Order.OrderDate,
		OrderDetailID = od.OrderDetailID,
		Product = od.Product.ProductName,
		Qty = od.Quantity,
		Price = od.UnitPrice,
		ExtPrice = od.Quantity * od.UnitPrice
	}).Dump();
}

