using DotNetEnv;

public class Config
{
    static Config()
    {
        Env.Load();   // Loads the .env file automatically
    }

    public static string BrightDataToken =>
        Environment.GetEnvironmentVariable("BRIGHTDATA_TOKEN");
    public static string URL =>
        Environment.GetEnvironmentVariable("BRIGHTDATA_BASE");

   
}