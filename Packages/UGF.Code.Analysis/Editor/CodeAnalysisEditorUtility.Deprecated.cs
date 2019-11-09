using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace UGF.Code.Analysis.Editor
{
    public static partial class CodeAnalysisEditorUtility
    {
        /// <summary>
        /// Prints full hierarchy of the specified syntax node or token as string representation.
        /// </summary>
        /// <param name="nodeOrToken">The syntax node or token to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
        [Obsolete("PrintSyntaxNodeOrToken has been deprecated. Use CodeAnalysisEditorFormatUtility.PrintSyntaxNodeOrToken instead.")]
        public static string PrintSyntaxNodeOrToken(SyntaxNodeOrToken nodeOrToken, int depth = 0, string indent = "    ")
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
        [Obsolete("PrintSyntaxNodeOrToken has been deprecated. Use CodeAnalysisEditorFormatUtility.PrintSyntaxNodeOrToken instead.")]
        public static void PrintSyntaxNodeOrToken(StringBuilder builder, SyntaxNodeOrToken nodeOrToken, int depth = 0, string indent = "    ")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Append(string.Concat(Enumerable.Repeat(indent, depth)));
            builder.Append($"{nodeOrToken.Kind()} {nodeOrToken.Span}");
            builder.AppendLine();

            foreach (SyntaxTrivia trivia in nodeOrToken.GetLeadingTrivia())
            {
                builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
                builder.Append($"Lead: {trivia.Kind()} {trivia.Span}");
                builder.AppendLine();
            }

            foreach (SyntaxTrivia trivia in nodeOrToken.GetTrailingTrivia())
            {
                builder.Append(string.Concat(Enumerable.Repeat(indent, depth + 1)));
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
