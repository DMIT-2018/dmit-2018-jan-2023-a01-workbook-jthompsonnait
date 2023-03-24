#nullable disable
using BlazorWebApp.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class AdvanceInvoice
    {
        enum PaymentTypes
        {
            Unknown,
            Cash,
            Chq,
            CreditCard
        }

        //  need to inject the NavigationManager so we can do a redirect back to query page.
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        #region Fields
        private InvoiceView invoiceView;
        private string feedback;
        private int counter = 1;

        /// <summary>
        /// The message store
        /// Used to store the validation messages
        /// </summary>
        private ValidationMessageStore? messageStore;

        /// <summary>
        /// The edit context
        /// Holds metadata related to a data editing process,
        /// such as flags to indicate which fields have been modified
        /// and the current set of validation messages.
        /// </summary>
        private EditContext? editContext;
        #endregion

        #region Properties
        [Parameter]
        public EventCallback<string> OnSelectionChanged { get; set; }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            invoiceView = new InvoiceView();
            Random rnd = new Random();
            invoiceView.InvoiceNo = rnd.Next(1000, 2000);

            //  setup the edit content to make use of the rolling stock info property
            editContext = new EditContext(invoiceView);
            //  set the validation to use the HandleValidationRequested event 
            editContext.OnValidationRequested += HandleValidationRequested;
            //  setup the message store to track any validation messages
            messageStore = new ValidationMessageStore(editContext);
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

        // Handles the validation requested.  This allows for custom validation outside
        // of using the DataAnnotationsValidator 
        private void HandleValidationRequested(object? sender,
            ValidationRequestedEventArgs args)
        {
            //  clear the message store if there is any existing validation errors.
            messageStore?.Clear();

            //  custom validation logic
            //  payment type cannot be set to "Unknown"
            if (invoiceView.PaymentType != null && invoiceView.PaymentType == PaymentTypes.Unknown.ToString())
            {
                messageStore?.Add(() => invoiceView.PaymentType, "Payment Type cannot be set to Unknown");
            }
        }

        /// <summary>
        /// Called when [payment type click].
        /// </summary>
        private async void OnPaymentTypeClick()
        {
            //  waiting for payment type to finish changing
            await OnSelectionChanged.InvokeAsync(invoiceView.PaymentType);
            //  manually call the validation for the edit context
            editContext?.Validate();
        }
    }
}