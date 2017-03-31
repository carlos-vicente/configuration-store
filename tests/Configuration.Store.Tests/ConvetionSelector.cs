using Common.Tests;
using Fixie;

namespace Configuration.Store.Tests
{
    public class ConvetionSelector : TestAssembly
    {
        public ConvetionSelector()
        {
            Apply<AutoFakerConvention>();
        }
    }
}