using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace UGF.Code.Analysis.Editor
{
    internal class CodeAnalysisRewriterAddAttribute : CSharpSyntaxRewriter
    {
        public SyntaxGenerator Generator { get; }
        public bool FirstFound { get; }
        public INamedTypeSymbol AttributeTypeSymbol { get; }
        public bool Applied { get; private set; }

        public CodeAnalysisRewriterAddAttribute(SyntaxGenerator generator, bool firstFound, INamedTypeSymbol attributeTypeSymbol)
        {
            Generator = generator;
            FirstFound = firstFound;
            AttributeTypeSymbol = attributeTypeSymbol;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (!Applied || !FirstFound)
            {
                SyntaxNode attribute = Generator.Attribute(Generator.TypeExpression(AttributeTypeSymbol));

                Applied = true;

                return Generator.AddAttributes(node, attribute);
            }

            return base.VisitClassDeclaration(node);
        }
    }
}
