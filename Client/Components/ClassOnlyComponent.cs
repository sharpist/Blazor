using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace Blazor.Client.Components
{
    public class ClassOnlyComponent : IComponent
    {
        [Parameter] public int Value { get; set; }

        private RenderHandle renderHandle;
        
        void IComponent.Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            foreach (var entry in parameters)
                switch (entry.Name)
                {
                    case nameof(Value) :
                        Value = (int)entry.Value;
                        break;
                }

            this.renderHandle.Render((RenderTreeBuilder builder) => {
                var limiter = Math.Min(Value, 5);
                var content = "";
                while (0 < limiter--) content += '★';

                int seq = 1;

                builder.OpenElement(seq++, "span");
                builder.AddAttribute(seq++, "style", "color:#ff9a08;font-size:30px");
                builder.AddContent(seq++, content);
                builder.CloseElement();
            });

            return Task.CompletedTask;
        }
    }
}
