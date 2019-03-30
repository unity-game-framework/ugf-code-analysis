using System;
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

        public class TestTargetAttribute : Attribute
        {
        }

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
        public void CollectUsingNamesFromPaths()
        {
            HashSet<string> results = CodeAnalysisEditorUtility.CollectUsingNamesFromPaths(new[] { m_scriptPath });
            
            Assert.NotNull(results);
            Assert.AreEqual(4, results.Count);
        }
        
        [Test]
        public void CheckAttributeAllPaths()
        {
            List<string> results = CodeAnalysisEditorUtility.CheckAttributeAllPaths(m_compilation, new List<string> { m_scriptPath }, typeof(PreserveAttribute));

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

            script = CodeAnalysisEditorUtility.AddAttributeToClassDeclaration(CodeAnalysisEditorUtility.ProjectCompilation, script, typeof(PreserveAttribute), false);

            Debug.Log(script);
        }

        [Test]
        public void AddUsings()
        {
            HashSet<string> usings = CodeAnalysisEditorUtility.CollectUsingNamesFromPaths(new []{ m_scriptPath });

            string script = CodeAnalysisEditorUtility.AddUsings(m_script, usings);

            Debug.Log(script);
        }

        [Test]
        public void CollectUsingNames()
        {
            HashSet<string> usings = CodeAnalysisEditorUtility.CollectUsingNamesFromPaths(new []{ m_scriptPath });

            Assert.AreEqual(4, usings.Count);
            Assert.True(usings.Contains("System"));
            Assert.True(usings.Contains("System.Collections"));
            Assert.True(usings.Contains("UnityEngine"));
        }
    }
}
