using MoDev.Entities;
using System.Collections.Generic;

namespace MoDev.Server.Models
{
    public class PhotographsList
    {
        public IEnumerable<Photograph> Photographs { get; set; }
    }
    public class PhotographInsert
    {
        public Photograph Photograph { get; set; }
        public string PhotographImageBase64 { get; set; }
    }
}