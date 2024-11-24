package cangjie_gateway

import net.http.ServerBuilder
import net.http.*
import std.time.*
import std.sync.*
import std.collection.*
import std.random.*



public var HOST: String = "127.0.0.1"
public var PORT: UInt16 = 41440

public func createClient(): String {
    // 创建客户端
    try {
        let client = ClientBuilder().build();
        let response = client.get("http://${HOST}:${PORT}");
        let buffer = Array<Byte>(32, item: 0);
        let length = response.body.read(buffer);
        return String.fromUtf8(buffer[..length]);
    } catch(e: Exception) {
        throw e;
    }
}

public var THREAD_COUNT: Int32 = 1000;
    private var WARMUP_ROUNDS: Int32 = 100;
    private var MEASUREMENT_ROUNDS: Int32 = 100;
    private var ACCESS_COUNT: Int32 = 100000;

    // 基础连接测试
    private func defaultConnection(): String {
        var result = "正在测试基本链接功能......\n";
        try {
            let response = createClient();
            result += "成功连接(${response})......";
        } catch(e: Exception) {
            result = "连接失败";
        }
        return result;
    }

    public func defaultConnectionPerformanceSingle(): String {
        var result = "正在测试基本链接性能(单线程)......\n";
        let round = 100000;
        var ticks = 0;
        for (_ in 0..round) {
            try {
                let response = createClient();
                result += "成功连接(${response})....";
            } catch(e: Exception) {
                result = "连接失败...";
            }
            ticks += 1;
        }
        var tick = ticks / round;
        result += "平均耗时: (${tick}) ticks。 \n";
        return result;
    }

    // 单线程的负载均衡性能测试
//     private static func loadBalencePerformanceSingle(): String {
//     var result = "正在测试负载均衡的单线程性能..... \n"
//     let round: Int32 = 100000
//     var ticks: Int32 = 0
//     var serverCount = HashMap<String, Int32>()
    
//     for (_ in 0..round) {
//         try {
//             let part = createClient();
//             result += "连接成功.....(${part})"
            
//             // Simulate tick count for the connection
//             let tick = Int32(Random().nextFloat16() * Float16(10)) ;
//             ticks += tick;
            
//             // Count server usage
//             if (serverCount[part] = ) {
//                 serverCount[part] = 0
//             }
//             serverCount[part]! += 1 // TODO: 
//         } catch(e: Exception) {
//             result += "连接失败...."
//         }
//     }
    
//     result += "平均耗时: (ticks / round) ticks。\n"
//     result += "各服务器请求占比：\n"
//     for ((k, v) in serverCount(by: { $0.key < $1.key })) {
//         result += "(${server}): \(count * 100 / round)%\n"
//     }
    
//     return result
// }

    public func rewritePerformanceSingle(): String {
        var result = "正在测试路径重写功能性能(单线程)...."
        let round = 1000000
        var ticks = 0
        for (_ in 0..round) {
            // 连接服务器
            try {
                let part = createClient();
                result += "成功....(${part})"
            } catch(e: Exception) {
                result = "失败...."
            }
            // TODO: do something
        }
        return result
    }






main(): Int8 {
    defaultConnection();
    defaultConnectionPerformanceSingle();
    rewritePerformanceSingle();
    return 0;
}