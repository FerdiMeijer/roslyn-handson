using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SourceGenerator.Library
{
    [Generator]
    public class TestUserSourceGenerator : ISourceGenerator
    {
        private const string markerAttributeCode = @"
namespace SourceGenerator.Library
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class TestUserAttribute : System.Attribute
    {
    }
}
";

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (TestUserSyntaxReceiver)context.SyntaxContextReceiver;
            foreach (var receveived in receiver.Received)
            {
                var (@namespace, @class) = receveived;
                var source = GenerateTestUser(@namespace, @class);
                context.AddSource($"TestUser_{@class}.g.cs", source);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Callback runs only once
            context.RegisterForPostInitialization((pi) => pi.AddSource("TestUserSourceGenerator_Attribute__.g.cs", markerAttributeCode));
            // Callback runs on each (debounced) change in source code of any project that is referencing this generator
            context.RegisterForSyntaxNotifications(() => new TestUserSyntaxReceiver()); // will reuse this instance for each visited node
        }

        private string GenerateTestUser(string @namespace, string @class)
        {
            var randomUser = RandomUserFactory.Create();
            var sourceCode = $@"
namespace {@namespace} 
{{

    public partial class {@class}
    {{
        public string Name {{ get; set; }}= @""{randomUser.Name}"";
        
        public string Username {{ get; set; }}= @""{randomUser.Username}"";

        public string Email {{ get; set; }}= @""{randomUser.Email}"";
        
        public string Address {{ get; set; }}= @""{randomUser.Address}"";

        public List<string> Hobbies {{ get; set; }}= new List<string> {{ {randomUser.Hobbies.Select(h => $"\"{h}\"").ToCsv()} }};
        
        public DateTime BirthDay {{ get; set; }}= DateTime.Parse(@""{randomUser.BirthDay}"");

        public override string ToString()
        {{
            return $""User {{Name}} BirthDay {{BirthDay}} Address:{{Address}} Email:{{Email}} Hobbies1:{{Hobbies.Count}}"";
        }}
    }}
}}
";
            return sourceCode;
        }

        public class TestUserSyntaxReceiver : ISyntaxContextReceiver
        {
            public TestUserSyntaxReceiver()
            {
                //Debugger.Launch(); // uncomment to start debugging
            }

            public List<(string Namespace, string ClassName)> Received { get; } = new();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is AttributeSyntax attrib
                    && context.SemanticModel.GetTypeInfo(attrib).Type?.Name == "TestUserAttribute")
                {
                    var @class = context.Node.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                    var @namespace = context.Node.AncestorsAndSelf().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

                    if (@class.IsPartial())
                    {
                        Received.Add((@namespace.Name.ToString(), @class.Identifier.Text));
                    }
                }
            }
        }
    }
}
