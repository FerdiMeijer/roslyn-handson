using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeWalker.Library
{
    public class MyFirstStepsWalker: CSharpSyntaxWalker
    {
        // override a vistor method here to get started
        // hint: use node.DescendantNodes() and node.Ancestors() to traverse the tree up or down to get the objects .OfType<NodeType>()
    }
}
