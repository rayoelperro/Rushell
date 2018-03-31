namespace Rushell
{
    class Shareable
    {

        public object this[string name]
        {
            get
            {
                return Memory.varv[Memory.varn.IndexOf(name)];
            }
            set
            {
                if (Memory.varn.IndexOf(name) > -1)
                {
                    Memory.varn[Memory.varn.IndexOf(name)] = name;
                    Memory.varv[Memory.varn.IndexOf(name)] = name;
                }
                else
                {
                    Memory.varn.Add(name);
                    Memory.varv.Add(value);
                }
            }
        }
    }
}
