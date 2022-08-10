namespace TreeWalker.TestProject
{
    public interface ISomeClass
    {
        int Count { get; set; }
        string Name { get; set; }
        void DoSomething();
    }

    public class SomeClass : ISomeClass
    {
        public int Count { get; set; }
        public string Name { get; set; } = "Default";

        private bool ToDoRename { get; set; }

        public void DoSomething()
        {
            Console.WriteLine("Doing something...");
        }
    }
}