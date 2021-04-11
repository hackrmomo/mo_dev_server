using MoDev.Entities;
using System.Collections.Generic;

namespace MoDev.Server.Models
{
    public class PortfolioItemsList
    {
        public IEnumerable<PortfolioItem> Items { get; set; }
    }

    public class PortfolioItemInsertOrUpdate
    {
        public PortfolioItem Item { get; set; }
        public string PortfolioImageBase64 { get; set; }
    }
}