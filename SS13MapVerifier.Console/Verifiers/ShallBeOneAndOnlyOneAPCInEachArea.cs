using System;
using System.Collections.Generic;

using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Console.Verifiers
{
    internal class ShallBeOneAndOnlyOneApcInEachArea : AreaCheckBase
    {
        #region Properties

        protected override Func<int, bool> BadCountPredicate
        {
            get
            {
                return count => count != 1;
            }
        }

        protected override string CheckedType
        {
            get
            {
                return Objects.APC;
            }
        }

        protected override IEnumerable<int> CheckedZLevels
        {
            get
            {
                return new[] { 1, 5 };
            }
        }

        protected override string ErrorMessage
        {
            get
            {
                return "Bad APC count";
            }
        }

        protected override IEnumerable<string> IgnoredAreas
        {
            get
            {
                return new List<string>
                           {
                               Areas.Space, 
                               Areas.SupplyStation, 
                               Areas.SyndicateStation, 
                               Areas.VoxStation, 
                               Areas.Solar, 
                               Areas.Shuttle, 
                               Areas.Holodeck, 
                               Areas.MineAbanoned, 
                               Areas.MineUnexplored, 
                               Areas.Genetics
                           };
            }
        }

        #endregion
    }
}
