namespace Bickle
{
    public interface IExampleContainer : IExampleNode
    {
        IExampleContainer[] ExampleContainers { get;  }
        IExample[] Examples { get; }
       
    }
}