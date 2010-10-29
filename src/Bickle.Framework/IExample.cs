namespace Bickle
{
    public interface IExample : IExampleNode
    {
        void Action();
        string FullName { get; }
       
    }
}