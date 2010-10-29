namespace Bickle
{
    public interface ISpec : IExampleNode
    {
        IExampleContainer[] ExampleContainers { get; }
        IExampleNode Find(string id);
    }
}