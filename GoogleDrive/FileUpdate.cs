using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System;

namespace SystemPlus.GoogleDrive
{
    public static class FileUpdate
    {
        public static File UpdateFileData(DriveService service, string fileId, byte[] newData)
        {
            try
            {
                // First retrieve the file from the API.
                File file = service.Files.Get(fileId).Execute();

                // File's new metadata.
                //file.Title = "test 2.txt";
                //file.Description = newDescription;
                //file.MimeType = "text/plain";
                // File's new content.
                //byte[] byteArray = System.IO.File.ReadAllBytes(newFilename);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(newData);
                // Send the request to the API.
                FilesResource.UpdateMediaUpload request = service.Files.Update(file, fileId, stream, file.MimeType);
                //request.NewRevision = newRevision;
                request.Upload();

                File updatedFile = request.ResponseBody;
                return updatedFile;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
    }
}
