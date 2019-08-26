namespace Probelms.Kattis.Com.Core
{
    public interface IProblem<T>
    where T : IItem
    {
       void Solve(IFactory<T> dollFactory);
    }
}