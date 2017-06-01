using Common.Tests;
using Fixie;

namespace Configuration.Store.Web.Tests
{
    public class ConvetionSelector : TestAssembly
    {
        public ConvetionSelector()
        {
            Apply<AutoFakerConvention>();
        }
    }
}