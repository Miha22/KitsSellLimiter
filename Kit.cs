using System.Collections.Generic;

namespace KitsLimiter
{
    public class Kit
    {
        public string Name;
        public string Category;
        public int Priority;
        public decimal Cost;
        public int CoolDown;
        public Dictionary<ushort, ushort> Items;
        public int Money;
    }

}
