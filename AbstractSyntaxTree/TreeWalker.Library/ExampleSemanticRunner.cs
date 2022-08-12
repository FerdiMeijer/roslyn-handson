using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TreeWalker.Library
{
    public static class ExampleSemanticRunner
    {
        // See: https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/semantic-analysis
        public static void Run()
        {
            const string programText =
    @"using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            string SystemAssemblyLocation = typeof(string).Assembly.Location; //  System Assembly used for string, Console etc.
            PortableExecutableReference references = MetadataReference.CreateFromFile(SystemAssemblyLocation); // loads assembly into memory, releasing it when "references" is out of scope
            CSharpCompilation compilation = CSharpCompilation.Create("HelloWorld")
                .AddReferences(references)
                .AddSyntaxTrees(tree);

            // Symantic model can answer questions about
            //      What is being referenced? => Symbol (once you have the Symbol you can find out about the Symbols referenced Types, Properties, Methods etc.
            //      It understands the rules of the syntax and can "Safely" tell you if a "Name" is a
            //      -type
            //      -field
            //      -method
            //      -local variable
            //      Knows about elements that comes from a previously compiled assembly
            //      Some objects or types don't come from the source code/syntax
            //      All diagnostics, which are errors and warnings.
            //      How variables flow in and out of regions of source.

            SemanticModel model = compilation.GetSemanticModel(tree);

            // Use the syntax tree to find "using System;"
            UsingDirectiveSyntax usingSystem = root.Usings[0];
            NameSyntax systemName = usingSystem.Name;

            // Use the semantic model for symbol information:
            SymbolInfo nameInfo = model.GetSymbolInfo(systemName);
            var systemSymbol = (INamespaceSymbol)nameInfo.Symbol; // now we can find out about other aspects of the Symbol not part of the source code/text
            foreach (INamespaceSymbol ns in systemSymbol.GetNamespaceMembers())
            {
                Console.WriteLine(ns);
            }

            // Use the syntax model to find the literal string:
            LiteralExpressionSyntax helloWorldString = root.DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .Single();

            // Use the semantic model for type information Remember this is not Reflection!:
            TypeInfo literalInfo = model.GetTypeInfo(helloWorldString);
            INamedTypeSymbol? stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;

            // Get all members that are public and return a string
            var allMembers = stringTypeSymbol
                .GetMembers().OfType<IMethodSymbol>()
                .Where(m => m.ReturnType.Equals(stringTypeSymbol) &&
                        m.DeclaredAccessibility == Accessibility.Public)
                .Select(m => m.Name)
                .Distinct()
                .ToList();

            allMembers.ForEach(m => Console.WriteLine(m));

        }

        // See DataFlowExample from: https://joshvarty.com/2015/02/05/learn-roslyn-now-part-8-data-flow-analysis/
        public static void RunDataFlowAnalysis()
        {
            var tree = CSharpSyntaxTree.ParseText(@"
public class Sample
{
   public void Foo()
   {
        int[] outerArray = new int[10] { 0, 1, 2, 3, 4, 0, 1, 2, 3, 4};
        for (int index = 0; index < 10; index++)
        {
             int[] innerArray = new int[10] { 0, 1, 2, 3, 4, 0, 1, 2, 3, 4 };
             index = index + 2;
             outerArray[index – 1] = 5;
        }
   }
}");

            var Mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { Mscorlib });
            var model = compilation.GetSemanticModel(tree);

            var forStatement = tree.GetRoot().DescendantNodes().OfType<ForStatementSyntax>().Single();
            DataFlowAnalysis result = model.AnalyzeDataFlow(forStatement);
        }
    }
}