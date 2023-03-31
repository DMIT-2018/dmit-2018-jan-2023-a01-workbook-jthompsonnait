using PlaylistManagementSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class SimpleNonIndexList
    {
        #region Fields

        private List<EmployeeView> employees { get; set; } = new();
        private string employeeName { get; set; }
        #endregion

        private void RemoveEmployee(int employeeId)
        {
            var selectedItem =
                employees.FirstOrDefault(x => x.EmployeeId == employeeId);
            if (selectedItem != null)
            {
                employees.Remove(selectedItem);
            }
        }

        private async Task AddToEmployeeList()
        {
            int maxID = employees.Count == 0
                ? 1
                : employees.Max(x => x.EmployeeId) + 1;
            employees.Add(new EmployeeView()
            {
                EmployeeId = maxID,
                Name = employeeName
            });
            await InvokeAsync(StateHasChanged);
        }
    }
}
