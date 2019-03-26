using System.Collections.Generic;
using NUnit.Framework;

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
    }
}