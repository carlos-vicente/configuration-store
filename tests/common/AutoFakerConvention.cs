using System;
using Autofac;
using Autofac.Extras.FakeItEasy;
using Fixie;

namespace Configuration.Tests
{
    public class AutoFakerConvention : Convention
    {
        public AutoFakerConvention()
        {
            ClassExecution
                .CreateInstancePerCase()
                .UsingFactory(GetInstance);
        }

        private object GetInstance(Type type)
        {
            return new AutoFake().Container.Resolve(type);
        }
    }
}