using BlazorWebApp.ViewModel;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class SimpleNonIndexList
    {
        #region Fields

        private List<EmployeeView> employees { get; set; } = new ();
        
        private string employeeName { get; set; }
        #endregion

        public void RemoveEmployee(int employeeUD)
        {
            var selectedItem = employees.FirstOrDefault(x => x.EmployeeId == employeeUD);
            if (selectedItem != null)
            {
                employees.Remove(selectedItem);
            }
        }

        private async Task AddToEmployeeList()
        {
            int maxID = employees.Count == 0
                ? 1
                : employees.Count() + 1;
                //: employees.OrderBy(x => x.EmployeeId)
                //    .Max(x => x.EmployeeId) + 1;
            employees.Add(new EmployeeView() { EmployeeId = maxID, Name = employeeName });

        }
    }
}
