using System;
using System.Collections.Generic;
using System.Linq;
using ArtShareApi.Models;
using ArtShareApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;

namespace ArtShareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MainController : ControllerBase
    {
        private ArtShareContext Context {get; set;}

        public MainController()
        {
            this.Context = new ArtShareContext();
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<string> Get()
        {
            return "ArtShare API default endpoint...";
        }

        [HttpPost]
        [Route("addCustomer")]
        public string AddCustomer([FromBody] CustomerModel customer){
            try{
                Context.Customers.Add(new Customer{
                    Username = customer.Username,
                    CreationTime = DateTime.Now,
                    FurAffinity = customer.FurAffinity,
                    Twitter = customer.Twitter,
                    Discord = customer.Discord
                });

                Context.SaveChanges();

                Directory.CreateDirectory($"userdata/{customer.Username}");

                return "User added";
            }catch(Exception ex){
                return ex.ToString();
            }
        }

        [HttpPost]
        [Route("addZipForUser")]
        [DisableRequestSizeLimit]
        public string AddZipForUser([FromBody] UserZipModel userZipModel){
            try{
                if(!Directory.Exists($"userdata/{userZipModel.Username}/"))
                    Directory.CreateDirectory($"userdata/{userZipModel.Username}/");
                else
                    Directory.GetFiles($"userdata/{userZipModel.Username}/").Where(x => !x.EndsWith(".zip")).ToList().ForEach(x => System.IO.File.Delete(x));

                System.IO.File.WriteAllBytes($"userdata/{userZipModel.Username}/{userZipModel.Username}.zip", Convert.FromBase64String(userZipModel.ZipFile));

                ZipFile.ExtractToDirectory($"userdata/{userZipModel.Username}/{userZipModel.Username}.zip", $"userdata/{userZipModel.Username}/");
                
                return "SUCCESS";                
            }catch(Exception ex){
                return ex.ToString();
            }
        }

        
        [HttpGet]
        [Route("unzipZip")]
        public string UnzipZip(string username){
            try{
                if(!Directory.Exists($"userdata/{username}/"))
                    Directory.CreateDirectory($"userdata/{username}/");
                else
                    Directory.GetFiles($"userdata/{username}/").Where(x => !x.EndsWith(".zip")).ToList().ForEach(x => System.IO.File.Delete(x));

                ZipFile.ExtractToDirectory($"userdata/{username}/{username}.zip", $"userdata/{username}/", true);

                return "SUCCESS";                
            }catch(Exception ex){
                return ex.ToString();
            }
        }

        private List<string> DirSearch(string sDir)
        {
            var output = new List<string>();

            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    output.Add(f);
                }

                output.AddRange(DirSearch(d));
            }

            return output;
        }

        [HttpGet]
        [Route("getImagesFromCustomer")]
        public List<string> GetImagesFromCustomer(string username){
            var rootFiles = Directory.GetFiles($"userdata/{username}/").ToList();

            var files = DirSearch($"userdata/{username}/");

            return rootFiles.Concat(files).ToList();
        }

        [HttpGet]
        [Route("getCustomers")]
        public IEnumerable<Customer> GetCustomer(){
            return Context.Customers.ToList();
        }

        [HttpGet]
        [Route("getExpireDateOfUser")]
        public DateTime GetExpireDateOfUser(string username){
            return Context.Customers.FirstOrDefault(x => x.Username == username).CreationTime.AddDays(30);
        }
    }
}
