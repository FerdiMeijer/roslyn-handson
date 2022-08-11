using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TreeWalker.Library
{
    public class ExampleRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (!node.Modifiers.Any(m => m.IsKind(SyntaxKind.PrivateKeyword)))
            {
                return node;
            }

            var publicKeywordToken = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
            var updatedToken = publicKeywordToken.WithTrailingTrivia(SyntaxFactory.Space);
            var updatedNode = node
                .WithModifiers(SyntaxTokenList.Create(updatedToken)); // With...methods create a copy with modifications

            return node.ReplaceNode(node, updatedNode); // replaces this note or one of its children with the updatedNode
        }
    }
}
