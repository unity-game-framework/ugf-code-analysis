using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace UGF.Code.Analysis.Editor
{
    internal class CodeAnalysisAddAttributeRewriter : CSharpSyntaxRewriter
    {
        public SyntaxGenerator Generator { get; }
        public bool FirstFound { get; }
        public string AttributeName { get; }
        public bool Applied { get; private set; }

        public CodeAnalysisAddAttributeRewriter(SyntaxGenerator generator, bool firstFound, string attributeName)
        {
            Generator = generator;
            FirstFound = firstFound;
            AttributeName = attributeName;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (!Applied || !FirstFound)
            {
                SyntaxNode attribute = Generator.Attribute(AttributeName);

                Applied = true;

                return Generator.AddAttributes(node, attribute);
            }

            return base.VisitClassDeclaration(node);
        }
    }
}
