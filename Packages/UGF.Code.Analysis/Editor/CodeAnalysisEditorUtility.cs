using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace UGF.Code.Analysis.Editor
{
    /// <summary>
    /// Provides utilities to work with code analysis in editor.
    /// </summary>
    public static partial class CodeAnalysisEditorUtility
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
    }
}
