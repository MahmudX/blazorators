﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

global using System.Collections.Concurrent;
global using System.Collections.Immutable;
global using System.Text;
global using System.Text.RegularExpressions;
global using Blazor.SourceGenerators.CSharp;
global using Blazor.SourceGenerators.Diagnostics;
global using Blazor.SourceGenerators.Expressions;
global using Blazor.SourceGenerators.Extensions;
global using Blazor.SourceGenerators.JavaScript;
global using Blazor.SourceGenerators.Parsers;
global using Blazor.SourceGenerators.Readers;
global using Blazor.SourceGenerators.Types;
global using Blazor.SourceGenerators.TypeScript;
global using Blazor.SourceGenerators.TypeScript.Parser;
global using CommentKind = Blazor.SourceGenerators.TypeScript.SyntaxKind;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using Microsoft.CodeAnalysis.Text;
global using static Blazor.SourceGenerators.Expressions.SharedRegex;
global using static Blazor.SourceGenerators.Source.SourceCode;