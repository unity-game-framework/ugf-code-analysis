using System;
using Microsoft.CodeAnalysis;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorExtensions
    {
        /// <summary>
        /// Prints full hierarchy of the specified syntax tree as string representation.
        /// </summary>
        /// <param name="tree">The syntax tree to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string PrintTree(this SyntaxTree tree, int depth = 0, int indent = 4)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            return CodeAnalysisEditorFormatUtility.PrintSyntaxNodeOrToken(tree.GetRoot(), depth, indent);
        }
    }
}
