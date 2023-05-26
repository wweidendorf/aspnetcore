// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Components.Endpoints.Binding;

public class FormDataDeserializerTests
{
    [Theory]
    [MemberData(nameof(PrimitiveTypesData))]
    public void CanDeserialize_PrimitiveTypes(string value, Type type, object expected)
    {
        // Arrange
        var collection = new Dictionary<string, StringValues>() { ["value"] = new StringValues(value) };
        var reader = new FormDataReader(collection, CultureInfo.InvariantCulture);
        reader.PushPrefix("value");
        var options = new FormDataSerializerOptions();

        // Act
        var result = CallDeserialize(reader, options, type);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(NullableBasicTypes))]
    public void CanDeserialize_NullablePrimitiveTypes(string value, Type type, object expected)
    {
        // Arrange
        var collection = new Dictionary<string, StringValues>() { ["value"] = new StringValues(value) };
        var reader = new FormDataReader(collection, CultureInfo.InvariantCulture);
        reader.PushPrefix("value");
        var options = new FormDataSerializerOptions();

        // Act
        var result = CallDeserialize(reader, options, type);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(NullNullableBasicTypes))]
    public void CanDeserialize_NullValues(Type type)
    {
        // Arrange
        var collection = new Dictionary<string, StringValues>() { };
        var reader = new FormDataReader(collection, CultureInfo.InvariantCulture);
        reader.PushPrefix("value");
        var options = new FormDataSerializerOptions();

        // Act
        var result = CallDeserialize(reader, options, type);

        // Assert
        Assert.Null(result);
    }

    public static TheoryData<string, Type, object> NullableBasicTypes
    {
        get
        {
            var result = new TheoryData<string, Type, object>
            {
                // strings
                { "C", typeof(char?), new char?('C')},
                // bool
                { "true", typeof(bool?), new bool?(true)},
                // bytes
                { "63", typeof(byte?), new byte?((byte)0b_0011_1111)},
                { "-63", typeof(sbyte?), new sbyte?((sbyte)-0b_0011_1111)},
                // numeric types
                { "123", typeof(ushort?), new ushort?((ushort)123u)},
                { "456", typeof(uint?), new uint?(456u)},
                { "789", typeof(ulong?), new ulong?(789uL)},
                { "-101112", typeof(Int128?), new Int128?(-(Int128)101112)},
                { "-123", typeof(short?), new short?((short)-123)},
                { "-456", typeof(int?), new int?(-456)},
                { "-789", typeof(long?), new long?(-789L)},
                { "101112", typeof(UInt128?), new UInt128?((UInt128)101112)},
                // floating point types
                { "12.56", typeof(Half?), new Half?((Half)12.56f)},
                { "6.28", typeof(float?), new float?(6.28f)},
                { "3.14", typeof(double?), new double?(3.14)},
                { "1.23", typeof(decimal?), new decimal?(1.23m)},
                // dates and times
                { "04/20/2023", typeof(DateOnly?), new DateOnly?(new DateOnly(2023, 04, 20))},
                { "4/20/2023 12:56:34", typeof(DateTime?), new DateTime?(new DateTime(2023, 04, 20, 12, 56, 34))},
                { "4/20/2023 12:56:34 PM +02:00", typeof(DateTimeOffset?), new DateTimeOffset?(new DateTimeOffset(2023, 04, 20, 12, 56, 34, TimeSpan.FromHours(2)))},
                { "02:01:03", typeof(TimeSpan?), new TimeSpan?(new TimeSpan(02, 01, 03))},
                { "12:56:34", typeof(TimeOnly?), new TimeOnly?(new TimeOnly(12, 56, 34))},

                // other types
                { "a55eb3df-e984-42b5-85ca-4f68da8567d1", typeof(Guid?), new Guid?(new Guid("a55eb3df-e984-42b5-85ca-4f68da8567d1")) },
            };

            return result;
        }
    }

    public static TheoryData<Type> NullNullableBasicTypes
    {
        get
        {
            var result = new TheoryData<Type>
            {
                // strings
                { typeof(char?) },
                // bool
                { typeof(bool?) },
                // bytes
                { typeof(byte?) },
                { typeof(sbyte?) },
                // numeric types
                { typeof(ushort?) },
                { typeof(uint?) },
                { typeof(ulong?) },
                { typeof(Int128?) },
                { typeof(short?) },
                { typeof(int?) },
                { typeof(long?) },
                { typeof(UInt128?) },
                // floating point types
                { typeof(Half?) },
                { typeof(float?) },
                { typeof(double?) },
                { typeof(decimal?) },
                // dates and times
                { typeof(DateOnly?) },
                { typeof(DateTime?) },
                { typeof(DateTimeOffset?) },
                { typeof(TimeSpan?) },
                { typeof(TimeOnly?) },

                // other types
                { typeof(Guid?) },
            };

            return result;
        }
    }

    public static TheoryData<string, Type, object> PrimitiveTypesData
    {
        get
        {
            var result = new TheoryData<string, Type, object>
            {
                // strings
                { "C", typeof(char), 'C' },
                { "hello", typeof(string), "hello" },
                // bool
                { "true", typeof(bool), true },
                // bytes
                { "63", typeof(byte), (byte)0b_0011_1111 },
                { "-63", typeof(sbyte), (sbyte)-0b_0011_1111 },
                // numeric types
                { "123", typeof(ushort), (ushort)123u },
                { "456", typeof(uint), 456u },
                { "789", typeof(ulong), 789uL },
                { "-101112", typeof(Int128), -(Int128)101112 },
                { "-123", typeof(short), (short)-123 },
                { "-456", typeof(int), -456 },
                { "-789", typeof(long), -789L },
                { "101112", typeof(UInt128), (UInt128)101112 },
                // floating point types
                { "12.56", typeof(Half), (Half)12.56f },
                { "6.28", typeof(float), 6.28f },
                { "3.14", typeof(double), 3.14 },
                { "1.23", typeof(decimal), 1.23m },
                // dates and times
                { "04/20/2023", typeof(DateOnly), new DateOnly(2023, 04, 20) },
                { "4/20/2023 12:56:34", typeof(DateTime), new DateTime(2023, 04, 20, 12, 56, 34) },
                { "4/20/2023 12:56:34 PM +02:00", typeof(DateTimeOffset), new DateTimeOffset(2023, 04, 20, 12, 56, 34, TimeSpan.FromHours(2)) },
                { "02:01:03", typeof(TimeSpan), new TimeSpan(02, 01, 03) },
                { "12:56:34", typeof(TimeOnly), new TimeOnly(12, 56, 34) },

                // other types
                { "a55eb3df-e984-42b5-85ca-4f68da8567d1", typeof(Guid), new Guid("a55eb3df-e984-42b5-85ca-4f68da8567d1") }
            };

            return result;
        }
    }

    private object CallDeserialize(FormDataReader reader, FormDataSerializerOptions options, Type type)
    {
        var method = typeof(FormDataDeserializer)
            .GetMethod("Deserialize", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public) ??
            throw new InvalidOperationException("Unable to find method 'Deserialize'.");

        return method.MakeGenericMethod(type).Invoke(null, new object[] { reader, options })!;
    }
}