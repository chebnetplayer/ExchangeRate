using System;

namespace HWLibrary.Domain
{
    public class GameField
    {
        public bool CheckOnFinishing()
        {
            var checker = CheckonWhoWinner();
            switch (checker)
            {
                case TypeofCell.O:
                    IsGameFinished = true;
                    IsOwins = true;
                    break;
                case TypeofCell.X:
                    IsGameFinished = true;
                    IsXwins = true;
                    break;
            }
            return IsGameFinished;
        }

        public void MakeMotion(int cellid, TypeofCell typeofCell)
        {
            Cells[cellid].TypeofCell = typeofCell;
        }

        public GameField(User host, Guid gameId, Cell[] cells, bool isHostX)
        {
            Host = host;
            GameId = gameId;
            Cells = cells;
            IsHostX = isHostX;
            Name = host.Name;
        }

        private TypeofCell Check(int cell1, int cell2, int cell3)
        {
            if (Cells[cell1].TypeofCell != Cells[cell2].TypeofCell || Cells[cell2].TypeofCell != Cells[cell3].TypeofCell
            ) return TypeofCell.Null;
            switch (Cells[cell1].TypeofCell)
            {
                case TypeofCell.O:
                    return TypeofCell.O;
                case TypeofCell.X:
                    return TypeofCell.X;
                case TypeofCell.Null:
                    break;
            }
            return TypeofCell.Null;
        }

        private TypeofCell CheckonWhoWinner()
        {
            var checker = Check(0, 1, 2);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(0, 4, 8);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(0, 3, 6);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(1, 4, 7);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker=Check(2, 5, 8);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(2, 4, 6);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(3, 4, 5);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            checker= Check(6, 7, 8);
            if (checker == TypeofCell.O || checker == TypeofCell.X)
                return checker;
            return checker;
        }


        public void ComeGamerInGame(User gamer)
        {
            Gamer = gamer;
        }
        public bool IsGameFinished { get; private set; }
        public bool IsXwins { get; private set; }
        public bool IsOwins { get; private set; }
        public bool IsHostX { get;}
        public Guid GameId { get; }
        public User Host { get; }
        public string Name { get; }
        public User Gamer { get; private set; }
        public Cell[] Cells { get;}
    }
}
