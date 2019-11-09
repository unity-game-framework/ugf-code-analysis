using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace UGF.Code.Analysis.Editor
{
    /// <summary>
    /// Provides utilities to work with formatting.
    /// </summary>
    public static class CodeAnalysisEditorFormatUtility
    {
        /// <summary>
        /// Prints full hierarchy of the specified syntax node or token as string representation.
        /// </summary>
        /// <param name="nodeOrToken">The syntax node or token to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static string PrintSyntaxNodeOrToken(SyntaxNodeOrToken nodeOrToken, int depth = 0, int indent = 4)
        {
            var builder = new StringBuilder();

            PrintSyntaxNodeOrToken(builder, nodeOrToken, depth, indent);

            return builder.ToString();
        }

        /// <summary>
        /// Prints full hierarchy of the specified syntax node or token as string representation.
        /// </summary>
        /// <param name="builder">The builder used to create a string representation.</param>
        /// <param name="nodeOrToken">The syntax node or token to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        public static void PrintSyntaxNodeOrToken(StringBuilder builder, SyntaxNodeOrToken nodeOrToken, int depth = 0, int indent = 4)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (depth < 0) throw new ArgumentOutOfRangeException(nameof(depth));
            if (indent < 0) throw new ArgumentOutOfRangeException(nameof(indent));

            builder.Append(' ', indent * depth);
            builder.Append($"{nodeOrToken.Kind()} {nodeOrToken.Span}");
            builder.AppendLine();

            foreach (SyntaxTrivia trivia in nodeOrToken.GetLeadingTrivia())
            {
                builder.Append(' ', indent * (depth + 1));
                builder.Append($"Lead: {trivia.Kind()} {trivia.Span}");
                builder.AppendLine();
            }

            foreach (SyntaxTrivia trivia in nodeOrToken.GetTrailingTrivia())
            {
                builder.Append(' ', indent * (depth + 1));
                builder.Append($"Trail: {trivia.Kind()} {trivia.Span}");
                builder.AppendLine();
            }

            foreach (SyntaxNodeOrToken childNodeOrToken in nodeOrToken.ChildNodesAndTokens())
            {
                PrintSyntaxNodeOrToken(builder, childNodeOrToken, depth + 1, indent);
            }
        }
    }
}
