namespace Probelms.Kattis.Com.Core
{
    public interface IProblem<T>
    where T : IItem
    {
       IFactory<T> Factory { get; }
       IPrinter Printer { get; }

        void Solve();
    }
}