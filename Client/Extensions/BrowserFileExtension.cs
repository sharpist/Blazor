using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Blazor.Client.Extensions
{
    public static class BrowserFileExtension
    {
        public static async Task<Byte[]> ToBytesAsync(this IBrowserFile rawFile,
            string format = "image/jpeg", int width = 640, int height = 480)
        {
            if (rawFile is null)
                throw new ArgumentNullException(nameof(rawFile));

            // 3mb maximum size in bytes to prevent allocating too much memory on the server
            var maxSize = 3 * 1024 * 1024;
            if (rawFile.Size > maxSize)
                throw new ArgumentOutOfRangeException(nameof(rawFile),
                    $"The maximum allowed size is {maxSize}, but the supplied file is of length {rawFile.Size}.");

            // convert to reasonably-sized JPEG
            var imageFile = await rawFile.RequestImageFileAsync(format, width, height);

            // image file loading to memory stream and byte buffer writing
            var buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream().ReadAsync(buffer);
            return buffer;
        }
    }
}
