using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using NPOI.HPSF;
using System.IO;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Utilities
{
  public class GoogleDriveStorageUtilities
  {
    public static async Task<UploadFileResponse> UploadFileToGoogleDriveAsync(string fileCredential, string folder)
    {
      try
      {
        var credential = GoogleCredential.FromFile(fileCredential)
                                         .CreateScoped(DriveService.ScopeConstants.Drive);
        var service = new DriveService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credential
        });
        var fileUpload = new Google.Apis.Drive.v3.Data.File()
        {
          Name = fileCredential + Guid.NewGuid().ToString(),
          Parents = new[] { folder }
        };
        // Create a new file on Google Drive
        using (var stream = new FileStream(fileCredential, FileMode.Create))
        {
          // Create a new file, with metadata and stream.
          var request = service.Files.Create(fileUpload, stream, Path.GetExtension(fileCredential));
          request.Fields = "id";
          var results = await request.UploadAsync();
          var uploadedFile = request.ResponseBody;
          return new UploadFileResponse()
          {
            Success = true,
            FileId = uploadedFile.Id,
            UrlAccess = $"https://drive.google.com/file/{folder}/{uploadedFile.Id}"
          };
        }

      }
      catch (Exception ex)
      {
        return new UploadFileResponse()
        {
          Success = false,
          Message = "Upload thất bại",
        };
        throw;
      }
    }
  }
}
