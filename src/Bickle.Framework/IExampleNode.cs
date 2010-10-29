namespace Bickle
{
    public interface IExampleNode
    {
        string Id { get; }
        void Execute(ITestResultListener listener);
        string Name { get; }
        bool IsIgnored();
        ISpec ContainingSpec { get; }
    }
}