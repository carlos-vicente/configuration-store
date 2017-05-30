using Common.Tests;
using Fixie;

namespace Configuration.Store.Persistence.Memory.Tests
{
    public class ConvetionSelector : TestAssembly
    {
        public ConvetionSelector()
        {
            Apply<AutoFakerConvention>();
        }
    }
}