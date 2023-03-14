using ChinookSystem.ViewModels;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class Invoice
    {
        #region Fields
        private InvoiceView invoice;
        private string feedBack;
        private int counter = 1;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            invoice = new InvoiceView();
            Random rnd = new Random();
            invoice.InvoiceNo = rnd.Next(1000, 2000);
        }

        private void HandleSubmit()
        {
            feedBack = $"Submitted Press - {counter++}";
        }
        private void HandleValidSubmit()
        {
            feedBack = $"Valid Submit - {counter++}";
        }
        private void HandleInValidSubmit()
        {
            feedBack = $"Invalid Submit - {counter++}";
        }
    }
}
