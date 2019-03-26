using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace UGF.Code.Analysis.Editor
{
    public static class CodeAnalysisEditorUtility
    {
        public static string AddLeadingTrivia(string text, List<string> trivia)
        {
            SyntaxNode root = SyntaxFactory.ParseSyntaxTree(text).GetRoot();
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

        public static TypeDeclarationSyntax AddAttributeToTypeDeclaration(TypeDeclarationSyntax typeDeclaration, Type attributeType)
        {
            AttributeListSyntax attributes = SyntaxFactory.AttributeList();
            string attributeName = !string.IsNullOrEmpty(attributeType.Namespace) ? $"{attributeType.Namespace}.{attributeType.Name}" : attributeType.Name;
            AttributeSyntax attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName(attributeName));

            attributes = attributes.AddAttributes(attribute);
            attributes = attributes.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            typeDeclaration = typeDeclaration.AddAttributeLists(attributes);

            return typeDeclaration;
        }
    }
}