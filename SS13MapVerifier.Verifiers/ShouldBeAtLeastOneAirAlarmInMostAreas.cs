using System;
using System.Collections.Generic;

using SS13MapVerifier.Map.Constants;

namespace SS13MapVerifier.Verifiers
{
    public class ShouldBeAtLeastOneAirAlarmInMostAreas : AreaCheckBase
    {
        #region Properties

        protected override Func<int, bool> BadCountPredicate
        {
            get
            {
                return count => count < 1;
            }
        }

        protected override string CheckedType
        {
            get
            {
                return Objects.AirAlarm;
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
                return "Bad air alarm count";
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
                                Areas.MineExplored,
                                Areas.MineUnexplored,
                                Areas.ToxinTestArea
                           };
            }
        }

        #endregion
    }
}