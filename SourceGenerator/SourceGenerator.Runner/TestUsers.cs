using SourceGenerator.Library;

namespace SourceGenerator.Runner.Test
{
    [TestUser]
    public partial class User1
    {
        public int MyProperty { get; set; }
    }

    [TestUser]
    public partial class User2
    {
        public int MyProperty { get; set; }
    }

    [TestUser]
    public partial class User3a
    {
    
    }

    // uncomment to get custom analysis warning
    //[TestUser]
    //public class User4
    //{

    //}
}
