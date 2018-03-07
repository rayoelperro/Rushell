namespace Rushell
{
    class Shareable
    {

        public object this[string name]
        {
            get
            {
                return Memoria.varv[Memoria.varn.IndexOf(name)];
            }
            set
            {
                if (Memoria.varn.IndexOf(name) > -1)
                {
                    Memoria.varn[Memoria.varn.IndexOf(name)] = name;
                    Memoria.varv[Memoria.varn.IndexOf(name)] = name;
                }
                else
                {
                    Memoria.varn.Add(name);
                    Memoria.varv.Add(value);
                }
            }
        }
    }
}
