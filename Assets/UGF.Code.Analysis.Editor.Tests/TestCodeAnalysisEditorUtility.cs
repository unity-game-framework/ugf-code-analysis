using System.IO;
using System.Text;
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
        public void PrintSyntaxNodeOrToken()
        {
            string source = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTarget.txt");
            string sourceTree = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTargetSyntaxTree.txt");

            SyntaxNode node = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var builder = new StringBuilder();

            CodeAnalysisEditorUtility.PrintSyntaxNodeOrToken(builder, node);

            string result0 = builder.ToString();
            string result1 = CodeAnalysisEditorUtility.PrintSyntaxNodeOrToken(node);

            Assert.AreEqual(result0, sourceTree);
            Assert.AreEqual(result1, sourceTree);
            Assert.AreEqual(result0, result1);
        }
    }
}
