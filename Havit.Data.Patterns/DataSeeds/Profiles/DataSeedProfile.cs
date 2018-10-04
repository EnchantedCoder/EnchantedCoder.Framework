using System;
using System.Collections.Generic;
using System.Linq;

namespace Havit.Data.Patterns.DataSeeds.Profiles
{
    /// <summary>
    /// P�edek pro implementaci profil� seedovan�ch dat.
    /// </summary>
    public abstract class DataSeedProfile : IDataSeedProfile
    {
        /// <summary>
        /// Vrac� n�zev profilu. Implementov�no tak, �e vrac� cel� jm�no typu profilu.
        /// </summary>
        public string ProfileName => this.GetType().FullName;

        /// <summary>
        /// Vrac� profily (resp. jejich typy), kter� musej� b�t naseedov�ny p�ed t�mto profilem.
        /// </summary>
        public virtual IEnumerable<Type> GetPrerequisiteProfiles()
        {
            return Enumerable.Empty<Type>();
        }
    }
}