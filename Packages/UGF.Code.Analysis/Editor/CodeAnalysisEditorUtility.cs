using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using UnityEditor.Compilation;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorUtility
    {
        public static CSharpCompilation ProjectCompilation { get { return m_projectCompilation ?? (m_projectCompilation = GetProjectCompilation()); } }

        private static CSharpCompilation m_projectCompilation;

        public static string AddLeadingTrivia(string source, IEnumerable<string> trivia)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (trivia == null) throw new ArgumentNullException(nameof(trivia));

            CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

            foreach (string text in trivia)
            {
                unit = unit.WithLeadingTrivia(unit.GetLeadingTrivia().Add(SyntaxFactory.Comment(text)));
            }

            return unit.NormalizeWhitespace().ToFullString();
        }

        public static void CheckAttributeAllPaths(CSharpCompilation compilation, List<string> sourcePath, Type attributeType, out List<string> resultPaths)
        {
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));
            if (sourcePath == null) throw new ArgumentNullException(nameof(sourcePath));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            resultPaths = new List<string>();

            INamedTypeSymbol attributeTypeSymbol = compilation.GetTypeByMetadataName(attributeType.FullName);

            if (attributeTypeSymbol.Kind == SymbolKind.ErrorType)
            {
                throw new ArgumentException($"No metadata found for specified attribute type: '{attributeType}'.");
            }

            foreach (string path in sourcePath)
            {
                string source = File.ReadAllText(path);
                SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);

                compilation = compilation.AddSyntaxTrees(tree);

                var walker = new CodeAnalysisCheckAttributeWalker(compilation.GetSemanticModel(tree), attributeTypeSymbol);
                
                walker.Visit(tree.GetRoot());

                if (walker.Result)
                {
                    resultPaths.Add(path);
                }
                
                compilation = compilation.RemoveSyntaxTrees(tree);
            }
        }

        public static bool CheckAttribute(CSharpCompilation compilation, string source, Type attributeType)
        {
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);
            INamedTypeSymbol attributeTypeSymbol = compilation.GetTypeByMetadataName(attributeType.FullName);

            compilation = compilation.AddSyntaxTrees(tree);

            var walker = new CodeAnalysisCheckAttributeWalker(compilation.GetSemanticModel(tree), attributeTypeSymbol);

            walker.Visit(tree.GetRoot());

            return walker.Result;
        }

        public static string AddAttributeToClassDeclaration(string source, Type attributeType, bool firstFound = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            SyntaxGenerator generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            string attributeName = !string.IsNullOrEmpty(attributeType.Namespace) ? $"{attributeType.Namespace}.{attributeType.Name}" : attributeType.Name;
            var rewriter = new CodeAnalysisAddAttributeRewriter(generator, firstFound, attributeName);
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);

            return rewriter.Visit(tree.GetRoot()).NormalizeWhitespace().ToFullString();
        }

        public static string AddUsings(string source, IEnumerable<string> names)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (names == null) throw new ArgumentNullException(nameof(names));

            CompilationUnitSyntax unit = SyntaxFactory.ParseCompilationUnit(source);

            foreach (string name in names)
            {
                unit = unit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(name)));
            }

            return unit.NormalizeWhitespace().ToFullString();
        }

        public static HashSet<string> CollectUsingNames(IEnumerable<string> sources)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));

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
