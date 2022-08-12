using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeWalker.Library
{
    public static class ExampleSyntaxTreeRunner
    {
        public static void Run()
        {
            var programText = @"
    interface IFirstInteface {
        public void MethodOne();
    }

    class FirstClass: IFirstInteface {
        public void MethodOne() {
            Console.WriteLine(1, 2);
        }
    }

    class SecondClass {
        public void AnotherMethod() {
            Console.WriteLine(3d,4d,5d);
            Console.WriteLine(6,7typo); // contains a typo
        }
    }";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
            SyntaxNode root = tree.GetRoot();

            var walker = new ExampleWalker();
            walker.Visit(root); // should print to console
        }
    }
}