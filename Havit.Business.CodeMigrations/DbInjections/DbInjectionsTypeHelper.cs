﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Havit.Business.CodeMigrations.DbInjections
{
    internal static class DbInjectionsTypeHelper
    {
        public static IEnumerable<Type> GetDbInjectors(Assembly assembly) => assembly.GetTypes().Where(t => t.GetInterface(nameof(IDbInjector)) != null);
    }
}