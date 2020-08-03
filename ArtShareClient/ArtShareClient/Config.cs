namespace ArtShareClient{
    /*
        Read from config.json

        Please create a config.json file in the root/ArtShareClient direcory of the ArtShareClient project (Next to the Config.cs).

        And create a json looking like this

        {
            "ApiUrl": "YOUR API URL"
        }
    */
    public sealed class Config{
        public string ApiUrl {get; set;}
    }
}