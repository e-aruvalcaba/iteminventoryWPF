using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ItemInventoryApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ItemInventoryApp.Classes
{
    class LDrive
    {

        //CREDENCIALES PARA LA CUENTA DE GOOGLE DRIVE:
        //USER: ItemInventoryApp@Gmail.com ----NOMBRE ADRIAN RUVALCABA
        //PASS: Nintendo_64.
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Item Inventory App";


        #region Constructores
        public LDrive()
        {

        }

        //public LDrive(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        //                RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //    _userRole = new UserRoles();
        //}
        #endregion

        #region Inicializacion y MimeType
        //Obtiene el tipo de MIME de archivo necesario para el upload de los archivos.
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public DriveService inizialiceDriveService()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public DriveService inizialiceDriveServiceAsync()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public bool IsDriveSync()
        {
            return Directory.Exists("token.json");
        }
        #endregion

        #region Operaciones con archivos DRIVE

        public async Task<List<DriveFileModel>> DBExistsOnDrive()
        {
            List<DriveFileModel> files = new List<DriveFileModel>();

            try
            {
                files = await searchRepositoryfFiles("_InventoryDB.json", "name");
            }
            catch (Exception)
            {

            }

            return files;
        }

        public async Task<List<DriveFileModel>> searchRepositoryfFiles(string searchFilter, string typeFilter)//agregar tipo de busqueda, actual=Nombre de archivo, agregar Username, fecha de subida, tipo de archivo
        {
            List<DriveFileModel> Lista = new List<DriveFileModel>();

            //Inicializar el servicio de Drive
            var service = inizialiceDriveService();

            typeFilter.ToLower();

            string pageToken = null;
            do
            {
                var request = service.Files.List();
                switch (typeFilter)
                {
                    case "GetRepositoryFiles":
                        request.Q = "mimeType != 'application/vnd.google-apps.folder'";
                        break;
                    case "nameUpload":
                        request.Q = "name = '" + searchFilter + "'";
                        break;
                    case "name":
                        request.Q = "name contains '" + searchFilter + "'";
                        break;
                    case "username":
                        request.Q = "appProperties has { key='uploader' and value=' '" + searchFilter + "'}'";
                        break;
                    case "pdf":
                    case "mp3":
                    case "jpeg":
                    case "png":
                    case "mp4":
                    case "exe":
                    case "txt":
                        if (string.IsNullOrEmpty(searchFilter))
                        {
                            request.Q = "name contains '" + searchFilter + "." + typeFilter + "'";
                        }
                        else
                        {
                            request.Q = "name contains '" + searchFilter + "' and mimeType contains '" + typeFilter + "'";
                        }
                        break;
                    case "date":
                        var dia1 = Convert.ToDateTime(searchFilter).AddDays(1);
                        var search2 = Convert.ToDateTime(dia1).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                        request.Q = "modifiedTime > '" + searchFilter + "' and modifiedTime < '" + search2 + "'";
                        break;
                    case "datemas":
                        request.Q = "modifiedTime > '" + searchFilter + "'";
                        break;
                    case "datemenos":
                        request.Q = "modifiedTime < '" + searchFilter + "'";
                        break;
                }

                request.Spaces = "drive";
                request.Fields = "nextPageToken, files";
                request.PageToken = pageToken;
                var result = await request.ExecuteAsync();
                foreach (var file in result.Files)
                {
                    string uploader = "";
                    try
                    {
                        file.Properties.TryGetValue("uploader", out uploader);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        uploader = "No uploader Information";
                    }

                    Lista.Add(new DriveFileModel
                    {
                        TableId = (Lista.Count + 1).ToString(),
                        Fileid = file.Id,
                        UserName = uploader,
                        FileName = file.Name,
                        DownloadLink = (file.WebContentLink != null ? file.WebContentLink : "This file can't be donwloaded."),
                        CreatedTime = (file.CreatedTime != null ? file.CreatedTime.ToString() : "This file don't have this information"),
                        Modified = (file.ModifiedTime != null ? file.ModifiedTime.ToString() : "This file don't have last modification."),
                        Description = (file.Description != null ? file.Description : "This file don't have description."),

                    });
                }
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            return Lista;
        }

        public async Task<String> DownloadStream(string fileID)
        {
            var service = inizialiceDriveService();
            var fileId = fileID;
            var request = service.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();
            string resp = "";
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");

                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };

            await request.DownloadAsync(stream);
            stream.Position = 0;

            using (StreamReader sr = new StreamReader(stream))
            {
                resp = sr.ReadToEnd();
            }

            return resp;
        }

        //public async Task<bool> subir(UploadFileModel uploadFile, string username, string path)
        //{
        //    var service = inizialiceDriveService();
        //    var success = false;
        //    var temp = Path.GetTempPath();
        //    var fileName = Path.GetFileName("");
        //    //var fileName = Path.GetFileName(uploadFile.Input.FileUrl.FileName);
        //    var path = temp + fileName;
        //    var newFile = System.IO.File.Create(temp + fileName);
        //    await File.Copy(path, newFile.); // uploadFile.Input.FileUrl.OpenReadStream().CopyToAsync(newFile);
        //    //await uploadFile.Input.FileUrl.OpenReadStream().CopyToAsync(newFile);
        //    newFile.Close();

        //    var busqueda = await searchRepositoryfFiles(fileName, "nameUpload");

        //    IDictionary<string, string> dictionary = new Dictionary<string, string>();
        //    dictionary.Add("uploader", username);

        //    if (busqueda.Count == 0)
        //    {
        //        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        //        {
        //            Name = fileName,
        //            Description = uploadFile.Input.Description + "  This file was uploaded with DriveRepository",
        //            Properties = dictionary
        //        };

        //        FilesResource.CreateMediaUpload request;
        //        using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
        //        {
        //            request = service.Files.Create(
        //            fileMetadata, stream, GetMimeType(fileName));
        //            request.Fields = "id";
        //            await request.UploadAsync();
        //        }
        //        var file = request.ResponseBody;

        //        if (file != null)
        //        {
        //            success = true;
        //        }
        //        return success;
        //    }
        //    else
        //    {
        //        foreach (var item in busqueda)
        //        {
        //            success = await updateFile(item.Fileid, path, uploadFile.Input.Description, GetMimeType(fileName), fileName, dictionary, true);
        //        }
        //        return success;
        //    }
        //}

        public async Task<bool> upload(string path, string fileName)
        {
            bool success = false;
            var service = inizialiceDriveService();

            try
            {
                List<DriveFileModel> file = new List<DriveFileModel>();
                file = await DBExistsOnDrive();

                if (file.Count > 0)
                {
                    var resp = MessageBox.Show("Ya existe una copia de BD creada anteriormente en el servidor de Google Drive, ¿Deseas actualizarla?", "Advertencia", MessageBoxButton.YesNo);

                    if (resp.ToString().Equals("Yes"))
                    {
                        IDictionary<string, string> dictionary = new Dictionary<string, string>();
                        dictionary.Add("uploader", "ItemInventoryApp");
                        success = await updateFile(file[0].Fileid, path, file[0].Description, GetMimeType(file[0].FileName), file[0].FileName, dictionary, true);
                    }
                }
                else //update new file
                {
                    IDictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("uploader", "ItemInventoryApp");
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = fileName,
                        Description = "This file was uploaded from Item inventoryApp as a DB backup on: " + DateTime.Now.ToShortDateString(),
                        Properties = dictionary
                    };

                    FilesResource.CreateMediaUpload request;
                    using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                    {
                        request = service.Files.Create(
                        fileMetadata, stream, GetMimeType(fileName + ".json"));
                        request.Fields = "id";
                        await request.UploadAsync();
                    }
                    var files = request.ResponseBody;

                    if (files != null)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return success;
        }

        private async Task<bool> updateFile(String fileId, String newTitle, String newDescription, String newMimeType,
                                String newFilename, IDictionary<string, string> uploader, bool newRevision)
        {
            try
            {
                var success = false;
                var service = inizialiceDriveService();

                // File's new metadata.
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = newFilename,
                    Description = newDescription + "  This file was uploaded with DriveRepository",
                    Properties = uploader
                };

                FilesResource.UpdateMediaUpload request;
                using (var stream = new System.IO.FileStream(newTitle, System.IO.FileMode.Open))
                {
                    request = service.Files.Update(fileMetadata, fileId, stream, newMimeType);
                    request.Fields = "id";
                    await request.UploadAsync();
                }
                var file = request.ResponseBody;

                if (file != null)
                {
                    success = true;
                }

                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return false;
            }
        }

        public string[] DeleteDriveFile(string fileid)
        {
            var success = false;
            var error = "";
            string[] array = new string[2];

            var service = inizialiceDriveService();

            try
            {
                service.Files.Delete(fileid).Execute();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                error = e.ToString();
            }

            var objeto = new
            {
                isSuccess = success,
                errorDescription = error
            };
            array[0] = success.ToString();
            array[1] = error;

            return array;
        }
        #endregion

        #region Cambio de cuenta DRIVE
        public bool changeDriveAccount()
        {
            bool success = false;
            string credPath = "token.json";
            if (Directory.Exists(credPath))
            {
                Directory.Delete(credPath, true);
                UserCredential credential;
                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
                success = true;
            }
            return success;
        }
        #endregion
    }
}
