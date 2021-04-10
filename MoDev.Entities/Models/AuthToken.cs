using System;
using MySql.EntityFrameworkCore.Extensions;

namespace MoDev.Entities
{
    public class AuthToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}