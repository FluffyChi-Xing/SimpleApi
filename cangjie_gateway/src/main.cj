package cangjie_gateway

import net.http.ServerBuilder
import net.http.*
import std.time.*
import std.sync.*
import std.collection.*



func test () {
    let server = ServerBuilder()
                        .addr("0.0.0.0")
                        .port(80)
                        .build()
    server.distributor.register("/index", {httpContext =>
        httpContext.responseBuilder.body("Hello Cangjie!")
    })
    server.serve()
}



public var HOST: String = "127.0.0.1";
public var PORT: UInt16 = 41440;




public func createClient(): String {
    // 创建客户端
    try {
        let client = ClientBuilder().build();
        let response = client.get("http://${HOST}:${PORT}");
        let buffer = Array<Byte>(32, item: 0);
        let length = response.body.read(buffer);
        return String.fromUtf8(buffer[..length]);
    } catch (e: Exception) {
        throw e;
    }
}



class PerformanceTest {
    private static var THREAD_COUNT: Int32 = 1000;
    private static var WARMUP_ROUNDS: Int32 = 100;
    private static var MEASUREMENT_ROUNDS: Int32 = 100;
    private static var ACCESS_COUNT: Int32 = 100000;

    // 基础连接测试
    private static func defaultConnection(): String {
        var result: String = "正在测试基本链接功能......\n";
        try {
            var result: String = createClient();
            result = "成功连接${result}......";
        } catch (error: Exception) {
            result = "连接失败";
        }
        return result;
    }



    private static func defaultConnectionPerformanceSingle(): String {
        var result: String = "正在测试基本链接性能(单线程)......\n";


        var round = 100000;
        var ticks = 0;
        for (r in 0..round) {
            try {
                let r: String = createClient();
                result = "成功连接${r}...."
            } catch (e: Exception) {
                result = "连接失败...";
            }
            ticks += 1;
        }
        result += "平均耗时: ${ticks / round} ticks。 \n";
        return result;
    }



    // 单线程的负载均衡性能测试
    private static func loadBalencePerformanceSingle(): String {
        var result: String = "正在测试负载均衡的单线程性能..... \n";


        var round: Int32 = 1000000;
        var ticks: Int32 = 0;
        var serverCount = HashMap<String, Int32>()
        for (i in 0..round) {
            // 连接服务器
            try {
                var part: String = createClient();
                result = "连接成功.....${part}";
            } catch (e: Exception) {
                result = "连接失败....";
            }
            // TODO: 计算负载均衡时间
        }
        return result;
    }





    private static func rewritePerformanceSingle(): String {
        var result: String = "正在测试路径重写功能性能(单线程)...."
        var round = 1000000;
        var ticks = 0;
        for (i in 0..round) {
            // 连接服务器
            try {
                var part: String = createClient();
                result = "成功....${part}";
            } catch (e: Exception) {
                result = "失败....";
            }

            // TODO: do something
        }
        return result;
    }
    
}






main(): Int8 {
    return 0;
}