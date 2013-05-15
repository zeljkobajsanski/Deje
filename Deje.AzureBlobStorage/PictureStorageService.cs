using System;
using System.IO;
using Deje.Core.Services;
using Deje.Core.Utils;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Deje.AzureBlobStorage
{
    public class PictureStorageService : IPictureStorageService
    {
        private CloudStorageAccount m_Account;

        private bool m_Initialized;

        public string SacuvajSlikuArtikla(Stream stream, string contentType, string type)
        {
            var slika = new MemoryStream();
            ImageUtils.ResizeImage(stream, slika, 80);
            var id = Guid.NewGuid().ToString() + type;
            var blob = Artikli().GetBlobReference(id);
            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(slika);
            return blob.Uri.AbsoluteUri;
        }

        public string SacuvajSlikuDobavljaca(Stream fileContent, string contentType, string type)
        {
            var id = Guid.NewGuid().ToString() + type;
            var blob = Dobavljaci().GetBlobReference(id);
            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(fileContent);
            return blob.Uri.AbsoluteUri;
        }

        public byte[] VratiSlikuDobavljaca(string putanja)
        {
            return Dobavljaci().GetBlockBlobReference(putanja).DownloadByteArray();
        }

        public byte[] VratiSlikuArtikla(string putanja)
        {
            return Artikli().GetBlockBlobReference(putanja).DownloadByteArray();
        }

        private CloudBlobContainer Artikli()
        {
            Initialize();
            var blobClient = m_Account.CreateCloudBlobClient();
            var artikli = blobClient.GetContainerReference("artikli");
            return artikli;
        }

        private CloudBlobContainer Dobavljaci()
        {
            Initialize();
            var blobClient = m_Account.CreateCloudBlobClient();
            var dobavljaci = blobClient.GetContainerReference("dobavljaci");
            return dobavljaci;
        }

        private void Initialize()
        {
            if (m_Initialized) return;
            m_Account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));
            var blobClient = m_Account.CreateCloudBlobClient();
            var artikli = blobClient.GetContainerReference("artikli");
            artikli.CreateIfNotExist();
            artikli.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            var dobavljaci = blobClient.GetContainerReference("dobavljaci");
            dobavljaci.CreateIfNotExist();
            dobavljaci.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            m_Initialized = true;
        }
    }
}