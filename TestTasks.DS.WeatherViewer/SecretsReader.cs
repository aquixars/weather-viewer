using System.Reflection;

namespace TestTasks.DS.WeatherViewer;

public class SecretsReader
{
    public static T ReadSection<T>(string sectionName)
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build()
            .GetSection(sectionName)
            .Get<T>();
    }
}