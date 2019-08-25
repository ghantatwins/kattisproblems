using Probelms.Kattis.Com.Core;

namespace RussianDolls.Kattis.Com
{
    public class DollComparer : ItemComparer
    {
        protected override int ActualCompare(IItem item1, IItem item2)
        {
            Doll d1 = (Doll) item1;
            Doll d2 = (Doll) item2;
            if (d1 == null && d2 == null) return 0;
            if (d1 == null) return 1;
            if (d2 == null) return 1;

            return CompareDoll(d1, d2);
        }

        private int CompareDoll(Doll d1, Doll d2)
        {
            if (d1.Height == d2.Height && d1.Diameter == d2.Diameter && d1.Width == d2.Width)
            {
                return 0;
            }
            if (d1.Height - 2 * d1.Width >= d2.Height)
            {
                return -1;
            }
            if (d1.Diameter- 2 * d1.Width >= d2.Diameter)
            {
                return -1;
            }
            return 1;
        }
    }
}