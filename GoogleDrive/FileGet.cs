using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;

namespace SystemPlus.GoogleDrive
{
    public static class FileGet
    {
        public static bool Exists(DriveService service, string fileId)
        {
            try
            {
                service.Files.Get(fileId).Execute();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static File GetFile(DriveService service, string fileId)
        {
            try
            {
                return service.Files.Get(fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }

        public static List<File> GetAllFiles(DriveService service, int resultsPerPage = 500)
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.MaxResults = resultsPerPage;
            List<File> result = new List<File>();

            do
            {
                try
                {
                    FileList files = listRequest.Execute();

                    result.AddRange(files.Items);
                    listRequest.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    listRequest.PageToken = null;
                }
            } while (!string.IsNullOrEmpty(listRequest.PageToken));

            return result;
        }

        public static List<File> GetAllFiles(DriveService service, Action<int> log, int resultsPerPage = 500)
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.MaxResults = resultsPerPage;
            List<File> result = new List<File>();

            int index = 0;

            do
            {
                try
                {
                    FileList files = listRequest.Execute();
                    log.Invoke(index);

                    result.AddRange(files.Items);
                    listRequest.PageToken = files.NextPageToken;
                    index++;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    listRequest.PageToken = null;
                }
            } while (!string.IsNullOrEmpty(listRequest.PageToken));

            return result;
        }

        public static System.IO.Stream DownloadFile(DriveService service, string fileId, out IDownloadProgress progress)
        {
            System.IO.Stream stream = new System.IO.MemoryStream();
            progress = service.Files.Get(fileId).DownloadWithStatus(stream);

            return stream;
        }
    }
}
