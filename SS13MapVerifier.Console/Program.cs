using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SS13MapVerifier.Map;
using SS13MapVerifier.Verifiers;
using SS13MapVerifier.Verifiers.PipeVerifier;

namespace SS13MapVerifier.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Enter path to map");
            var map = MapParser.ParseFile(System.Console.ReadLine());

            var verifiers = new List<IVerifier>
                                {
                                    new TerminalsShallHaveOpenCableEndOnSameTurf(),
                                    new ShallBeOneAndOnlyOneApcInEachArea(),
                                    new OnlyAllowPureAreaInstances(),
                                    new ThereShallBeOnlyBeTwoPipeLines(),
                                    new ShouldBeAtLeastOneAirAlarmInMostAreas(),
                                    new ShallHaveNoStackedPipes(),
                                    new ShallHaveNoStackedPowerCables(),
                                    new VentsShouldHaveProperDefaultSettings()
                                };

            Parallel.ForEach(verifiers, verifier => Verify(verifier, map));
            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        private static void Verify(IVerifier verifier, IMap map)
        {
            foreach (var log in verifier.ValidateMap(map))
            {
                System.Console.WriteLine(log.Severity + " - " + log.Message + " - " + log.Tiles.First().Coordinate);
            }
        }
    }
}
