using System;

namespace SS13MapVerifier.Map
{
    [Flags]
    public enum Directions
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        Up = 16,
        Down = 32,
        Cardinal = North | South | West | East,
        Any = North | South | West | South | Up | Down
    }

    public static class Direction
    {
        public static Directions GetOppositeDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.East:
                    return Directions.West;
                case Directions.South:
                    return Directions.North;
                case Directions.West:
                    return Directions.East;
                case Directions.Up:
                    return Directions.Down;
                case Directions.Down:
                    return Directions.Up;
                case Directions.Any:
                    return Directions.None;
                case Directions.None:
                    return Directions.Any;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}
