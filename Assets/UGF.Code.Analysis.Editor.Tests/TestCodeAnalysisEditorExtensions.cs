using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace UGF.Code.Analysis.Editor.Tests
{
    public class TestCodeAnalysisEditorExtensions
    {
        [Test]
        public static void SyntaxTreePrint()
        {
            string source = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTarget.txt");
            string sourceTree = File.ReadAllText("Assets/UGF.Code.Analysis.Editor.Tests/TestTargetSyntaxTree.txt");

            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source);

            string result0 = tree.PrintTree();

            Assert.AreEqual(result0, sourceTree);
        }
    }
}
