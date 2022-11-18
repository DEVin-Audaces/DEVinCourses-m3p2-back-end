using Microsoft.AspNetCore.Mvc.Testing;

namespace DEVCoursesAPI.Tests;

public class ConfigurationHostApi
{
    private const string url = "https://localhost:7200";
    private protected HttpClient client;

    public ConfigurationHostApi()
    {
        var application = new WebApplicationFactory<Program>();
        application.ClientOptions.BaseAddress = new Uri(url);
        client = application.CreateClient();

    }
}