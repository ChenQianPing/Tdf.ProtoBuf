using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Tdf.ProtoBufDemo
{
    public class ProtobufHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string content)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        /// <summary>
        /// 将结构转换为字节数组，发送的时候先要把结构转换成字节数组
        /// </summary>
        /// <param name="obj">结构对象</param>
        /// <returns>字节数组</returns>
        public byte[] StructToBytes(object obj)
        {
            // 得到结构体的大小  
            var size = Marshal.SizeOf(obj);
            // 创建byte数组  
            var bytes = new byte[size];
            // 分配结构体大小的内存空间  
            var structPtr = Marshal.AllocHGlobal(size);
            // 将结构体拷到分配好的内存空间  
            Marshal.StructureToPtr(obj, structPtr, false);
            // 从内存空间拷到byte数组  
            Marshal.Copy(structPtr, bytes, 0, size);
            // 释放内存空间  
            Marshal.FreeHGlobal(structPtr);
            // 返回byte数组  
            return bytes;
        }

        /// <summary>
        /// byte数组转结构，接收的时候需要把字节数组转换成结构 
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="type">结构类型</param>
        /// <returns>转换后的结构</returns>
        public object BytesToStruct(byte[] bytes, Type type)
        {
            //得到结构的大小  
            var size = Marshal.SizeOf(type);
            // byte数组长度小于结构的大小  
            if (size > bytes.Length)
            {
                // 返回空  
                return null;
            }
            // 分配结构大小的内存空间  
            var structPtr = Marshal.AllocHGlobal(size);
            // 将byte数组拷到分配好的内存空间  
            Marshal.Copy(bytes, 0, structPtr, size);
            // 将内存空间转换为目标结构  
            var obj = Marshal.PtrToStructure(structPtr, type);
            // 释放内存空间  
            Marshal.FreeHGlobal(structPtr);
            // 返回结构  
            return obj;
        }
    }
}


/*
 * 什么是ProtoBuf-net
 Protobuf是Google开源的一个项目，用户数据序列化反序列化，
 Google声称Google的数据通信都是用该序列化方法。
 它比xml格式要少的多，甚至比二进制数据格式也小的多。

 Protobuf格式协议和xml一样具有平台独立性，可以在不同平台间通信，
 通信所需资源很少，并可以扩展，可以旧的协议上添加新数据。

 Protobuf是在java和c++运行的，Protobuf-net当然就是Protobuf在.net环境下的移植。


 requied是必须有的字段、optional是可有可无的字段、repeated是可以重复的字段（数组或列表），同时枚举字段都必须给出默认值。
 接下来就可以使用ProgoGen来根据proto脚本生成源代码cs文件了，命令行如下：
 protogen -i:ProtoMyData.proto -o:ProtoMyData.cs -ns:MyProtoBuf
 protogen -i:ProtoMyRequest.proto -o:ProtoMyRequest.cs -ns:MyProtoBuf
 protogen -i:ProtoMyResponse.proto -o:ProtoMyResponse.cs -ns:MyProtoBuf

 -i指定了输入，-o指定了输出，-ns指定了生成代码的namespace，上面的proto脚本生成的源码如下：


 */
