using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Client.Pages
{
    public partial class JsInDotNet : IAsyncDisposable
    {
        [Inject] public IJSRuntime   JSRuntime    { get; set; }
        [Inject] public ImageService ImageService { get; set; }

        private IJSObjectReference jsObjRef;
        private ElementReference   carouselRef;
        // prefix id with 'id_' to make sure id's don't start with a number or other invalid character
        // add Guid to ensure unique-ness but remove '-' as they are invalid characters for JS property names
        private string carouselId = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";
        private string text;

        /* Call JS functions from Blazor */
        private async Task PrintMessage() =>
            await JSRuntime.InvokeVoidAsync("jsInDotNet.setText", "Hello from .NET");

        /* Call JS functions with return value from .NET */
        private async Task PrintReturnValue() =>
            text = await JSRuntime.InvokeAsync<string>("jsInDotNet.getValue");

        /* Call JS functions with IJSObjectReference */
        protected override async Task OnInitializedAsync() => await ImageService.Load();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) {
                var dotNetObjRef = DotNetObjectReference.Create(this);
                jsObjRef = await JSRuntime.InvokeAsync<IJSObjectReference>("jsInDotNet.createCarousel");
                await jsObjRef.InvokeVoidAsync("init", dotNetObjRef, carouselRef);
            }
        }

        [JSInvokable] public void OnSlideRoot(string direction, int from, int to) =>
            Console.WriteLine($"Carousel is sliding from {from} to {to}.");

        [JSInvokable] public void OnSlidRoot(string direction, int from, int to) =>
            Console.WriteLine($"Carousel slid from {from} to {to}.");

        public async ValueTask DisposeAsync()
        {
            await jsObjRef.InvokeVoidAsync("dispose");
            await jsObjRef.DisposeAsync();
        }
    }
}
