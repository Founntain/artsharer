using Microsoft.EntityFrameworkCore;
using ArtShareApi.Entities;
using ArtShareApi;
using System.IO;
using Newtonsoft.Json;

namespace ArtShareApi{
    public class ArtShareContext : DbContext{
        public DbSet<Customer> Customers {get; set;}

        //Insert Connectionstring inside UseMySql(string)
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")).ConnectionString);
    }
}