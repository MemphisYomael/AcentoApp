namespace MyStudentsApp.Shared.Services;

public static class ApiEndpoints
{
#if DEBUG
    public const string BaseUrl = "http://localhost:5062";
#else
    public const string BaseUrl = "https://acentoapi.jollygrass-a3eff857.westus2.azurecontainerapps.io";
#endif
}