﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace TypeScript.TypeConverter.CSharp;

internal record CSharpType(
    string RawName,
    string RawTypeName,
    bool IsNullable = false)
{
    /// <summary>
    /// Gets a string representation of the C# type as a parameter declaration. For example,
    /// <c>"DateTime date"</c> might be returned from a <see cref="CSharpType"/> with
    /// <c>"date"</c> as its <see cref="CSharpType.RawName"/> and <c>"DateTime"</c>
    /// as its <see cref="CSharpType.RawTypeName"/>.
    /// </summary>
    public string ToParameterString() =>
        $"{RawTypeName}{(IsNullable ? "?" : "")} {RawName}";
}