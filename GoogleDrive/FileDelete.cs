using Google.Apis.Drive.v2;
using System;

namespace SystemPlus.GoogleDrive
{
    public static class FileDelete
    {
        public static void DeleteFile(DriveService service, string fileId)
        {
            try
            {
                service.Files.Delete(fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }
}
