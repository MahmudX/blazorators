﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators.TypeScript;

internal class CallSignatureDeclaration : Declaration, ISignatureDeclaration, ITypeElement
{
    internal CallSignatureDeclaration() => ((INode)this).Kind = CommentKind.CallSignature;

    NodeArray<TypeParameterDeclaration> ISignatureDeclaration.TypeParameters { get; set; } = default!;
    NodeArray<ParameterDeclaration> ISignatureDeclaration.Parameters { get; set; } = default!;
    ITypeNode ISignatureDeclaration.Type { get; set; } = default!;
    object ITypeElement.TypeElementBrand { get; set; } = default!;
    QuestionToken ITypeElement.QuestionToken { get; set; } = default!;
}
