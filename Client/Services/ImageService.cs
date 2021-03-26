using Blazor.Client.Extensions;
using Blazor.Shared;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Blazor.Client.Services
{
    public class ImageService
    {
        public ImageModel  Model   { get; }
        public EditContext Context { get; }

        private readonly HttpClient http;

        public ImageService(HttpClient http)
        {
            Model     = new();
            Context   = new(Model);
            this.http = http;
        }

        private List<ImageData> images = new();
        public IList<ImageData> Images => images;

        private string message = "No file(s) selected";
        public  String Message => message;

        public async Task Read(InputFileChangeEventArgs e)
        {
            Model.Picture = e.GetMultipleFiles().ToArray();

            images.AddRange(
                await Task.WhenAll(
                    Model.Picture.Select(async (file) => {
                        var name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                        var data = file.ToBytesAsync();
                        return new ImageData(name, await data, "image/jpeg");
                    })));

            // The EditContext class object holds metadata and
            // when adding any file to the InputFile component it signals the EditForm component that the Model field is modified
            Context.NotifyFieldChanged(FieldIdentifier.Create(() => Model.Picture));

            message = $"{images.Count} file(s) selected";
        }

        public async Task Save()
        {
            foreach (var image in Images)
                await http.PostAsJsonAsync<ImageData>("/image", image);

            message = $"{images.Count} file(s) uploaded on server";
        }

        public async Task Load()
        {
            images.AddRange(await http.GetFromJsonAsync<List<ImageData>>("/image"));

            message = $"{images.Count} file(s) downloaded from server";
        }

        public class ImageModel
        {
            [Required]
            [FileValidation(new[] { "png", "jpg" })]
            public IBrowserFile[] Picture { get; set; }
        }

        private class FileValidationAttribute : ValidationAttribute
        {
            private string[] allowedExtensions { get; }
            public FileValidationAttribute(string[] allowedExtensions) =>
                this.allowedExtensions = allowedExtensions;

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                foreach (IBrowserFile file in (IBrowserFile[])value) {
                    var extension = System.IO.Path.GetExtension(file.Name).Substring(1);

                    if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                        return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", allowedExtensions)}.", new[] { validationContext.MemberName });
                }
                return ValidationResult.Success;
            }
        }
    }
}
