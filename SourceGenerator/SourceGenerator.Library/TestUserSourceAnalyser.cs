using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace SourceGenerator.Library
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestUserSourceAnalyser : DiagnosticAnalyzer // Step 1. Inherit from DiagnosticAnalyzer
    {
        public TestUserSourceAnalyser()
        {
            //Debugger.Launch(); // uncomment to start debugging
        }

        /// <summary>
        /// Step 2. Override the SupportedDiagnostics Array
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
           = ImmutableArray.Create(TestUserDiagnosticsDescriptors.TestUserClassMustBePartial);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None); // Don't analyse generated code!
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            if (!(context.Symbol.GetAttributes().Any(a => a.AttributeClass.Name == "TestUserAttribute")))
                return;

            var type = (INamedTypeSymbol)context.Symbol;

            foreach (var declaringSyntaxReference in type.DeclaringSyntaxReferences)
            {
                if (declaringSyntaxReference.GetSyntax()
                    is not ClassDeclarationSyntax classDeclaration ||
                    IsPartial(classDeclaration))
                    continue;

                var error = Diagnostic.Create(TestUserDiagnosticsDescriptors.TestUserClassMustBePartial,
                                              classDeclaration.Identifier.GetLocation(),
                                              type.Name);
                context.ReportDiagnostic(error);
            }
        }

        private static bool IsPartial(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
        }
    }

    public static class TestUserDiagnosticsDescriptors
    {
        public static readonly DiagnosticDescriptor TestUserClassMustBePartial
            = new("TST001",                               // id
                 "TestUser class must be partial",        // title
                 "The class '{0}' must be partial", // message, note the {0} that will be replaced
                 nameof(TestUserSourceAnalyser),          // category
                 DiagnosticSeverity.Error,
                 true);
    }
}
