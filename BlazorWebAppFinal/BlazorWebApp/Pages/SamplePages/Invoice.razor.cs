using BlazorWebApp.ViewModel;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class Invoice
    {
        #region Fields
        private InvoiceView invoiceView;
        private string feedback;
        private int counter = 1;
        #endregion
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            invoiceView = new InvoiceView();
            Random rnd = new Random();
            invoiceView.InvoiceNo = rnd.Next(1000, 2000);
        }

        private void HandleSubmit()
        {
            feedback = $"Submitted Press - {counter++}";
        }

        private void HandleValidSubmit()
        {
            feedback = $"Valid Submit - {counter++}";
        }

        private void HandleInValidSubmit()
        {
            feedback = $"Invalid Submit - {counter++}";
           
        }
    }
}
