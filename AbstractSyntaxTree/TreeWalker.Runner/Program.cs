// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using TreeWalker.Library;

Console.WriteLine("Start walking");

ExampleSyntaxTreeRunner.Run();

ExampleSemanticRunner.Run();
ExampleSemanticRunner.RunDataFlowAnalysis();

TodoWalk();

Console.WriteLine("Finished");

//TODO
static void TodoWalk()
{
    var projectPath = @"..\..\..\..\TreeWalker.TestProject";
    using (var stream = File.OpenRead(Path.Combine(projectPath, "SomeClass.cs")))
    {
        var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: projectPath);
        var root = tree.GetRoot();

        // TODO 1: finish <MyFirstStepsWalker> to list all the properties of <SomeClass> from the TreeWalker.TestProject
        // Use the Syntax Visualizer to find the SyntaxNodeType of a Class' property.
        // To install the Syntax Visualizer => Visual Studio Installer => Individual Components=> ​[v] DGML Editor
        // Rider?: https://plugins.jetbrains.com/plugin/16356-syntax-visualizer-for-rider

        var firstWalker = new MyFirstStepsWalker();
        firstWalker.Visit(root);

        // TODO 2: finish <MyFirstRewriter> to modifiy exising code. I.e. try to rename the property "ToDoRename" of class <SomeClass>
        // Use the Syntax Visualizer to inspect the Leading and Trailing white spaces.
        var rewriter = new MyFirstRewriter();
        var updated = rewriter.Visit(root);

        Console.WriteLine(updated.ToFullString()); // write the updated syntax tree to console.
    }
}