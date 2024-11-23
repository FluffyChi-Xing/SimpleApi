using System.Text.Json;
using Shared;
using TheSalLab.GeneralReturnValues;

namespace BenchmarkServer.Components.Pages;

public partial class Home {
    private string _result = string.Empty;
    private bool _isDisabled;

    private async Task StartAsync() {
        _isDisabled = true;

        await DefaultConnect();
        StateHasChanged();
        await DefaultConnectPerformanceSingleThreaded();
        StateHasChanged();
        await LoadBalanceConnectPerformanceSingleThreaded();
        StateHasChanged();
        await RewritePerformanceSingleThreaded();
        StateHasChanged();
        await UserAgentPerformanceSingleThreaded();
        StateHasChanged();

        // MultiThreaded
        await DefaultConnectPerformanceMultiThreaded();
        StateHasChanged();
        await LoadBalanceConnectPerformanceMultiThreaded();
        StateHasChanged();
        await RewritePerformanceMultiThreaded();
        StateHasChanged();
        await UserAgentPerformanceMultiThreaded();
        StateHasChanged();

        _isDisabled = false;
    }

    private async Task DefaultConnect() {
        _result += "正在测试基本连接功能……\n";
        StateHasChanged();

        var result = await Connect("api/default");
        if (result.Status != ServiceResultStatus.Success ||
            !CheckBackendServer(result.Result.Item1.ServerName)) {
            _result += "失败。\n";
            _result += $"{result.Message}\n";
            return;
        }

        _result += "成功。\n";
    }

    private async Task DefaultConnectPerformanceSingleThreaded() {
        _result += "\n正在测试基本连接功能性能（单线程）……\n";
        StateHasChanged();

        var round = 100000;
        var ticks = 0l;
        for (var i = 0; i < round; i++) {
            var result = await Connect("api/default");
            if (result.Status != ServiceResultStatus.Success ||
                !CheckBackendServer(result.Result.Item1.ServerName)) {
                _result += "失败。\n";
                _result += $"{result.Message}\n";
                return;
            }

            ticks += result.Result.Item2;
        }

        _result += $"成功。平均耗时：{ticks / round} ticks。\n";
    }

    private async Task LoadBalanceConnectPerformanceSingleThreaded() {
        _result += "\n正在测试负载均衡功能及性能（单线程）……\n";
        StateHasChanged();

        var round = 100000;
        var ticks = 0l;
        var serverCount = new Dictionary<string, int>();
        for (var i = 0; i < round; i++) {
            var result = await Connect("api/loadbalance");
            if (result.Status != ServiceResultStatus.Success ||
                !CheckBackendServer(result.Result.Item1.ServerName)) {
                _result += "失败。\n";
                _result += $"{result.Message}\n";
                return;
            }

            ticks += result.Result.Item2;
            if (!serverCount.ContainsKey(result.Result.Item1.ServerName)) {
                serverCount[result.Result.Item1.ServerName] = 0;
            }

            serverCount[result.Result.Item1.ServerName]++;
        }

        _result += $"成功。平均耗时：{ticks / round} ticks。\n";
        _result += "各服务器请求占比：\n";
        foreach (var kvp in serverCount.OrderBy(p => p.Key)) {
            _result += $"{kvp.Key}: {kvp.Value * 100 / round}%\n";
        }
    }


    private async Task RewritePerformanceSingleThreaded() {
        _result += "\n正在测试路径重写功能性能（单线程）……\n";
        StateHasChanged();

        var round = 100000;
        var ticks = 0l;
        for (var i = 0; i < round; i++) {
            var result = await Connect("api/rewrite");
            if (result.Status != ServiceResultStatus.Success ||
                !CheckBackendServer(result.Result.Item1.ServerName)) {
                _result += "失败。\n";
                _result += $"{result.Message}\n";
                return;
            }

            ticks += result.Result.Item2;
        }

        _result += $"成功。平均耗时：{ticks / round} ticks。\n";
    }

    private async Task UserAgentPerformanceSingleThreaded() {
        _result += "\n正在测试UA重写功能性能（单线程）……\n";
        StateHasChanged();

        var round = 100000;
        var ticks = 0l;
        for (var i = 0; i < round; i++) {
            var result = await Connect("api/uarewrite");
            if (result.Status != ServiceResultStatus.Success ||
                !CheckBackendServer(result.Result.Item1.ServerName) ||
                !CheckUserAgent(result.Result.Item1.UserAgent)) {
                _result += "失败。\n";
                _result += $"{result.Message}\n";
                return;
            }

            ticks += result.Result.Item2;
        }

        _result += $"成功。平均耗时：{ticks / round} ticks。\n";
    }

    private async Task DefaultConnectPerformanceMultiThreaded() {
        _result += "\n正在测试基本连接功能性能（多线程）……\n";
        StateHasChanged();

        var round = 100;
        var numberThreads = 1000;
        var totalTicks = 0l;
        for (var i = 0; i < round; i++) {
            var tasks = Enumerable.Range(0, numberThreads)
                .Select(_ => Connect("api/default")).ToArray();
            var ticks = DateTime.Now.Ticks;
            var results = await Task.WhenAll(tasks);
            var ticksCost = DateTime.Now.Ticks - ticks;

            if (results.Any(p =>
                    p.Status != ServiceResultStatus.Success ||
                    !CheckBackendServer(p.Result.Item1.ServerName))) {
                _result += "失败。";
                _result +=
                    $"{string.Join("\n", results.Where(p => p.Status != ServiceResultStatus.Success).Select(p => p.Message))}\n";
                return;
            }

            totalTicks += ticksCost;
        }

        _result +=
            $"成功。平均每{numberThreads}个请求耗时：{totalTicks / round} ticks。折合每个请求耗时：{totalTicks / (round * numberThreads)} ticks。\n";
    }

    private async Task LoadBalanceConnectPerformanceMultiThreaded() {
        _result += "\n正在测试负载均衡功能及性能（多线程）……\n";
        StateHasChanged();

        var round = 100;
        var numberThreads = 1000;
        var totalTicks = 0l;

        var serverCount = new Dictionary<string, int>();
        for (var i = 0; i < round; i++) {
            var tasks = Enumerable.Range(0, numberThreads)
                .Select(_ => Connect("api/loadbalance")).ToArray();
            var ticks = DateTime.Now.Ticks;
            var results = await Task.WhenAll(tasks);
            var ticksCost = DateTime.Now.Ticks - ticks;

            if (results.Any(p =>
                    p.Status != ServiceResultStatus.Success ||
                    !CheckBackendServer(p.Result.Item1.ServerName))) {
                _result += "失败。";
                _result +=
                    $"{string.Join("\n", results.Where(p => p.Status != ServiceResultStatus.Success).Select(p => p.Message))}\n";
                return;
            }

            totalTicks += ticksCost;

            foreach (var result in results) {
                if (!serverCount.ContainsKey(result.Result.Item1.ServerName)) {
                    serverCount[result.Result.Item1.ServerName] = 0;
                }

                serverCount[result.Result.Item1.ServerName]++;
            }
        }

        _result +=
            $"成功。平均每{numberThreads}个请求耗时：{totalTicks / round} ticks。折合每个请求耗时：{totalTicks / (round * numberThreads)} ticks。\n";
        _result += "各服务器请求占比：\n";
        foreach (var kvp in serverCount.OrderBy(p => p.Key)) {
            _result +=
                $"{kvp.Key}: {kvp.Value * 100 / (round * numberThreads)}%\n";
        }
    }

    private async Task RewritePerformanceMultiThreaded() {
        _result += "\n正在测试路径重写功能性能（多线程）……\n";
        StateHasChanged();

        var round = 100;
        var numberThreads = 1000;
        var totalTicks = 0l;
        for (var i = 0; i < round; i++) {
            var tasks = Enumerable.Range(0, numberThreads)
                .Select(_ => Connect("api/rewrite")).ToArray();
            var ticks = DateTime.Now.Ticks;
            var results = await Task.WhenAll(tasks);
            var ticksCost = DateTime.Now.Ticks - ticks;

            if (results.Any(p =>
                    p.Status != ServiceResultStatus.Success ||
                    !CheckBackendServer(p.Result.Item1.ServerName))) {
                _result += "失败。";
                _result +=
                    $"{string.Join("\n", results.Where(p => p.Status != ServiceResultStatus.Success).Select(p => p.Message))}\n";
                return;
            }

            totalTicks += ticksCost;
        }

        _result +=
            $"成功。平均每{numberThreads}个请求耗时：{totalTicks / round} ticks。折合每个请求耗时：{totalTicks / (round * numberThreads)} ticks。\n";
    }

    private async Task UserAgentPerformanceMultiThreaded() {
        _result += "\n正在测试UA重写功能性能（多线程）……\n";
        StateHasChanged();

        var round = 100;
        var numberThreads = 1000;
        var totalTicks = 0l;
        for (var i = 0; i < round; i++) {
            var tasks = Enumerable.Range(0, numberThreads)
                .Select(_ => Connect("api/uarewrite")).ToArray();
            var ticks = DateTime.Now.Ticks;
            var results = await Task.WhenAll(tasks);
            var ticksCost = DateTime.Now.Ticks - ticks;

            if (results.Any(p =>
                    p.Status != ServiceResultStatus.Success ||
                    !CheckBackendServer(p.Result.Item1.ServerName) ||
                    !CheckUserAgent(p.Result.Item1.UserAgent))) {
                _result += "失败。";
                _result +=
                    $"{string.Join("\n", results.Where(p => p.Status != ServiceResultStatus.Success).Select(p => p.Message))}\n";
                return;
            }

            totalTicks += ticksCost;
        }

        _result +=
            $"成功。平均每{numberThreads}个请求耗时：{totalTicks / round} ticks。折合每个请求耗时：{totalTicks / (round * numberThreads)} ticks。\n";
    }

    private bool CheckBackendServer(string s) =>
        !string.IsNullOrWhiteSpace(s) && s.Contains("backendserver-");

    private bool CheckUserAgent(string s) =>
        !string.IsNullOrWhiteSpace(s) && s.Contains("NEU-Cangjie");

    private async Task<ServiceResult<(Response, long)>> Connect(string path) {
        HttpResponseMessage response;
        var ticks = DateTime.Now.Ticks;
        try {
            response =
                await _httpClient.GetAsync(
                    $"http://reverseproxybenchmark-gateway/{path}");
            response.EnsureSuccessStatusCode();
        } catch (Exception e) {
            return ServiceResult<(Response, long)>.CreateExceptionResult(e,
                "http request failed");
        }

        var ticksCost = DateTime.Now.Ticks - ticks;

        var json = await response.Content.ReadAsStringAsync();
        var deserializeResult = Deserialize(json);
        return deserializeResult.Status == ServiceResultStatus.Success
            ? ServiceResult<(Response, long)>.CreateSuccessResult((
                deserializeResult.Result, ticksCost))
            : deserializeResult.AsResult<(Response, long)>();
    }

    private ServiceResult<Response> Deserialize(string json) {
        if (string.IsNullOrWhiteSpace(json)) {
            return ServiceResult<Response>.CreateInvalidParameterResult(
                "json is null or whitespace");
        }

        Response response;
        try {
            response =
                JsonSerializer.Deserialize<Response>(json, options: _options);
        } catch (Exception e) {
            return ServiceResult<Response>.CreateExceptionResult(e,
                "json deserialize failed");
        }

        if (response is null) {
            return ServiceResult<Response>.CreateFailureResult(
                "json deserialize failed");
        }

        return ServiceResult<Response>.CreateSuccessResult(response);
    }

    private readonly HttpClient _httpClient = new();

    private static readonly JsonSerializerOptions _options =
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
}