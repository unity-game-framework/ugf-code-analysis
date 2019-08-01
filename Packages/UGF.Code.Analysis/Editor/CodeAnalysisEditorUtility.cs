using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace UGF.Code.Analysis.Editor
{
    /// <summary>
    /// Provides utilities to work with code analysis in editor.
    /// </summary>
    public static class CodeAnalysisEditorUtility
    {
        /// <summary>
        /// Gets the project compilation with all available references.
        /// </summary>
        public static CSharpCompilation ProjectCompilation { get { return m_projectCompilation ?? (m_projectCompilation = GetProjectCompilation()); } }

        /// <summary>
        /// Gets the default syntax generator.
        /// </summary>
        public static SyntaxGenerator Generator { get { return m_generator ?? (m_generator = GetSyntaxGenerator()); } }

        private static CSharpCompilation m_projectCompilation;
        private static SyntaxGenerator m_generator;

        /// <summary>
        /// Gets new compilation with project references.
        /// </summary>
        /// <returns></returns>
        public static CSharpCompilation GetProjectCompilation()
        {
            CSharpCompilation compilation = CSharpCompilation.Create("Project Compilation");

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    string location = assembly.Location;

                    if (!string.IsNullOrEmpty(location))
                    {
                        compilation = compilation.AddReferences(MetadataReference.CreateFromFile(location));
                    }
                }
            }

            return compilation;
        }

        /// <summary>
        /// Gets new default syntax generator.
        /// </summary>
        /// <returns></returns>
        public static SyntaxGenerator GetSyntaxGenerator()
        {
            return SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
        }

        /// <summary>
        /// Prints full hierarchy of the specified syntax node or token as string representation.
        /// </summary>
        /// <param name="nodeOrToken">The syntax node or token to print.</param>
        /// <param name="depth">The initial indent depth.</param>
        /// <param name="indent">The indent value used for nested nodes.</param>
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
