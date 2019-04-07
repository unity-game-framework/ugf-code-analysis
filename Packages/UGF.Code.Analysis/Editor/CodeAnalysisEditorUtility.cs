using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorUtility
    {
        public static CSharpCompilation ProjectCompilation { get { return m_projectCompilation ?? (m_projectCompilation = GetProjectCompilation()); } }
        public static SyntaxGenerator Generator { get { return m_generator ?? (m_generator = GetSyntaxGenerator()); } }

        private static CSharpCompilation m_projectCompilation;
        private static SyntaxGenerator m_generator;

        public static CSharpCompilation GetProjectCompilation()
        {
            CSharpCompilation compilation = CSharpCompilation.Create("Project Compilation");

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    compilation = compilation.AddReferences(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            return compilation;
        }

        public static SyntaxGenerator GetSyntaxGenerator()
        {
            return SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
        }

        public static string PrintSyntaxNodeOrToken(SyntaxNodeOrToken nodeOrToken, int depth = 0, string indent = "    ")
        {
            var builder = new StringBuilder();

            PrintSyntaxNodeOrToken(builder, nodeOrToken, depth, indent);

            return builder.ToString();
        }

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
