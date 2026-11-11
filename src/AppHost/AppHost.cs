var builder = DistributedApplication.CreateBuilder(args);
var webScheme = Environment.GetEnvironmentVariable("APPHOST_WEB_SCHEME") ?? "https";
var isWebHttps = string.Equals(webScheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);

var postgresPassword = builder.AddParameter("postgres-password");

var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume("solution-postgres-data")
    .WithPgAdmin(pgAdmin => pgAdmin.WithUrlForEndpoint("http", url => url.DisplayText = "pgAdmin"))
    .WithEndpoint(port: 5432, targetPort: 5432);

var applicationDatabase = postgres.AddDatabase("app");

var redis = builder.AddRedis("cache")
    .WithRedisCommander(redisCommander =>
        redisCommander.WithUrlForEndpoint("http", url => url.DisplayText = "Redis Commander"))
    .WithEndpoint(port: 6379, targetPort: 6379);

var storage = builder.AddContainer("storage", "bitnamilegacy/minio", "latest")
    .WithEnvironment("MINIO_ROOT_USER", "minioadmin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "minioadmin")
    .WithArgs("server", "/bitnami/minio/data", "--console-address", ":9001")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.TargetPort = 9000;
    });

var seeder = builder.AddProject<Projects.Solution_DataSeeder>("seeder")
    .WithReference(applicationDatabase)
    .WaitFor(postgres);

#pragma warning disable ASPIRECERTIFICATES001 // WithHttpsDeveloperCertificate
var api = builder.AddProject<Projects.Solution_Api>("api", launchProfileName: "https")
    .WithReference(applicationDatabase)
    .WithReference(redis, connectionName: "Cache")
    .WithEnvironment("ConnectionStrings__storage", storage.GetEndpoint("http"))
    .WaitFor(postgres)
    .WaitFor(redis)
    .WaitFor(storage)
    .WaitForCompletion(seeder)
    .WithHttpsDeveloperCertificate()
    .WithExternalHttpEndpoints()
    .WithUrl("/scalar", "Scalar")
    .WithUrls(context =>
    {
        for (var index = context.Urls.Count - 1; index >= 0; index--)
        {
            if (string.Equals(context.Urls[index].Endpoint?.EndpointName, "http", StringComparison.OrdinalIgnoreCase))
            {
                context.Urls.RemoveAt(index);
            }
        }
    });
#pragma warning restore ASPIRECERTIFICATES001

var web = builder.AddViteApp("web", "../../web")
    .WithYarn(installArgs: ["--frozen-lockfile"])
    .WithReference(api)
    .WaitFor(api)
    .WithEndpoint("http", endpoint =>
    {
        endpoint.Port = 5000;
        endpoint.TargetPort = 5000;
        endpoint.UriScheme = webScheme;
        endpoint.IsProxied = false;
    })
    .WithExternalHttpEndpoints()
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
    .WithUrls(context =>
    {
        foreach (var url in context.Urls)
        {
            url.DisplayText = "Frontend";
        }
    });

if (isWebHttps)
{
#pragma warning disable ASPIRECERTIFICATES001 // WithHttpsDeveloperCertificate
    web.WithHttpsDeveloperCertificate();
#pragma warning restore ASPIRECERTIFICATES001
}

builder.Build().Run();
