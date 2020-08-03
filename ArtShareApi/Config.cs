namespace ArtShareApi{
    /*
        Read from config.json

        Please create a config.json in the root file of the ArtShareApi project.

        And create a json looking like this

        {
            "ConnectionString": "server=YOUR_SERVER;user=YOUR_USER;password=YOUR_PASSWORD;database=YOUR_DATABASE"
        }
    */
    public sealed class Config{
        public string ConnectionString {get;set;}
    }
}