using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;


public class BrightDataClient
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "https://api.brightdata.com";
    private readonly int _pollInterval;
    private readonly int _timeout;

    public BrightDataClient(string baseUrl, string apiToken, int pollInterval = 2000, int timeout = 60000)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
        _pollInterval = pollInterval;
        _timeout = timeout;
        _baseUrl = baseUrl;
    }

    public async Task<Object> CreateJobAsync(string zone, string url, string format = "raw")
    {
        var payload = new { zone, url, format };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var resp = await _client.PostAsync($"{_baseUrl}/request", content);
        resp.EnsureSuccessStatusCode();

        if (format == "json")
        {
            return  await resp.Content.ReadFromJsonAsync<JsonElement>();
        }
        else
        {
            return await resp.Content.ReadAsStringAsync();
        }

        //var responseJson = await resp.Content.ReadAsStringAsync();
        //var data = JsonSerializer.Deserialize<JsonElement>(responseJson);

        //return data.GetProperty("id").GetString()!;
    }

    public async Task<JsonElement> GetJobAsync(string jobId)
    {
        var resp = await _client.GetAsync($"{_baseUrl}/request/{jobId}");
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public async Task<JsonElement> WaitForResultAsync(string jobId)
    {
        var start = DateTime.Now;

        while (true)
        {
            var job = await GetJobAsync(jobId);

            var status = job.GetProperty("status").GetString();

            if (status == "done")
                return job;

            if (status == "failed")
                throw new Exception("Bright Data job failed: " + job);

            if ((DateTime.Now - start).TotalMilliseconds > _timeout)
                throw new TimeoutException("Job polling timeout.");

            await Task.Delay(_pollInterval);
        }
    }

    public async Task<Object> RunAsync(string zone, string url, string format = "raw")
    {
        var html = await CreateJobAsync(zone, url, format);
        //return await WaitForResultAsync(jobId);
        return html;
    }
}
