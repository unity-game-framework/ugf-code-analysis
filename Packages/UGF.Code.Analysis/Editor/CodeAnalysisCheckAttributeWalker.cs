using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UGF.Code.Analysis.Editor
{
    internal class CodeAnalysisCheckAttributeWalker : CSharpSyntaxWalker
    {
        public SemanticModel SemanticModel { get; }
        public Type AttributeType { get; }
        public bool Result { get; private set; }

        public CodeAnalysisCheckAttributeWalker(SemanticModel semanticModel, Type attributeType)
        {
            SemanticModel = semanticModel;
            AttributeType = attributeType;
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            base.VisitAttribute(node);

            ITypeSymbol nodeAttributeTypeSymbol = SemanticModel.GetTypeInfo(node).ConvertedType;
            INamedTypeSymbol attributeTypeSymbol = SemanticModel.Compilation.GetTypeByMetadataName(AttributeType.FullName);

            if (nodeAttributeTypeSymbol.Equals(attributeTypeSymbol))
            {
                Result = true;
            }
        }
    }
}
