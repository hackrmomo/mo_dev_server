using MoDev.Entities;

namespace MoDev.Server.Services 
{
    public class BaseService {
        public MoDevDbContext Context = new MoDevDbContext();
    }
}