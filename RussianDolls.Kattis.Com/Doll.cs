using Probelms.Kattis.Com;
using Probelms.Kattis.Com.Core;

namespace RussianDolls.Kattis.Com
{
    [Item("Doll", "With Dimensions H, D and W.")]
    public class Doll : Item
    {
        private readonly int _d;
        private readonly int _h;
        private readonly int _w;

        public Doll(int d, int h, int w)
        {
            _d = d;
            _h = h;
            _w = w;
        }


        public int Height
        {
            get { return _h; }
        }
        public int Width
        {
            get { return _w; }
        }

        public int Diameter
        {
            get { return _d; }
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return cloner.Clone(this);
        }

        public override bool Constraint(IItem item)
        {
            if (item is Doll)
                return Constraint((Doll) item);
            return false;
        }

        public bool Constraint(Doll doll)
        {
           return _h - 2 * _w >= doll._h && _d - 2 * _w >= doll._d;
        }

        public override void Print(IPrinter printer)
        {
            printer.WriteLine(ItemName);
        }

        public override string ItemName
        {
            get {return string.Format("{0} {1} {2}", _h, _d, _w); }
        }
    }
}
