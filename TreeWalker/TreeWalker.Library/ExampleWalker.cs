using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TreeWalker.Library
{
    public class ExampleWalker : CSharpSyntaxWalker
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var namespaceDeclaration = node.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
            var namespaceExplanation = namespaceDeclaration == null
                ? " without a namespace"
                : $" within the {namespaceDeclaration.Name} namespace";

            Console.WriteLine($"{nameof(VisitClassDeclaration)}: Found class with name '{node.Identifier.Text}' {namespaceExplanation}");
            Console.WriteLine();

            base.VisitClassDeclaration(node);
        }
        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            Console.WriteLine($"{nameof(VisitInterfaceDeclaration)}: Found interface with name {node.Identifier.Text}");
            Console.WriteLine();

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var classDeclaration = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();

            Console.Write($"{nameof(VisitMethodDeclaration)}: Found method with name '{node.Identifier.Text}'");
            if (classDeclaration != null)
            {
                Console.Write($"...that is in class with name '{classDeclaration?.Identifier.Text}'");

            }

            if (classDeclaration?.ContainsDiagnostics == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"...and it contains a problem.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine();

            base.VisitMethodDeclaration(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.ContainsDiagnostics)
            {
                var diagnostics = node.GetDiagnostics()
                    .Select(diagnostic => new { message = diagnostic.GetMessage(), location = diagnostic.Location.ToString() })
                    .ToList();

                Console.ForegroundColor = ConsoleColor.Red;
                diagnostics.ForEach(diagnostic => Console.WriteLine($"   {diagnostic.message} {diagnostic.location}"));
                Console.ForegroundColor = ConsoleColor.White;
            }

            var classDeclaration = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();
            var methodDeclaration = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();

            var argumentCount = node.ArgumentList.Arguments.Count;
            Console.WriteLine($"{nameof(VisitInvocationExpression)}: Found {node} expression with {argumentCount} arguments in method with name '{methodDeclaration?.Identifier.Text}' that is in class with name '{classDeclaration?.Identifier.Text}'");
            
            Console.WriteLine();

            base.VisitInvocationExpression(node);
        }
    }
}