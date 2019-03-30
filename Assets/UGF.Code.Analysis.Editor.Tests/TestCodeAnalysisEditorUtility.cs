using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace UGF.Code.Analysis.Editor.Tests
{
    public class TestCodeAnalysisEditorUtility
    {
        private readonly string m_script = @"using System;

namespace Test
{
    public class Target
    {
    }   
}";

        private readonly string m_scriptWithComments = @"// Comment0
// Comment1
using System;

namespace Test
{
    public class Target
    {
    }
}";

        private readonly string m_scriptPath = "Assets/UGF.Code.Analysis.Editor.Tests/TestTarget.cs";

        private readonly List<string> m_sources = new List<string>
        {
            "using System;",
            "using System;\nusing System.Collections;",
            "using UnityEngine;"
        };

        private CSharpCompilation m_compilation;
        
        [SetUp]
        public void Setup()
        {
            m_compilation = CodeAnalysisEditorUtility.ProjectCompilation;
        }
        
        [Test]
        public void AddLeadingTrivia()
        {
            var comments = new List<string> { "// Comment0", "// Comment1" };
            string script = CodeAnalysisEditorUtility.AddLeadingTrivia(m_script, comments);

            Assert.AreEqual(m_scriptWithComments, script);
        }

        [Test]
        public void CheckAttributeAllPaths()
        {
            CodeAnalysisEditorUtility.CheckAttributeAllPaths(m_compilation, new List<string> { m_scriptPath }, typeof(PreserveAttribute), out List<string> results);
            
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.Contains(m_scriptPath, results);
        }
        
        [Test]
        public void CheckAttribute()
        {
            string script = AssetDatabase.LoadAssetAtPath<MonoScript>(m_scriptPath).text;

            bool result = CodeAnalysisEditorUtility.CheckAttribute(m_compilation, script, typeof(PreserveAttribute));
            
            Assert.True(result);
        }
        
        [Test]
        public void AddAttributeToClassDeclaration()
        {
            string script = AssetDatabase.LoadAssetAtPath<MonoScript>(m_scriptPath).text;

            script = CodeAnalysisEditorUtility.AddAttributeToClassDeclaration(script, typeof(PreserveAttribute), false);

            Debug.Log(script);
        }

        [Test]
        public void AddUsings()
        {
            HashSet<string> usings = CodeAnalysisEditorUtility.CollectUsingNames(m_sources);

            string script = CodeAnalysisEditorUtility.AddUsings(m_script, usings);
            
            Debug.Log(script);
        }

        [Test]
        public void CollectUsingNames()
        {
            HashSet<string> usings = CodeAnalysisEditorUtility.CollectUsingNames(m_sources);

            Assert.AreEqual(3, usings.Count);
            Assert.True(usings.Contains("System"));
            Assert.True(usings.Contains("System.Collections"));
            Assert.True(usings.Contains("UnityEngine"));
        }
    }
}
