using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.Patterns.DataSeeds.Profiles;

namespace EnchantedCoder.Data.Patterns.Tests.DataSeeds.Infrastructure
{
    public class ProfileWithPrerequisite : DataSeedProfile
    {
        public override IEnumerable<Type> GetPrerequisiteProfiles()
        {
            yield return typeof(DefaultProfile);
        }
    }
}
