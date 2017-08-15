using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.ProtoBufDemo
{
    public static class TestProtoBuf
    {
        public static void TestMethod1()
        {
            var p1 = new Person
            {
                Id = 1,
                Name = "Bobby",
                Address = new Address
                {
                    Line1 = "Line1",
                    Line2 = "Line2"
                }
            };

            var p2 = new Person
            {
                Id = 2,
                Name = "Samba",
                Address = new Address
                {
                    Line1 = "Flat Line1",
                    Line2 = "Flat Line2"
                }
            };

            var pSource = new List<Person>() { p1, p2 };

            var content = ProtobufHelper.Serialize<List<Person>>(pSource);

            Console.Write(content);

            // 写入文件
            File.WriteAllText("D://helloProtoBuf.txt", content);

            Console.WriteLine("\r\n****解析部分*****");

            var pResult = ProtobufHelper.DeSerialize<List<Person>>(content);


            foreach (var p in pResult)
            {
                Console.WriteLine(p.Name);
            }

            Console.Read();
        }

        /*
         * 使用ProtoBuf序列化1000个对象，
         * 查看Person.bin文件大小为：30 KB (29,760 字节)
         */
        public static void TestMethod2()
        {
            var list = new List<Person>();
            for (var i = 0; i < 1000; i++)
            {
                var person = new Person
                {
                    Id = i,
                    Name = "Name" + i,
                    Address = new Address { Line1 = "Line1", Line2 = "Line2" }
                };
                list.Add(person);
            }

            using (var file = System.IO.File.Create("Person.bin"))
            {
                ProtoBuf.Serializer.Serialize(file, list);
            }

        }

        /*
         * 使用xml序列化1000个对象，Person.xml大小为：152 KB (155,935 字节)
         */
        public static void TestMethod3()
        {
            var list = new List<Person>();
            for (var i = 0; i < 1000; i++)
            {
                var person = new Person
                {
                    Id = i,
                    Name = "Name" + i,
                    Address = new Address { Line1 = "Line1", Line2 = "Line2" }
                };
                list.Add(person);
            }

            var xmlSerizlizer = new System.Xml.Serialization.XmlSerializer(typeof(List<Person>));
            using (var file = System.IO.File.Create("Persion.xml"))
            {
                xmlSerizlizer.Serialize(file, list);
            }
        }

        /*
         * 使用binary序列化1000个对象，Person.dat大小为：54.1 KB (55,445 字节)
         */
        public static void TestMethod4()
        {
            var list = new List<Person>();
            for (var i = 0; i < 1000; i++)
            {
                var person = new Person
                {
                    Id = i,
                    Name = "Name" + i,
                    Address = new Address { Line1 = "Line1", Line2 = "Line2" }
                };
                list.Add(person);
            }

            using (var file = new System.IO.FileStream("Person.dat", System.IO.FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(file, list);
            }
        }

        

    }
}


/*
 * 在一些性能要求很高的应用中，使用protocol buffer序列化，优于Json。
 * 而且protocol buffer向后兼容的能力比较好。

 */
