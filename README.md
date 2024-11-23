<div align="center">
<img alt="" src="logo.png" width="20%" />
<h1>ReverseProxyBenchmark</h1>
</div>

<p align="center">
<img alt="" src="https://img.shields.io/badge/release-v0.1.0-brightgreen" style="display: inline-block;" />
<img alt="" src="https://img.shields.io/badge/cjc-v0.53.13-brightgreen" style="display: inline-block;" />
</p>

## <img alt="" src="./doc/readme-image/readme-icon-introduction.png" style="display: inline-block;" width=3%/> 1 介绍

### 1.1 项目特性

本项目是华为ICT学院创新训练营-仓颉编程语言（东北站）实践作业的评测程序。评测涵盖如下评测内容：

* 基本连接功能：最为基本的反向代理功能
* 基本连接性能：单线程十万次访问的平均耗时，单位Ticks。
* 负载均衡性能：单线程十万次负载均衡访问的平均耗时，单位Ticks。
* 路径重写性能：单线程十万次路径重写访问的平均耗时，单位Ticks。
* UA重写性能：单线程十万次UA重写访问的平均耗时，单位Ticks。
* 高并发基本连接性能：一百次一千线程访问的平均耗时，单位Ticks。
* 高并发负载均衡性能：一百次一千线程负载均衡访问的平均耗时，单位Ticks。
* 高并发路径重写性能：一百次一千线程路径重写访问的平均耗时，单位Ticks。
* 高并发UA重写性能：一百次一千线程UA重写访问的平均耗时，单位Ticks。

### 1.2 环境要求

* Docker Compose V2

## <img alt="" src="./doc/readme-image/readme-icon-framework.png" style="display: inline-block;" width=3%/> 2 架构

### 2.1 项目结构

```shell
.
├── BackendServer
│   ├── appsettings.json
│   ├── BackendServer.csproj
│   ├── Controllers
│   │   ├── DefaultController.cs
│   │   └── LoadBalanceController.cs
│   ├── Dockerfile
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   └── Server.cs
├── BenchmarkServer
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── BenchmarkServer.csproj
│   ├── Components
│   │   ├── App.razor
│   │   ├── _Imports.razor
│   │   ├── Layout
│   │   │   ├── MainLayout.razor
│   │   │   ├── MainLayout.razor.css
│   │   │   ├── NavMenu.razor
│   │   │   └── NavMenu.razor.css
│   │   ├── Pages
│   │   │   ├── Home.razor
│   │   │   └── Home.razor.cs
│   │   └── Routes.razor
│   ├── Dockerfile
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   └── wwwroot
│       ├── app.css
│       ├── bootstrap
│       │   ├── bootstrap.min.css
│       │   └── bootstrap.min.css.map
│       └── favicon.png
├── compose.yaml
├── doc
│   └── readme-image
│       ├── readme-icon-compile.png
│       ├── readme-icon-contribute.png
│       ├── readme-icon-framework.png
│       └── readme-icon-introduction.png
├── envoy.yaml
├── README.md
├── ReverseProxyBenchmark.sln
└── Shared
    ├── Response.cs
    └── Shared.csproj
```

## <img alt="" src="./doc/readme-image/readme-icon-compile.png" style="display: inline-block;" width=3%/> 3 使用说明

### 3.1 测试环境

使用如下命令启动测试环境：

```shell
# cd /path/to/ReverseProxyBenchmark
docker compose -f compose.yaml up -d
```

首次启动时间较长，需要保持网络连接顺畅并耐心等待。

启动完成后打开浏览器，访问 http://127.0.0.1:41440 ，点击“开始评测”，对内置的envoy反向代理服务器进行评测。

执行下列命令以关闭容器：

```shell
# cd /path/to/ReverseProxyBenchmark
docker compose -f compose.yaml down
```

### 3.2 评测规范

自行使用仓颉开发反向代理服务器，替换 `compose.yaml` 中
`reverseproxybenchmark-gateway` 服务，不要改变服务名称进行评测。具体流程参见后续小节。

### 3.3 评测内容

#### 3.3.1 基本连接功能

访问 `reverseproxybenchmark-gateway` 的 `/api/default`
路径，请求应该被转发到指定的BackendServer。

在示例envoy中，请求被转发到 `reverseproxybenchmark-backendserver-1` 。

#### 3.3.2 基本连接性能

评测基本连接功能，单线程十万次访问。

#### 3.3.3 负载均衡功能及性能

访问 `reverseproxybenchmark-gateway` 的 `/api/loadbalance`
路径，请求依据负载均衡策略被转发到BackendServer。

在示例envoy中，请求被转发到 `reverseproxybenchmark-backendserver-1` 、
`reverseproxybenchmark-backendserver-2` 、
`reverseproxybenchmark-backendserver-3` ，策略为ROUND_ROBIN。

#### 3.3.4 路径重写功能及性能

访问 `reverseproxybenchmark-gateway` 的 `/api/rewrite`
路径，请求应该被转发到指定的BackendServer的 `/api/default` 路径。

在示例envoy中，请求以负载均衡的形式被转发到
`reverseproxybenchmark-backendserver-1` 、
`reverseproxybenchmark-backendserver-2` 、
`reverseproxybenchmark-backendserver-3`
，策略为ROUND_ROBIN。注意此评测并不要求负载均衡，此处使用负载均衡仅为演示目的。

#### 3.3.5 UA重写功能及性能

访问 `reverseproxybenchmark-gateway` 的 `/api/uarewrite`
路径，请求应该被转发到指定的BackendServer的 `/api/uarewrite` 路径，并将UA修改为
`NEU-Cangjie` 。

在示例envoy中，请求以负载均衡的形式被转发到
`reverseproxybenchmark-backendserver-1` 、
`reverseproxybenchmark-backendserver-2` 、
`reverseproxybenchmark-backendserver-3`
，策略为ROUND_ROBIN。注意此评测并不要求负载均衡，此处使用负载均衡仅为演示目的。

#### 3.3.6 高并发测试

对于上述性能测试，采用一千线程重复一百次。

### 3.4 评分依据

#### 3.4.1 门禁评测

基本连接功能为门禁评测。通过此并测后进行评分。不通过此评测则不进行评分。

#### 3.4.2 分数意义

分数范围：0-5分，分别对应无、差、低于平均水平、平均水平、高于平均水平、很好。其中平均水平以评审日常经验为准，不代表参赛团队平均水平。

#### 3.4.2 评分项目

评测项目：

* 基本连接性能
* 负载均衡功能与性能
* 路径重写功能与性能
* UA重写功能与性能

非评测项目：

* 设计与代码质量
* 日志功能
* 可配置性
* 其他改进与创新

以上满分为40分。

### 3.5 执行评测

1. 参考 [cangjie/README.md](cangjie/README.md) 准备SDK。
2. 参考 [cangjie_gateway/README.md](cangjie_gateway/README.md) 放置项目代码。
3. 注释掉 `compose.yaml` 中使用 `envoy` 镜像的 `reverseproxybenchmark-gateway` 服务。
4. 取消 `compose.yaml` 中使用 `reverseproxybenchmark-gateway` 镜像的 `reverseproxybenchmark-gateway` 服务的注释。
5. 执行下列命令。首次启动时间较长，需要保持网络连接顺畅并耐心等待。

```shell
# cd /path/to/ReverseProxyBenchmark
docker compose -f compose.yaml up -d
```

6. 启动完成后打开浏览器，访问 http://127.0.0.1:41440 ，点击“开始评测”。
7. 执行下列命令以关闭容器。

```shell
# cd /path/to/ReverseProxyBenchmark
docker compose -f compose.yaml down
```

8. 如果需要重新编译项目，则执行下列命令：

```shell
# cd /path/to/ReverseProxyBenchmark
docker compose -f compose.yaml down
docker rmi reverseproxybenchmark-gateway
docker compose -f compose.yaml up -d
```

9. 根据需要可以编辑 `compose.yaml` 的内容，但仅限于使用 `reverseproxybenchmark-gateway` 镜像的 `reverseproxybenchmark-gateway` 服务。

### 3.6 提交代码

1. 将项目代码（即 `./cangjie_gateway/` 路径下的项目，以及修改后的 `compose.yaml` 文件）发布于gitcode。
2. 按照要求提供仓库地址。

## <img alt="" src="./doc/readme-image/readme-icon-contribute.png" style="display: inline-block;" width=3%/> 4 参与贡献

本项目由 [SIGCANGJIE / 仓颉兴趣组](https://gitcode.com/SIGCANGJIE)
实现并维护。技术支持和意见反馈请提Issue。

本项目基于 Apache License 2.0，欢迎给我们提交PR，欢迎参与任何形式的贡献。

本项目committer：[@zhangyin_gitcode](https://gitcode.com/zhangyin_gitcode/)