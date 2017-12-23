using System;

namespace HWLibrary.Domain
{
    public class Motion
    {
        public Motion(Guid gameId, int cellId, string motionMaker)
        {
            GameId = gameId;
            CellId = cellId;
            MotionMaker = motionMaker;
        }

        public Guid GameId { get; }
        public int CellId { get; }
        public string MotionMaker { get; }
    }
}
