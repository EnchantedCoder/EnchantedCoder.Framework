﻿using Havit.Extensions.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Extensions.DependencyInjection.Tests.Infrastructure;

/// <summary>
/// Implementuje jeden interface - IService.
/// </summary>
[Service(Profile = nameof(AttributedGenericService<object, object>), ServiceType = typeof(IGenericService<,>))]
public class AttributedGenericService<TA, TB> : IGenericService<TA, TB>
{
}
