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
    public interface IPortfolioService
    {
        public IEnumerable<PortfolioItem> GetPortfolioItems();
        public Task<IEnumerable<PortfolioItem>> InsertNewPortfolioItem(PortfolioItem portfolioItem, string portfolioImageStr);
        public Task<IEnumerable<PortfolioItem>> DeletePortfolioItem(int id);
        public Task<IEnumerable<PortfolioItem>> EditPortfolioItem(PortfolioItem portfolioItem, string portfolioImageStr);
    }
    public class PortfolioService : BaseService, IPortfolioService
    {

        private string _bucketName = Environment.GetEnvironmentVariable("MODEV_S3_BUCKET");
        private string _spacesEndpoint = Environment.GetEnvironmentVariable("MODEV_S3_ENDPOINT");
        private IAmazonS3 s3 = new AmazonS3Client(new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("MODEV_S3_ENDPOINT")
        });
        public IEnumerable<PortfolioItem> GetPortfolioItems()
        {
            try
            {
                return Context.PortfolioItems.AsEnumerable();
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e.Message);
                System.Console.ResetColor();
                return null;
            }
        }

        public async Task<IEnumerable<PortfolioItem>> InsertNewPortfolioItem(PortfolioItem portfolioItem, string portfolioImageStr)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(portfolioImageStr.Split(",")[1]);
                var stream = new MemoryStream(bytes);
                var fileName = Guid.NewGuid().ToString() + ".png";
                var uri = $"{String.Join($"//{_bucketName}.", _spacesEndpoint.Split("//"))}/{fileName}";
                stream.Position = 0;
                var result = await s3.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                {
                    Key = fileName,
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = _bucketName
                });

                await Context.PortfolioItems.AddAsync(new PortfolioItem
                {
                    ImageUri = uri,
                    Markdown = portfolioItem.Markdown
                });
                await Context.SaveChangesAsync();

                return GetPortfolioItems();
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e.Message);
                System.Console.ResetColor();
                return null;
            }
        }

        public async Task<IEnumerable<PortfolioItem>> DeletePortfolioItem(int id)
        {
            var entityToDelete = Context.PortfolioItems.Where(a => a.Id == id).First();
            if (entityToDelete != null) 
            {
                // entity exists and can be deleted
                Context.PortfolioItems.Remove(entityToDelete);
                var fileName = entityToDelete.ImageUri.Split(_bucketName)[1];
                await s3.DeleteAsync(_bucketName, fileName, null);

                await Context.SaveChangesAsync();
            }
            return GetPortfolioItems();
        }

        public async Task<IEnumerable<PortfolioItem>> EditPortfolioItem(PortfolioItem portfolioItem, string portfolioImageStr)
        {
            // will be implemented later - I have a bunch of other things to work on right now.
            return null;
        }
    }
}