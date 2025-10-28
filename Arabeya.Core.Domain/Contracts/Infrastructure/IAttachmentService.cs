using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Core.Domain.Contracts.Infrastructure
{
    public interface IAttachmentService
    {
          Task<string?> UploadAsync(IFormFile file, string folderName);
        
    }
}
