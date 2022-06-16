using Google.Apis.Drive.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
