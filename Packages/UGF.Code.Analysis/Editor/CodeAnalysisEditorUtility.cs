﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

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

        public static List<string> CheckAttributeAllPaths(CSharpCompilation compilation, IEnumerable<string> sourcePaths, Type attributeType)
        {
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));
            if (sourcePaths == null) throw new ArgumentNullException(nameof(sourcePaths));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            var resultPaths = new List<string>();

            INamedTypeSymbol attributeTypeSymbol = compilation.GetTypeByMetadataName(attributeType.FullName);

            if (attributeTypeSymbol == null)
            {
                throw new ArgumentException($"No metadata found for specified attribute type: '{attributeType}'.");
            }

            foreach (string path in sourcePaths)
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

            return resultPaths;
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

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    compilation = compilation.AddReferences(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            return compilation;
        }
    }
}
