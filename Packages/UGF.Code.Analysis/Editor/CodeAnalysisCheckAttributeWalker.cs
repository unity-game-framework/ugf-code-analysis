using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UGF.Code.Analysis.Editor
{
    internal class CodeAnalysisCheckAttributeWalker : CSharpSyntaxWalker
    {
        public SemanticModel SemanticModel { get; }
        public INamedTypeSymbol AttributeTypeSymbol { get; }
        public bool Result { get; private set; }

        public CodeAnalysisCheckAttributeWalker(SemanticModel semanticModel, INamedTypeSymbol attributeTypeSymbol)
        {
            SemanticModel = semanticModel;
            AttributeTypeSymbol = attributeTypeSymbol;
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            base.VisitAttribute(node);

            ITypeSymbol nodeAttributeTypeSymbol = SemanticModel.GetTypeInfo(node).ConvertedType;

            if (nodeAttributeTypeSymbol.Equals(AttributeTypeSymbol))
            {
                Result = true;
            }
        }
    }
}
