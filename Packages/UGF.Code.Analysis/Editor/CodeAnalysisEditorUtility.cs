using System;
using System.Reflection;
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
    }
}
