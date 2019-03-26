using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Scripting;

namespace UGF.Code.Analysis.Editor.Tests
{
    public class TestCodeAnalysisEditorUtility
    {
        private readonly string m_script = @"using System

namespace Test
{
    public class Target
    {
    }   
}
";

        private readonly string m_scriptWithComments = @"// Comment0
// Comment1

using System

namespace Test
{
    public class Target
    {
    }   
}
";

        [Test]
        public void AddLeadingTrivia()
        {
            var comments = new List<string> { "// Comment0", "// Comment1" };
            string script = CodeAnalysisEditorUtility.AddLeadingTrivia(m_script, comments);

            Assert.AreEqual(m_scriptWithComments, script);
        }

        [Test]
        public void AddAttribute()
        {
            var unit = (CompilationUnitSyntax)SyntaxFactory.ParseSyntaxTree(m_script).GetRoot();
            TypeDeclarationSyntax declaration = unit.DescendantNodes().OfType<TypeDeclarationSyntax>().FirstOrDefault();
            
            Assert.NotNull(declaration);

            declaration = CodeAnalysisEditorUtility.AddAttributeToTypeDeclaration(declaration, typeof(PreserveAttribute));
            
            Debug.Log(declaration.ToFullString());
        }
    }
}