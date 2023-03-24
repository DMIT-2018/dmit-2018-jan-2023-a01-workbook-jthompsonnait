#nullable disable
using BlazorWebApp.ViewModel;

namespace BlazorWebApp.Pages.SamplePages
{
    public partial class Basics
    {
        #region Private
        private string myName;
        private int oddEven;
        private string feedback;
        private int MyRide { get; set; }

        //  pretend tat the following collection is data from a database
        //  The collection is based on a 2 property class called SelectionList
        //  The data for the list will be created in a separate method.
        private List<SelectionView> rides { get; set; }
        private string vacationSpot { get; set; }
        private List<string> vacationSpots { get; set; }
        private string emailText { get; set; }
        private string passwordText { get; set; }
        private DateTime? dateText { get; set; } = DateTime.Today;
        private TimeSpan timeText { get; set; } = DateTime.Now.TimeOfDay;
        private bool acceptanceBox { get; set; }

        private string messageBody { get; set; }
        private string meal { get; set; }

        //  assume this array is actually data retrieved from the database
        private string[] meals { get; set; } = new string[] { "breakfast", "lunch", "dinner", "snacks" };
        private int reviewRating { get; set; }
        #endregion

        // Method invoked when the component is ready to start,
        // having received its initial parameters from its parent in the render tree.
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            RandomValue();
            PopulatedList();
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

            InvokeAsync(StateHasChanged);
        }

        private void PopulatedList()
        {
            int i = 1;
            //  Create a pretend collection from the database represents different types
            //      of transportation (rides)
            rides = new List<SelectionView>();
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Car" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Bus" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Bike" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Motorcycle" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Boat" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Plane" });
            rides.Sort((x, y) => x.DisplayText.CompareTo(y.DisplayText));

            vacationSpots = new List<string>();
            vacationSpots.Add("California");
            vacationSpots.Add("Caribbean");
            vacationSpots.Add("Cruising");
            vacationSpots.Add("Europe");
            vacationSpots.Add("Florida");
            vacationSpots.Add("Mexico");

        }

        private void TextSubmit()
        {
            feedback = $"Email {emailText}; Password {passwordText}; Date {dateText}; Time {timeText}";
            InvokeAsync(StateHasChanged);
        }

        private void RadioCheckAreaSubmit()
        {
            feedback = $"meal {meal}; Acceptance {acceptanceBox}; Message {messageBody}";
            InvokeAsync(StateHasChanged);
        }

        private void SliderSubmit()
        {
            feedback = $"Ride {MyRide}; Vacation {vacationSpot}; Review Rating {reviewRating}";
            InvokeAsync(StateHasChanged);
        }
    }
}
/*
 <div class=onethird>
        <h3>Radiobuttons CheckBox TextArea</h3>

        <label>
            Select favourite meal<br />
        </label>
        @*<input type="radio" value="breakfast" bind-Value="meal"/>
            &nbsp;Breakfast<br/>
            <input type="radio" value="lunch" bind-Value="meal"/>
            &nbsp;Lunch<br/>
            <input type="radio" value="Supper" bind-Value="meal"/>
            &nbsp;Supper<br/>
            <input type="radio" value="Snacks" bind-Value="meal"/>
            &nbsp;Snacks<br/>
            *@
        @foreach (var item in meals)
        {
                @*  Need a space before the display item *@
            <input type="radio" value="@item" bind-Value="meal" /> @item

            <br />
        }
        <label>
            <InputCheckbox  value="true" bind-Value="AcceptanceBox" />
            &nbsp; I agree to terms
        </label>
        <br />
        <label>
            Message
            <textarea rows="5" cols="50" placeholder="enter message"
                          bind-Value="MessageBody"></textarea>
        </label>
        <button class="btn-success" @onclick="RadioCheckAreaSubmit">
            Radio/Check/Area Submit
        </button>
    </div>
    <div class=onethird>
        <h3>List and Slider</h3>
        <label>
            Slect your favourite Ride
            <select bind-Value="MyRide">
                <option value="0">Select ride ...</option>
                @foreach (var ride in rides)
                {
                    <option value="@ride.ValueID">@ride.DisplayText</option>
                }
            </select>
        </label>
        <br/>
        <label>
            Select your favorite Vacation Spot
            <input type="text" bind-Value="VacationSpot"
                   list="VacationChoices" />
            <datalist id="VacationChoices">
                @foreach (var spot in vacationSpots)
                {
                    <option value="@spot"></option>
                    //  <option value="@spot">@spot</option>
                }
            </datalist>
        </label>
        <br/>
        <label>
            Rate the form control review (bad to good)
            <input type="range" min="0" max="10" step="1" value="5"
                   bind-Value="ReviewRating"/>
        </label>
        <button class="btn-success" @onclick="SliderSubmit">
            Slider
        </button>
    </div>
 */

/*

<EditForm>
<section class="setflex">
    <div class=onethird>
        <h3>TextBoxes</h3>
        <label>
            Enter an Email

        </label>
        <InputText type="email" 
                   @bind-Value="emailText"
                   placeholder="enter email" />
        <br />
       @* <label>
            Enter an password
            <InputText type="password" bind-Value="passwordText"
                   placeholder="enter password" />
        </label>
        <br />
        <label>
            Enter an Date
            <InputText type="date" bind-Value="dateText" />
        </label>
        <br />
        <label>
            Enter an Time
            <InputText type="time" bind-Value="timeText" />
        </label>
        <br />*@
            
        <button class="btn-success" @onclick="TextSubmit">
            Text Submit
        </button>
    </div>
    
</section>
</EditForm>
*/