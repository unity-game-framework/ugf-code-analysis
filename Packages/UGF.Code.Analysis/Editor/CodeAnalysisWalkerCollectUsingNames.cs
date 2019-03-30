using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UGF.Code.Analysis.Editor
{
    internal class CodeAnalysisWalkerCollectUsingNames : CSharpSyntaxWalker
    {
        public HashSet<string> Result { get; } = new HashSet<string>();

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            base.VisitUsingDirective(node);

            Result.Add(node.Name.ToString());
        }
    }
}
