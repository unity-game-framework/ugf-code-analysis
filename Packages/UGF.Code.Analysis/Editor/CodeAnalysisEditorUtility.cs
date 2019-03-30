using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using UnityEditor.Compilation;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorUtility
    {
        public static string AddLeadingTrivia(string source, IEnumerable<string> trivia)
        {
            CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

            foreach (string text in trivia)
            {
                unit = unit.WithLeadingTrivia(unit.GetLeadingTrivia().Add(SyntaxFactory.Comment(text)));
            }

            return unit.NormalizeWhitespace().ToFullString();
        }

        public static bool CheckAttribute(string source, Type attributeType)
        {
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);
            CSharpCompilation compilation = GetProjectCompilation();

            compilation = compilation.AddSyntaxTrees(tree);

            SemanticModel model = compilation.GetSemanticModel(tree);

            var walker = new CodeAnalysisCheckAttributeWalker(model, attributeType);

            walker.Visit(tree.GetRoot());

            return walker.Result;
        }

        public static string AddAttributeToClassDeclaration(string source, Type attributeType, bool firstFound = true)
        {
            SyntaxGenerator generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            string attributeName = !string.IsNullOrEmpty(attributeType.Namespace) ? $"{attributeType.Namespace}.{attributeType.Name}" : attributeType.Name;
            var rewriter = new CodeAnalysisAddAttributeRewriter(generator, firstFound, attributeName);
            CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

            return rewriter.Visit(unit).NormalizeWhitespace().ToFullString();
        }

        public static string AddUsings(string source, IEnumerable<string> names)
        {
            CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

            foreach (string name in names)
            {
                unit = unit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(name)));
            }

            return unit.NormalizeWhitespace().ToFullString();
        }

        public static HashSet<string> CollectUsingNames(IEnumerable<string> sources)
        {
            var names = new HashSet<string>();

            foreach (string source in sources)
            {
                CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

                foreach (UsingDirectiveSyntax directiveSyntax in unit.Usings)
                {
                    names.Add(directiveSyntax.Name.ToString());
                }
            }

            return names;
        }

        public static CSharpCompilation GetProjectCompilation()
        {
            CSharpCompilation compilation = CSharpCompilation.Create("Project Compilation");

            foreach (string path in CompilationPipeline.GetPrecompiledAssemblyPaths(CompilationPipeline.PrecompiledAssemblySources.All))
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(path));
            }

            return compilation;
        }
    }
}
