using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyProtoBuf;
using ProtoBuf;

namespace Tdf.ProtoBufDemo
{
    class Program
    {
        private static readonly ManualResetEvent AllDone = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            // TestProtoBuf.TestMethod2();
            // TestProtoBuf.TestMethod3();
            // TestProtoBuf.TestMethod4();

            BeginDemo();

            Console.ReadLine();
        }

        private static void BeginDemo()
        {
            // 启动服务端
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9527);
            server.Start();
            server.BeginAcceptTcpClient(ClientConnected, server);
            Console.WriteLine("SERVER : 等待数据 ---");

            // 启动客户端
            ThreadPool.QueueUserWorkItem(RunClient);
            AllDone.WaitOne();

            Console.WriteLine("SERVER : 退出 ---");
            server.Stop();
        }

        // 服务端处理
        private static void ClientConnected(IAsyncResult result)
        {
            try
            {
                var server = (TcpListener)result.AsyncState;
                using (var client = server.EndAcceptTcpClient(result))
                using (var stream = client.GetStream())
                {
                    // 获取
                    Console.WriteLine("SERVER : 客户端已连接，读取数据 ---");

                    /*
                     * 服务端接收对象；
                     * 从代码中可以发现protobuf-net已考虑的非常周到，不论是客户端发送对象还是服务端接收对象，均只需一行代码就可实现：
                     * 
                     * proto-buf 使用 Base128 Varints 编码；
                     */
                    var myRequest = Serializer.DeserializeWithLengthPrefix<MyRequest>(stream, PrefixStyle.Base128);

                    // 使用C# BinaryFormatter
                    IFormatter formatter = new BinaryFormatter();
                    var myData = (MyData)formatter.Deserialize(new MemoryStream(myRequest.data));

                    Console.WriteLine($@"SERVER : 获取成功, myRequest.version={myRequest.version}, myRequest.name={myRequest.name}, myRequest.website={myRequest.website}, myData.resume={myData.resume}");

                    // 响应(MyResponse)
                    var myResponse = new MyResponse
                    {
                        version = myRequest.version,
                        result = 99
                    };
                    Serializer.SerializeWithLengthPrefix(stream, myResponse, PrefixStyle.Base128);
                    Console.WriteLine("SERVER : 响应成功 ---");

                    Console.WriteLine("SERVER: 关闭连接 ---");
                    stream.Close();
                    client.Close();
                }
            }
            finally
            {
                AllDone.Set();
            }
        }

        // 客户端请求
        private static void RunClient(object state)
        {
            try
            {
                // 构造MyData
                var myData = new MyData {resume = "我的个人简介"};

                // 构造MyRequest
                var myRequest = new MyRequest
                {
                    version = 1,
                    name = "Bobby",
                    website = "www.apache.org"
                };

                // 使用C# BinaryFormatter
                using (var ms = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, myData);
                    myRequest.data = ms.GetBuffer();
                    ms.Close();
                }
                Console.WriteLine("CLIENT : 对象构造完毕 ...");

                using (var client = new TcpClient())
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9527));
                    Console.WriteLine("CLIENT : socket 连接成功 ...");

                    using (var stream = client.GetStream())
                    {
                        // 发送，客户端发送对象；
                        Console.WriteLine("CLIENT : 发送数据 ...");
                        ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, myRequest, PrefixStyle.Base128);

                        // 接收
                        Console.WriteLine("CLIENT : 等待响应 ...");
                        var myResponse = ProtoBuf.Serializer.DeserializeWithLengthPrefix<MyResponse>(stream, PrefixStyle.Base128);

                        Console.WriteLine($"CLIENT : 成功获取结果, version={myResponse.version}, result={myResponse.result}");

                        // 关闭
                        stream.Close();
                    }
                    client.Close();
                    Console.WriteLine("CLIENT : 关闭 ...");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($@"CLIENT ERROR : {error.ToString()}");
            }
        }
    }
}
