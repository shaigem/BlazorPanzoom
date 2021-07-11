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
    /// When placed on an enum type specifies the options for the <see
    /// cref="JsonStringEnumMemberConverter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class JsonStringEnumMemberConverterOptionsAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="JsonStringEnumMemberConverterOptions"/>
        /// generated for the attribute.
        /// </summary>
        public JsonStringEnumMemberConverterOptions? Options { get; }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="JsonStringEnumMemberConverterOptionsAttribute"/> class.
        /// </summary>
        /// <param name="namingPolicyType">
        /// Optional type of a <see cref="JsonNamingPolicy"/> to use for writing
        /// enum values. Type must expose a public parameterless constructor.
        /// </param>
        /// <param name="allowIntegerValues">
        /// True to allow undefined enum values. When true, if an enum value
        /// isn't defined it will output as a number rather than a string.
        /// </param>
        /// <param name="deserializationFailureFallbackValue">
        /// Optional default value to use when a json string does not match
        /// anything defined on the target enum. If not specified a <see
        /// cref="JsonException"/> is thrown for all failures.
        /// </param>
        public JsonStringEnumMemberConverterOptionsAttribute(
            Type? namingPolicyType = null,
            bool allowIntegerValues = true,
            object? deserializationFailureFallbackValue = null)
        {
            Options = new JsonStringEnumMemberConverterOptions
            {
                AllowIntegerValues = allowIntegerValues,
                DeserializationFailureFallbackValue = deserializationFailureFallbackValue
            };

            if (namingPolicyType != null)
            {
                if (!typeof(JsonNamingPolicy).IsAssignableFrom(namingPolicyType))
                    throw new InvalidOperationException(
                        $"Supplied namingPolicyType {namingPolicyType} does not derive from JsonNamingPolicy.");
                if (namingPolicyType.GetConstructor(Type.EmptyTypes) == null)
                    throw new InvalidOperationException(
                        $"Supplied namingPolicyType {namingPolicyType} does not expose a public parameterless constructor.");

                Options.NamingPolicy = (JsonNamingPolicy) Activator.CreateInstance(namingPolicyType);
            }
        }
    }
}