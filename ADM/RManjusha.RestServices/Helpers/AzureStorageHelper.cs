using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RManjusha.RestServices.Helpers
{
    public class AzureStorageHelper
    {
        //public static async Task<string> UploadFileAsync()
        //{
        //    string storageConnection = CloudConfigurationManager.GetSetting("BlobStorageConnectionString"); CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

        //    //create a block blob CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

        //    //create a container CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("appcontainer");

        //    //create a container if it is not already exists

        //    if (await CloudBlobContainer.CreateIfNotExistsAsync())
        //    {

        //        await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        //    }

        //    string imageName = "Test-" + Path.GetExtension(imageToUpload.FileName);

        //    //get Blob reference

        //    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName); cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;

        //    cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);
        //}
    }
}
