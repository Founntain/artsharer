using System;

namespace ArtShareApi.Models{
    public class CustomerModel{
        public int ID { get; set;}
        public string Username {get; set;}
        public DateTime CreationTime {get; set;}
        public DateTime ExpireDate => CreationTime.AddDays(30);
        public string FurAffinity {get; set;}
        public string Twitter {get; set;}
        public string Discord {get; set;}
    }
}