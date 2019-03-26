using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorUtility
    {
        public static string AddLeadingTrivia(string text, List<string> trivia)
        {
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(text);
            SyntaxNode root = tree.GetRoot();
            SyntaxTriviaList leadingTrivia = root.GetLeadingTrivia();

            for (int i = 0; i < trivia.Count; i++)
            {
                string comment = trivia[i];

                leadingTrivia = leadingTrivia.Add(SyntaxFactory.Comment(comment));
                leadingTrivia = leadingTrivia.Add(SyntaxFactory.CarriageReturnLineFeed);

                if (i == trivia.Count - 1)
                {
                    leadingTrivia = leadingTrivia.Add(SyntaxFactory.CarriageReturnLineFeed);
                }
            }

            root = root.WithLeadingTrivia(leadingTrivia);

            return root.ToFullString();
        }
    }
}