using System;
using Autofac;
using Autofac.Extras.FakeItEasy;
using Fixie;

namespace Common.Tests
{
    public class AutoFakerConvention : Convention
    {
        public AutoFakerConvention()
        {
            Classes
                .NameEndsWith("Tests");

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