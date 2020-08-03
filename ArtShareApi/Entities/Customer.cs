using System;

namespace ArtShareApi.Entities{
    public class Customer{
        public int ID { get; set;}
        public string Username {get; set;}
        public DateTime CreationTime {get; set;}
        public string FurAffinity {get; set;}
        public string Twitter {get; set;}
        public string Discord {get; set;}
    }
}