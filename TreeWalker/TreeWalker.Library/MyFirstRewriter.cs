using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TreeWalker.Library
{
    public class MyFirstRewriter: CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            // Try to rename the property of SomeClass

            return base.VisitPropertyDeclaration(node);
        }
    }
}
