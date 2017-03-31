using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Common.Tests
{
    public class VersionBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;

            if (typeof(Version) != t)
                return new NoSpecimen();

            return new Version(
                context.Create<int>(),
                context.Create<int>(),
                context.Create<int>(),
                context.Create<int>());
        }
    }
}