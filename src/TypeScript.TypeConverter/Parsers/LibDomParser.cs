﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using TypeScript.TypeConverter.CSharp;
using TypeScript.TypeConverter.Extensions;

namespace TypeScript.TypeConverter.Parsers;

public class LibDomParser
{
    private static readonly ConcurrentDictionary<string, string> _typeNameToTypeDefinitionMap = new();

    private readonly string _rawUrl = "https://raw.githubusercontent.com/microsoft/TypeScript/main/lib/lib.dom.d.ts";
    private readonly HttpClient _httpClient = new();

    // See: https://regex101.com/r/GV3DiG/1
    private readonly Regex _interfacesRegex = new("(?'declaration'interface.*?{.*?})", RegexOptions.Singleline);
    private readonly Regex _interfaceTypeName = new("(?:interface )(?'TypeName'\\S+)");

    /// <summary>
    /// For testing purposes.
    /// </summary>
#pragma warning disable CA1822 // Mark members as static
    internal bool IsInitialized => _typeNameToTypeDefinitionMap is { Count: > 0 };
#pragma warning restore CA1822 // Mark members as static

    public async ValueTask InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        try
        {
            var libDomDefinitionTypeScript = await _httpClient.GetStringAsync(_rawUrl);
            if (libDomDefinitionTypeScript is { Length: > 0 })
            {
                var matchCollection = _interfacesRegex.Matches(libDomDefinitionTypeScript).Select(m => m.Value);
                Parallel.ForEach(
                    matchCollection,
                    match =>
                    {
                        var typeName = _interfaceTypeName.GetMatchGroupValue(match, "TypeName");
                        if (typeName is not null)
                        {
                            _typeNameToTypeDefinitionMap[typeName] = match;
                        }
                    });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error intializing lib dom parser. {ex}");
        }
    }

    public ParserResult<CSharpExtensionObject> ParseStaticType(string typeName)
    {
        ParserResult<CSharpExtensionObject> result = new(ParserResultStatus.Unknown);

        if (_typeNameToTypeDefinitionMap.TryGetValue(typeName, out var typeScriptDefinitionText))
        {
            try
            {
                result = result with
                {
                    Status = ParserResultStatus.SuccessfullyParsed,
                    Result = typeScriptDefinitionText.ToExtensionObject()
                };
            }
            catch (Exception ex)
            {
                result = result with
                {
                    Status = ParserResultStatus.ErrorParsing,
                    Error = ex.Message
                };
            }
        }
        else
        {
            result = result with
            {
                Status = ParserResultStatus.TargetTypeNotFound
            };
        }

        return result;
    }

    public bool TryParseType(string typeName, out string? csharpSourceText)
    {
        // TODO: See ParseStaticType
        // This needs to become smarter.
        // It needs to account for the fact that a single API could define peripheral assets in both
        // JavaScript and C# files.
        // As such it should probably return a more comprehensive type.
        if (_typeNameToTypeDefinitionMap.TryGetValue(typeName, out var typeScriptDefinitionText))
        {
            csharpSourceText = typeScriptDefinitionText.AsCSharpSourceText();
            return true;
        }

        csharpSourceText = null;
        return false;
    }
}