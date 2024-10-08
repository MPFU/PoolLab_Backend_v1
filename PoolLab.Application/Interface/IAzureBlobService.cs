using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IAzureBlobService
    {
        Task<string> UploadFileImageAsync(string containerName, IFormFile file);

        Task<string> UploadQRCodeImageAsync(string containerName, byte[] fileBytes);
    }
}
