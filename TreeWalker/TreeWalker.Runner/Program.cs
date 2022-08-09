// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using TreeWalker.Library;

Console.WriteLine("Start walking");

//var programText = @"
//    interface IFirstInteface { 
//        public void MethodOne();
//    }

//    class FirstClass: IFirstInteface { 
//        public void MethodOne() {
//            Console.WriteLine(1, 2);
//        } 
//    }

//    class SecondClass { 
//        public void AnotherMethod() {
//            Console.WriteLine(3d,4d,5d);
//            Console.WriteLine(6,7typo); // contains a typo
//        } 
//    }";

//var tree = CSharpSyntaxTree.ParseText(programText);
//var root = tree.GetRoot();
//var walker = new ExampleWalker();
//walker.Visit(root);

var projectPath = @"..\..\..\..\TreeWalker.TestProject";
using (var stream = File.OpenRead(Path.Combine(projectPath, "SomeClass.cs")))
{
    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: projectPath);
    var root = tree.GetRoot();

    // TODO: finish this walker to list all the properties of SomeClass
    // Use the Syntax Visualizer to find the SyntaxNodeType of a Class Property.
    // To install the Syntax Visualizer => Visual Studio Installer => Individual Components=> ​[v] DGML Editor
    var firstWalker = new MyFirstStepsWalker();
    firstWalker.Visit(root);
}
