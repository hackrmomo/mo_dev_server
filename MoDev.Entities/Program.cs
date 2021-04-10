using System;

namespace MoDev.Entities
{
    public class Program
    {
        static void Main(string[] args)
        {
            PopulateInitial();
        }

        public static void PopulateInitial()
        {
            using (var context = new MoDevDbContext())
            {
                // Creates the database if not exists
                context.Database.EnsureCreated();

                // Saves changes
                context.SaveChanges();
            }
        }
    }
}
