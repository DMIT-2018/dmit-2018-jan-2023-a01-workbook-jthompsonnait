using ChinookSystem.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class AdvancedInvoice
    {
        enum PaymentTypes
        {
            Unknown,
            Cash,
            Chq,
            Creditcard
        }
        #region Fields
        private InvoiceView invoice;
        private string feedBack;
        private int counter = 1;

        //  Holds metadata related to a data editing process, such as flags to indicate
        //      which fields have been modified and the current set of validation messages
        private EditContext? editContext;

        //  used to store the validation messages
        private ValidationMessageStore? messageStore;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            invoice = new InvoiceView();
            Random rnd = new Random();
            invoice.InvoiceNo = rnd.Next(1000, 2000);

            //  edit context needs to be setup after data has been initialized
            //  the setup will use the invoice property/object
            editContext = new EditContext(invoice);
            //  set the validation to used the HandleValidationRequested event
            editContext.OnValidationRequested += HandleValidationRequested;
            //  setup the message store to track any validation messages
            messageStore = new ValidationMessageStore(editContext);

        }

        //  Handles the validation requested.  This allows for custom
        //      validation outside of using the DataAnnotationsValidator
        private void HandleValidationRequested(object? sender,
            ValidationRequestedEventArgs args)
        {
            // clear the message store if there is any existing validation errors.
            messageStore?.Clear();
            
            //  custom validation logic
            //  payment type cannot be set to "Unknown"
            if (invoice.PaymentType == "Unknown")
            {
                messageStore?.Add(() => invoice.PaymentType, "Payment type cannot be set to unknown");
            }
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
