using DocumentStorageApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDocumentStorageApi
{
    class Program
    {
        static void Main(string[] args) {
            string tempFolder = "C:\\CodeGames\\TempDocuments";
            DocumentController docController = new DocumentController();
            docController.SelectStorage(DocumentStorage.Dropbox);
            docController.SetStoragePath("/");
            docController.SetFilePath(tempFolder);
            string fileList = docController.GetFiles();
            docController.Download("Get Started with Dropbox.pdf");
            docController.SelectStorage(DocumentStorage.GoogleDrive);
            docController.Upload("Get Started with Dropbox.pdf");
        }
    }
}
