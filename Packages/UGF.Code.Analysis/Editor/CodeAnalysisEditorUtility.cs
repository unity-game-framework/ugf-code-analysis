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

        public static string PrintSyntaxTree(SyntaxTree tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            var builder = new StringBuilder();

            PrintSyntaxNode(builder, tree.GetRoot(), 0, "    ");

            return builder.ToString();
        }

        private static void PrintSyntaxNode(StringBuilder builder, SyntaxNodeOrToken node, int depth, string tab)
        {
            builder.Append(string.Concat(Enumerable.Repeat(tab, depth)));
            builder.Append($"{node.Kind()} {node.Span}");
            builder.AppendLine();

            foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
            {
                builder.Append(string.Concat(Enumerable.Repeat(tab, depth + 1)));
                builder.Append($"Lead: {trivia.Kind()} {trivia.Span}");
                builder.AppendLine();
            }

            foreach (SyntaxTrivia trivia in node.GetTrailingTrivia())
            {
                builder.Append(string.Concat(Enumerable.Repeat(tab, depth + 1)));
                builder.Append($"Trail: {trivia.Kind()} {trivia.Span}");
                builder.AppendLine();
            }

            foreach (SyntaxNodeOrToken nodeOrToken in node.ChildNodesAndTokens())
            {
                PrintSyntaxNode(builder, nodeOrToken, depth + 1, tab);
            }
        }
    }
}
