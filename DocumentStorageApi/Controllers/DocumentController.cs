using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Com.CloudRail.SI;
using Com.CloudRail.SI.Interfaces;
using Com.CloudRail.SI.ServiceCode.Commands.CodeRedirect;
using Com.CloudRail.SI.Services;
using Com.CloudRail.SI.Types;
using System.Web;
using System.Web.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentStorageApi.Controllers
{
    public enum DocumentStorage
    {
        Dropbox,
        GoogleDrive,
        OneDrive
    }

    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private ICloudStorage service;
        private String storagePath = "/";
        private string filePath = "";
        private int port = 8082;

        public void SelectStorage(DocumentStorage storage) {
            CloudRail.AppKey = "5ad616fe53d06b4cef4eb35c";

            switch (storage) {
                case DocumentStorage.Dropbox:
                    Dropbox dropbox = new Dropbox(
                            new LocalReceiver(port),
                            "yr0vm4fdyc5m5wq",
                            "ec81w1sde5psdyn",
                            "http://localhost:" + port + "/",
                            "someState"
                            );
                    service = dropbox;
                    break;
                case DocumentStorage.GoogleDrive:
                    GoogleDrive googledrive = new GoogleDrive(
                            new LocalReceiver(port),
                            "452884182377-af4j4heo22mgo04vvg95ol547tih4md7.apps.googleusercontent.com",
                            "1TKbAAkPHtf4DLRdH3os0hL8",
                            "http://localhost:" + port + "/",
                            "someState"
                            );
                    service = googledrive;
                    break;
                case DocumentStorage.OneDrive:
                    OneDrive onedrive = new OneDrive(
                            new LocalReceiver(port),
                            "488bea49-a172-45df-bb6f-a9efb228a4e6",
                            "jD57;nfrylvEJAIWP807[;#",
                            "http://localhost:" + port + "/",
                            "someState"
                            );
                    service = onedrive;
                    break;
            }
        }

        public void SetFilePath(string path) {
            filePath = path;
        }

        public void SetStoragePath(string path) {
            storagePath = path;
        }

        public string GetFiles() {
            string files = "";

            List<CloudMetaData> children = service.GetChildren(storagePath);

            foreach (CloudMetaData c in children) {
                if (string.IsNullOrEmpty(files))
                    files = c.GetName();
                else
                    files = string.Format("{0}, {1}", files, c.GetName());
            }

            return files;
        }

        public void Download(string documentName) {
            try {
                String pathToDocument = storagePath;
                if (!pathToDocument.Equals("/")) {
                    pathToDocument = pathToDocument + "/";
                }

                pathToDocument = pathToDocument + documentName;

                Stream downloadStream = service.Download(pathToDocument);
                string fileName = Path.Combine(filePath, documentName);
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write)) {
                    downloadStream.CopyTo(fileStream);
                }

            }
            catch (Exception e) {
                throw;
            }
        }

        public void Upload(string documentName) {
            try {
                String pathToDocument = storagePath;
                if (!pathToDocument.Equals("/")) {
                    pathToDocument = pathToDocument + "/";
                }

                pathToDocument = pathToDocument + documentName;
                string fileName = Path.Combine(filePath, documentName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                service.Upload(pathToDocument, fs, fs.Length, true);
            }
            catch (Exception e) {
                throw;
            }
        }
    }
}
