using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using MoDev.Entities;
using Amazon.S3;
using BC = BCrypt.Net;
namespace MoDev.Server.Services
{
    public interface IPhotographService
    {
        public IEnumerable<Photograph> GetPhotographs();
        public Task<IEnumerable<Photograph>> InsertNewPhotograph(Photograph photograph, string photographImageStr);
        public Task<IEnumerable<Photograph>> DeletePhotograph(int id);
    }
    public class PhotographService : BaseService, IPhotographService
    {

        private string _bucketName = Environment.GetEnvironmentVariable("MODEV_S3_BUCKET");
        private string _spacesEndpoint = Environment.GetEnvironmentVariable("MODEV_S3_ENDPOINT");
        private IAmazonS3 s3 = new AmazonS3Client(new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("MODEV_S3_ENDPOINT")
        });
        public IEnumerable<Photograph> GetPhotographs()
        {
            try
            {
                return Context.Photographs.AsEnumerable();
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e.Message);
                System.Console.ResetColor();
                return null;
            }
        }

        public async Task<IEnumerable<Photograph>> InsertNewPhotograph(Photograph photograph, string photographImageStr)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(photographImageStr.Split(",")[1]);
                var stream = new MemoryStream(bytes);
                var fileName = $"web/photograph/{Guid.NewGuid().ToString()}.png";
                var uri = $"{String.Join($"://{_bucketName}.", _spacesEndpoint.Split("://"))}/{fileName}";
                stream.Position = 0;
                var result = await s3.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                {
                    Key = fileName,
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = _bucketName
                });

                await Context.Photographs.AddAsync(new Photograph
                {
                    ImageUri = uri,
                    Aperture = photograph.Aperture,
                    FocalLength = photograph.FocalLength,
                    Iso = photograph.Iso,
                    Location = photograph.Location,
                    ShutterSpeed = photograph.ShutterSpeed,
                    Time = photograph.Time
                });
                await Context.SaveChangesAsync();

                return GetPhotographs();
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e.Message);
                System.Console.ResetColor();
                return null;
            }
        }

        public async Task<IEnumerable<Photograph>> DeletePhotograph(int id)
        {
            var entityToDelete = Context.Photographs.Where(a => a.Id == id).First();
            if (entityToDelete != null) 
            {
                // entity exists and can be deleted
                Context.Photographs.Remove(entityToDelete);
                var fileName = entityToDelete.ImageUri.Split(_bucketName).Last();
                await s3.DeleteAsync(_bucketName, fileName, null);

                await Context.SaveChangesAsync();
            }
            return GetPhotographs();
        }
    }
}