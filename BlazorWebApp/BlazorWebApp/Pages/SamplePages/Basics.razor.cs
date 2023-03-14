namespace BlazorWebApp.Pages.SamplePages
{
    public partial class Basics
    {
        #region Fields
        private string myName = string.Empty;
        private int oddEven;
        #endregion

        //  Method invoked when the component is ready to start having
        //      received it initial parameters from its parent in the render
        //      tree.
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Random rnd = new Random();
            int oddEven = rnd.Next(0, 25);
            if (oddEven % 2 == 0)
            {
                myName = $"James is even {oddEven}";
            }
            else
            {
                myName = null;
            }
        }
        private void RandomValue()
        {
            Random rnd = new Random();
            oddEven = rnd.Next(0, 25);
            if (oddEven % 2 == 0)
            {
                myName = $"James is even {oddEven}";
            }
            else
            {
                myName = null;
            }
        }
    }
}
