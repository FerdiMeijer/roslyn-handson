// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using TreeWalker.Library;

Console.WriteLine("Start walking");

var projectPath = @"..\..\..\..\TreeWalker.TestProject";
using (var stream = File.OpenRead(Path.Combine(projectPath, "SomeClass.cs")))
{
    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: projectPath);
    var root = tree.GetRoot();

    // TODO 1: finish <MyFirstStepsWalker> to list all the properties of <SomeClass> from the TreeWalker.TestProject
    // Use the Syntax Visualizer to find the SyntaxNodeType of a Class' property.
    // To install the Syntax Visualizer => Visual Studio Installer => Individual Components=> ​[v] DGML Editor

    var firstWalker = new MyFirstStepsWalker();
    firstWalker.Visit(root);

    // TODO 2: finish <MyFirstRewriter> to modifiy exising code. I.e. try to rename the property "ToDoRename" of class <SomeClass>
    // Use the Syntax Visualizer to inspect the Leading and Trailing white spaces.
    var rewriter = new MyFirstRewriter();
    var updated = rewriter.Visit(root);

    Console.WriteLine(updated.ToFullString()); // write the updated syntax tree to console.
}

static void SimpleRun()
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

    var tree = CSharpSyntaxTree.ParseText(programText);
    var root = tree.GetRoot();
    var walker = new ExampleWalker();
    walker.Visit(root);
}