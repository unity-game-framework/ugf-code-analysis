using System;
using Microsoft.CodeAnalysis;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorExtensions
    {
        public static string Print(this SyntaxTree tree, int depth = 0, string indent = "    ")
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            return CodeAnalysisEditorUtility.PrintSyntaxNodeOrToken(tree.GetRoot(), depth, indent);
        }
    }
}
