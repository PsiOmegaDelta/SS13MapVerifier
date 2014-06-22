using System;

namespace SS13MapVerifier.Map
{
    [Flags]
    public enum Direction
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        Up = 16,
        Down = 32,
        Any = North | South | West | South | Up | Down
    }

    public static class Directions
    {
        public static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Any:
                    return Direction.None;
                case Direction.None:
                    return Direction.Any;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}
