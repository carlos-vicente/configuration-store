using Common.Tests;
using Fixie;

namespace Configuration.Store.Web.Tests
{
    public class ConventionSelector : TestAssembly
    {
        public ConventionSelector()
        {
            Apply<AutoFakerConvention>();
        }
    }
}