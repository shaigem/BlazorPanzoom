// Licensed to Macross Software under the MIT license.
// https://github.com/Macross-Software/core/tree/develop/ClassLibraries/Macross.Json.Extensions
//
// Copyright (c) 2020 Macross Software
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// 	of this software and associated documentation files (the "Software"), to deal
// 	in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// 	furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// 	copies or substantial portions of the Software.
//
// 	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// 	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// 	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// 	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// 	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Text.Json;

namespace BlazorPanzoom.Converters
{
    /// <summary>
    /// Stores options for <see cref="JsonStringEnumMemberConverter"/>.
    /// </summary>
    public class JsonStringEnumMemberConverterOptions
    {
        private object? _DeserializationFailureFallbackValue;

        /// <summary>
        /// Gets or sets the optional <see cref="JsonNamingPolicy"/> for writing
        /// enum values.
        /// </summary>
        public JsonNamingPolicy? NamingPolicy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether integer values are allowed
        /// for reading enum values. Default value: <see langword="true"/>.
        /// </summary>
        public bool AllowIntegerValues { get; set; } = true;

        /// <summary>
        /// Gets or sets the optional default value to use when a json string
        /// does not match anything defined on the target enum. If not specified
        /// a <see cref="JsonException"/> is thrown for all failures.
        /// </summary>
        public object? DeserializationFailureFallbackValue
        {
            get => _DeserializationFailureFallbackValue;
            set
            {
                _DeserializationFailureFallbackValue = value;
                ConvertedDeserializationFailureFallbackValue = ConvertEnumValueToUInt64(value);
            }
        }

        internal ulong? ConvertedDeserializationFailureFallbackValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringEnumMemberConverterOptions"/> class.
        /// </summary>
        /// <param name="namingPolicy">
        /// Optional naming policy for writing enum values.
        /// </param>
        /// <param name="allowIntegerValues">
        /// True to allow undefined enum values. When true, if an enum value isn't
        /// defined it will output as a number rather than a string.
        /// </param>
        /// <param name="deserializationFailureFallbackValue">
        /// Optional default value to use when a json string does not match
        /// anything defined on the target enum. If not specified a <see
        /// cref="JsonException"/> is thrown for all failures.
        /// </param>
        public JsonStringEnumMemberConverterOptions(
            JsonNamingPolicy? namingPolicy = null,
            bool allowIntegerValues = true,
            object? deserializationFailureFallbackValue = null)
        {
            NamingPolicy = namingPolicy;
            AllowIntegerValues = allowIntegerValues;
            DeserializationFailureFallbackValue = deserializationFailureFallbackValue;
        }

        private static ulong? ConvertEnumValueToUInt64(object? deserializationFailureFallbackValue)
        {
            if (deserializationFailureFallbackValue == null)
                return null;

            ulong? value = deserializationFailureFallbackValue switch
            {
                int intVal => (ulong) intVal,
                long longVal => (ulong) longVal,
                byte byteVal => byteVal,
                short shortVal => (ulong) shortVal,
                uint uintVal => uintVal,
                ulong ulongVal => ulongVal,
                sbyte sbyteVal => (ulong) sbyteVal,
                ushort ushortVal => ushortVal,
                _ => null,
            };

            return value ?? (deserializationFailureFallbackValue is not Enum enumValue
                ? throw new InvalidOperationException(
                    "Supplied deserializationFailureFallbackValue parameter is not an enum value.")
                : JsonStringEnumMemberConverter.GetEnumValue(Type.GetTypeCode(enumValue.GetType()),
                    deserializationFailureFallbackValue));
        }
    }
}