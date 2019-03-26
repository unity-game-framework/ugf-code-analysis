using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using UnityEditor;

namespace UGF.Code.Analysis.Editor.Tests
{
    public class TestCodeAnalysisEditorUtility
    {
        [Test]
        public void Test()
        {
            string path = "Assets/UGF.Code.Analysis.Editor.Tests/TestCodeAnalysisEditorUtility.cs";
            string text = AssetDatabase.LoadAssetAtPath<MonoScript>(path).text;
            
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(text);
            
            Assert.NotNull(tree);
        }
    }
}