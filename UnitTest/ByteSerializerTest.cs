using Narumikazuchi.Serialization.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;
using Narumikazuchi;
using System.Diagnostics;

namespace UnitTest
{
    [TestClass]
    public class ByteSerializerTest
    {
        [TestMethod]
        public void SerializeOwnedType()
        {
            using MemoryStream stream = new();
            IByteSerializer<InterfaceTest> serializer = CreateByteSerializer
                                                       .ForSerialization()
                                                       .ConfigureForOwnedType<InterfaceTest>()
                                                       .UseDefaultStrategies()
                                                       .Construct();
            serializer.Serialize(stream, new InterfaceTest());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializeUnownedType()
        {
            using MemoryStream stream = new();
            IByteSerializer<DateOnly> serializer = CreateByteSerializer
                                                  .ForSerialization()
                                                  .ConfigureForForeignType<DateOnly>(Methods.DateOnlySetter)
                                                  .UseDefaultStrategies()
                                                  .Construct();
            serializer.Serialize(stream, DateOnly.FromDateTime(DateTime.UtcNow));
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializeViaStrategy()
        {
            TimeOnly original = TimeOnly.FromDateTime(DateTime.UtcNow);

            using MemoryStream stream = new();
            IByteSerializer<TimeOnly> serializer = CreateByteSerializer
                                                  .ForSerialization()
                                                  .ConfigureForForeignType<TimeOnly>(Singleton<TimeOnlyStrategy>.Instance)
                                                  .UseDefaultStrategies()
                                                  .Construct();
            serializer.Serialize(stream, original);
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializeNull()
        {
            using MemoryStream stream = new();
            IByteSerializer<UnmarkedTest> serializer = CreateByteSerializer
                                                      .ForSerialization()
                                                      .ConfigureForForeignType<UnmarkedTest>(UnmarkedTest.GetInfo)
                                                      .UseDefaultStrategies()
                                                      .Construct();
            serializer.Serialize(stream, null);
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializeWithSerializableMember()
        {
            using MemoryStream stream = new();
            IByteSerializer<UnmarkedTest> serializer = CreateByteSerializer
                                                      .ForSerialization()
                                                      .ConfigureForForeignType<UnmarkedTest>(UnmarkedTest.GetInfo)
                                                      .UseDefaultStrategies()
                                                      .Construct();
            serializer.Serialize(stream, new());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void DeserializeOwnedType()
        {
            InterfaceTest original = new(Guid.NewGuid(),
                                         "Foobar",
                                         "Schiggy",
                                         256,
                                         0.7525125187f,
                                         (Half)0.333333f);

            using MemoryStream stream = new();
            IByteSerializerDeserializer<InterfaceTest> serializer = CreateByteSerializer
                                                                   .ForSerializationAndDeserialization()
                                                                   .ConfigureForOwnedType<InterfaceTest>()
                                                                   .UseDefaultStrategies()
                                                                   .Construct();
            serializer.Serialize(stream, original);
            stream.Position = 0;
            InterfaceTest result = serializer.Deserialize(stream);
            Assert.IsTrue(original.Equals(result));
        }

        [TestMethod]
        public void DeserializeUnownedType()
        {
            DateOnly original = DateOnly.FromDateTime(DateTime.UtcNow);

            using MemoryStream stream = new();
            IByteSerializerDeserializer<DateOnly> serializer = CreateByteSerializer
                                                              .ForSerializationAndDeserialization()
                                                              .ConfigureForForeignType<DateOnly>(Methods.DateOnlySetter, Methods.DateOnlyGetter)
                                                              .UseDefaultStrategies()
                                                              .Construct();
            serializer.Serialize(stream, original);
            stream.Position = 0;
            DateOnly result = serializer.Deserialize(stream);
            Assert.IsTrue(original.Equals(result));
        }

        [TestMethod]
        public void DeserializeViaStrategy()
        {
            TimeOnly original = TimeOnly.FromDateTime(DateTime.UtcNow);

            using MemoryStream stream = new();
            IByteSerializerDeserializer<TimeOnly> serializer = CreateByteSerializer
                                                              .ForSerializationAndDeserialization()
                                                              .ConfigureForForeignType<TimeOnly>(Singleton<TimeOnlyStrategy>.Instance)
                                                              .UseDefaultStrategies()
                                                              .Construct();
            serializer.Serialize(stream, original);
            stream.Position = 0;
            TimeOnly result = serializer.Deserialize(stream);
            Assert.IsTrue(original.Equals(result));
        }

        [TestMethod]
        public void DeserializeNull()
        {
            using MemoryStream stream = new();
            IByteSerializerDeserializer<UnmarkedTest> serializer = CreateByteSerializer
                                                                  .ForSerializationAndDeserialization()
                                                                  .ConfigureForForeignType<UnmarkedTest>(UnmarkedTest.GetInfo, UnmarkedTest.FromInfo)
                                                                  .UseDefaultStrategies()
                                                                  .Construct();
            serializer.Serialize(stream, null);
            stream.Position = 0;
            UnmarkedTest? result = serializer.Deserialize(stream);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeserializeWithSerializableMember()
        {
            UnmarkedTest original = new();
            using MemoryStream stream = new();
            IByteSerializerDeserializer<UnmarkedTest> serializer = CreateByteSerializer
                                                                  .ForSerializationAndDeserialization()
                                                                  .ConfigureForForeignType<UnmarkedTest>(UnmarkedTest.GetInfo, UnmarkedTest.FromInfo)
                                                                  .UseDefaultStrategies()
                                                                  .Construct();
            serializer.Serialize(stream, original);
            stream.Position = 0;
            UnmarkedTest? result = serializer.Deserialize(stream);
            Assert.IsTrue(original.Equals(result));
        }

        [TestMethod]
        public void DeserializeEmptyStream()
        {
            using MemoryStream stream = new();
            IByteSerializerDeserializer<UnmarkedTest> serializer = CreateByteSerializer
                                                                  .ForSerializationAndDeserialization()
                                                                  .ConfigureForForeignType<UnmarkedTest>(UnmarkedTest.GetInfo, UnmarkedTest.FromInfo)
                                                                  .UseDefaultStrategies()
                                                                  .Construct();
            UnmarkedTest? result = serializer.Deserialize(stream);
            Assert.IsNull(result);
        }

        public TestContext TestContext
        {
            get => this._instance;
            set => this._instance = value;
        }

        private TestContext _instance;
    }
}
