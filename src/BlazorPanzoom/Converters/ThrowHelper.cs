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
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace BlazorPanzoom.Converters
{
    internal static class ThrowHelper
    {
        private static readonly PropertyInfo? s_JsonException_AppendPathInformation
            = typeof(JsonException).GetProperty("AppendPathInformation",
                BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Generate a <see cref="JsonException"/> using the internal
        /// <c>JsonException.AppendPathInformation</c> property that will
        /// eventually include the JSON path, line number, and byte position in
        /// line.
        /// <para>
        /// The final message of the exception looks like: The JSON value could
        /// not be converted to {0}. Path: $.{JSONPath} | LineNumber:
        /// {LineNumber} | BytePositionInLine: {BytePositionInLine}.
        /// </para>
        /// </summary>
        /// <param name="propertyType">Property type.</param>
        /// <returns><see cref="JsonException"/>.</returns>
        public static JsonException GenerateJsonException_DeserializeUnableToConvertValue(Type propertyType)
        {
            Debug.Assert(s_JsonException_AppendPathInformation != null);

            JsonException jsonException =
                new JsonException($"The JSON value could not be converted to {propertyType}.");
            s_JsonException_AppendPathInformation?.SetValue(jsonException, true);
            return jsonException;
        }

        /// <summary>
        /// Generate a <see cref="JsonException"/> using the internal
        /// <c>JsonException.AppendPathInformation</c> property that will
        /// eventually include the JSON path, line number, and byte position in
        /// line.
        /// <para>
        /// The final message of the exception looks like: The JSON value '{1}'
        /// could not be converted to {0}. Path: $.{JSONPath} | LineNumber:
        /// {LineNumber} | BytePositionInLine: {BytePositionInLine}.
        /// </para>
        /// </summary>
        /// <param name="propertyType">Property type.</param>
        /// <param name="propertyValue">Value that could not be parsed into
        /// property type.</param>
        /// <param name="innerException">Optional inner <see cref="Exception"/>.</param>
        /// <returns><see cref="JsonException"/>.</returns>
        public static JsonException GenerateJsonException_DeserializeUnableToConvertValue(
            Type propertyType,
            string propertyValue,
            Exception? innerException = null)
        {
            Debug.Assert(s_JsonException_AppendPathInformation != null);

            JsonException jsonException = new JsonException(
                $"The JSON value '{propertyValue}' could not be converted to {propertyType}.",
                innerException);
            s_JsonException_AppendPathInformation?.SetValue(jsonException, true);
            return jsonException;
        }
    }
}