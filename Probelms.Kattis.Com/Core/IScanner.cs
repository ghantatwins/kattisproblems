namespace Probelms.Kattis.Com.Core
{
    public interface IScanner
    {
        int NextInt();
        long NextLong();
        float NextFloat();
        double NextDouble();
        bool HasNext();
        string NextString();
    }
}