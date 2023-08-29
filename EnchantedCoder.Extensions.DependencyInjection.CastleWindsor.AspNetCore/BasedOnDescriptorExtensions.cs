using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;

namespace EnchantedCoder.Extensions.DependencyInjection.CastleWindsor.AspNetCore
{
    /// <summary>
    /// Extension metody k BasedOnDescriptor.
    /// </summary>
    public static class BasedOnDescriptorExtensions
    {
        /// <summary>
        /// Lifestype per webový request pro ASP.NET Core.
        /// </summary>
        public static BasedOnDescriptor LifestylePerAspNetCoreRequest(this BasedOnDescriptor basedOnDescriptor)
        {
            return basedOnDescriptor.Configure(c => c.LifestylePerAspNetCoreRequest());
        }
    }
}
