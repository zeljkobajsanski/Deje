using System;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Deje.Web.Admin.Services
{
    public class BlobStorage
    {
        private CloudStorageAccount m_Account;

        private bool m_Initialized;

        public string SacuvajSlikuArtikla(Stream stream, string contentType, string type)
        {
            var id = Guid.NewGuid().ToString() + type;
            var blob = Artikli().GetBlobReference(id);
            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(stream);
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
            artikli.SetPermissions(new BlobContainerPermissions(){PublicAccess = BlobContainerPublicAccessType.Blob});
            var dobavljaci = blobClient.GetContainerReference("dobavljaci");
            dobavljaci.CreateIfNotExist();
            dobavljaci.SetPermissions(new BlobContainerPermissions(){PublicAccess = BlobContainerPublicAccessType.Blob});
            m_Initialized = true;
        }

        
    }
}