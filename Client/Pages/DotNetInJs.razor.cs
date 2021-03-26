using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Blazor.Client.Pages
{
    public partial class DotNetInJs
    {
        // for calling non-static C# methods must use an DotNetObjectReference object which pass to JS
        [Inject] public IJSRuntime        JSRuntime  { get; set; }
        [Inject] public HttpClient        Http       { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        private MouseCoordinates coordinates = new();

        protected override Task OnInitializedAsync()
        {
            var dotNetObjRef = DotNetObjectReference.Create(this);
            JSRuntime.InvokeVoidAsync("dotNetInJS.setDotNetReference", dotNetObjRef);
            return base.OnInitializedAsync();
        }

        /* Calling static C# methods from JS */
        [JSInvokable] // the attribute indicates this method as invokable by JS
        public static string CalculateSquareRoot(int number) =>
            $"The square root of {number} is {Math.Sqrt(number)}";

        /// <summary>
        /// The identifier consists of the assembly name and the method name.
        /// This identifier must be unique across the entire assembly, so having
        /// another class with the same-named method in the same assembly won’t work.
        /// This is solved by another attribute overload with specifying an identifier.
        /// </summary>
        /* Calling static C# methods from JS with Custom Identifier */
        [JSInvokable("CalculateSquareUnformattedRoot")]
        public static string CalculateSquareRoot(int number, bool justResult)
        {
            var result = Math.Sqrt(number);

            return !justResult ?
                $"The square root of {number} is {result}" :
                result.ToString();
        }

        /* Call .NET class instance methods from JS */
        private async Task SendDotNetInstanceToJS()
        {
            var dotNetObjRef = DotNetObjectReference.Create(new JsInDotNetBridge());
            await JSRuntime.InvokeVoidAsync("dotNetInJS.getPersonObject", dotNetObjRef);
        }

        public class JsInDotNetBridge // class embedded for demo purposes, move this class out of your razor component
        {
            [JSInvokable] public object GetPersonRoot() => new {
                FistName    = "Alexander",
                LastName    = "Usov",
                Age         = 34,
                BirthYear   = DateTime.Now.AddYears(-34).Year,
                LikesDotNet = true
            };
        }

        /* Call Async methods .NET from JS */
        [JSInvokable] public async Task<object> GetWeatherDataRootAsync() =>
            await Http.GetFromJsonAsync<object>($"{Navigation.BaseUri}/sample-data/weather.json");

        /* Calling C# instance methods from JS */
        [JSInvokable] public void ShowCoordinatesRoot(MouseCoordinates coordinates)
        {
            this.coordinates = coordinates;
            StateHasChanged();
        }
    }

    public record MouseCoordinates
    {
        public int X { get; init; }
        public int Y { get; init; }
    }
}
