using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using hems.HighTail;
using YouSendIt;
using System.Configuration;
using hems.HighTail.Services;
using System;
using Moq;
using hems.HighTail.Models;
//using hems.DataFileDelivery.Services;

namespace hems.DataFileDelivery.Test.Services {
    public class HighTailTest: TestBaseSetup {
        public override void Register(Autofac.ContainerBuilder builder) {
            builder.RegisterModule(new hems.HighTail.Logging.LogInjectionModule());
            hems.HighTail.Logging.LogInjectionModule.SetLevel("Log", "Info");
            builder.RegisterType<ApiHelper>();
            builder.RegisterType<Storage>().As<IStorage>();
            builder.RegisterType<Share>().As<IShare>();
            builder.RegisterType<Files>().As<IFiles>();

            builder.RegisterType<YouSendItAPI>().As<IYouSendItAPI>()
                .WithParameter("apiEndpoint", ConfigurationManager.AppSettings["ApiEndPoint_Sandbox"])
                .WithParameter("apiKey", ConfigurationManager.AppSettings["Api_Key_Sandbox"]);
            builder.Register((c) => {
                return c.Resolve<ApiHelper>();
            }).As<IApiHelper>();
        }
    }

    public class StorageTest : HighTailTest {
        
        [Test, Explicit]
        public void StressTest() {
            var storage = Container.Resolve<IStorage>();
            var share = Container.Resolve<IShare>();
            var files = Container.Resolve<IFiles>();
            string folderName = string.Format("Stress Test Data File : {0}", DateTime.Now);
            var resposne = storage.CreateFolder(folderName, null);
            Assert.AreEqual("0", resposne.Parentid);
            Assert.Greater(resposne.Id, 0);
            Assert.AreEqual(folderName, resposne.Name);

            var uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest1.txt", resposne.Id, true, "1".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 1), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest2.txt", resposne.Id, true, "2".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 2), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest3.txt", resposne.Id, true, "3".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 3), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest4.txt", resposne.Id, true, "4".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 4), uploadedFile.Name);


            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest5.txt", resposne.Id, true, "5".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 5), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest6.txt", resposne.Id, true, "6".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 6), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest7.txt", resposne.Id, true, "7".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 7), uploadedFile.Name);


            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest8.txt", resposne.Id, true, "8".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 8), uploadedFile.Name);

            uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest9.txt", resposne.Id, true, "9".ToString());
            Assert.AreEqual(string.Format("{0}.xlsx", 9), uploadedFile.Name);

        }

        [Test, Explicit]
        public void CreateFolder_ShareFolder_UploadFile_Test() {
            var storage = Container.Resolve<IStorage>();
            var share = Container.Resolve<IShare>();
            var files = Container.Resolve<IFiles>();
            string folderName = string.Format("Test File : {0}",DateTime.Now);
            var resposne = storage.CreateFolder(folderName, null);
            Assert.AreEqual("0", resposne.Parentid);
            Assert.Greater(resposne.Id, 0);
            Assert.AreEqual(folderName, resposne.Name);

            var shareResult = share.ShareFolder(resposne.Id, "niteshdubey.jb@gmail.com", "Hello from the other side", false);
            Assert.AreEqual(shareResult.Status, "OK");

            var uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest10.txt", resposne.Id);
            Assert.AreEqual("FileToTest10.txt", uploadedFile.Name);
            Assert.AreEqual(resposne.Id.ToString(), uploadedFile.ParentId);

            
            var shareFile = share.ShareFile(Convert.ToInt32(uploadedFile.Id), "niteshdubey.jb@gmail.com", "Hello Minions", "Welcome to the other side of the world", 5);
            Assert.AreEqual(shareFile.status, "OK");
        }

        [Test, Explicit]
        public void UploadFile_Convert_To_Excel() {
            var files = Container.Resolve<IFiles>();
            var uploadedFile = files.UploadFile(@"\\RELATIVE_FILE\FileToTest11.txt", 46022898, true, "ConvertedDataFile");
            Assert.AreEqual("ConvertedDataFile.xlsx", uploadedFile.Name);
            Assert.AreEqual("46022898", uploadedFile.ParentId);
        }

        [Test, Explicit]
        public void Upload_BlankFile_Convert_To_Excel() {
            var files = Container.Resolve<IFiles>();
            var uploadedFile = files.UploadFile(@"RELATIVE_FILE\FileToTestBlankFile.txt", 46022898, true, "ConvertedDataFile");
            Assert.AreEqual("ConvertedDataFile.xlsx", uploadedFile.Name);
            Assert.AreEqual("46022898", uploadedFile.ParentId);
        }

        [Test, Explicit]
        public void UploadFile_Without_Converting_To_Excel() {
            var files = Container.Resolve<IFiles>();
            var uploadedFile = files.UploadFile(@"RELATIVE_FILE\FileToTest12.txt", 46022898);
            Assert.AreEqual("Prospect_Profiles_03312014_04032014_882051.txt", uploadedFile.Name);
            Assert.AreEqual("46022898", uploadedFile.ParentId);
        }

        [Test, Explicit]
        public void Share_Folder() {
            var share = Container.Resolve<IShare>();
            var shareInfo = share.ShareFolder(46022898, "nitesh.d83@gmail.com,niteshdubey.jb@gmail.com", "hello sharing folder", false);
            Assert.AreEqual(shareInfo.Status, "OK");
        }


        [Test, Explicit]
        public void Get_Folder_Details() {
            var storage = Container.Resolve<IStorage>();
            var folder = storage.GetFolderDetail(45101964);
            Assert.AreEqual(folder.Id, "45104231");
            Assert.AreEqual(folder.Name, "HelloWorldFolder");
            Console.WriteLine("File Count :{0}", folder.Files.Items.Count());
        }

    }
}
