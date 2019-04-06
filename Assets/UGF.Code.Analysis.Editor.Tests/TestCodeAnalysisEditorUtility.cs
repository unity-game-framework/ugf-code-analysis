using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using NUnit.Framework;

namespace UGF.Code.Analysis.Editor.Tests
{
    public class TestCodeAnalysisEditorUtility
    {
        [Test]
        public void ProjectCompilation()
        {
            CSharpCompilation result = CodeAnalysisEditorUtility.ProjectCompilation;

            Assert.NotNull(result);
        }

        [Test]
        public void Generator()
        {
            SyntaxGenerator result = CodeAnalysisEditorUtility.Generator;

            Assert.NotNull(result);
        }

        [Test]
        public void GetProjectCompilation()
        {
            CSharpCompilation result = CodeAnalysisEditorUtility.GetProjectCompilation();

            Assert.NotNull(result);
        }

        [Test]
        public void GetSyntaxGenerator()
        {
            SyntaxGenerator result = CodeAnalysisEditorUtility.GetSyntaxGenerator();

            Assert.NotNull(result);
        }

        [Test]
        public void PrintSyntaxTree()
        {
            string source = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTarget.txt");
            string sourceTree = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTargetSyntaxTree.txt");

            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);
            string result = CodeAnalysisEditorUtility.PrintSyntaxTree(tree);

            Assert.AreEqual(sourceTree, result);
        }
    }
}
