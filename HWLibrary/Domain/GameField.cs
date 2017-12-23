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
                case 'O':
                    IsGameFinished = true;
                    IsOwins = true;
                    break;
                case 'X':
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

        private char CheckonWhoWinner()
        {


            if(Cells[0].TypeofCell==Cells[1].TypeofCell
                &&Cells[1].TypeofCell==Cells[2].TypeofCell)
                switch (Cells[0].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if(Cells[0].TypeofCell == Cells[4].TypeofCell
               && Cells[4].TypeofCell == Cells[8].TypeofCell)
                switch (Cells[0].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[0].TypeofCell == Cells[3].TypeofCell
                && Cells[3].TypeofCell == Cells[6].TypeofCell)
                switch (Cells[0].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[1].TypeofCell == Cells[4].TypeofCell
                && Cells[4].TypeofCell == Cells[7].TypeofCell)
                switch (Cells[1].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[2].TypeofCell == Cells[5].TypeofCell
                && Cells[5].TypeofCell == Cells[8].TypeofCell)
                switch (Cells[2].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[2].TypeofCell == Cells[4].TypeofCell
                && Cells[4].TypeofCell == Cells[6].TypeofCell)
                switch (Cells[2].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[3].TypeofCell == Cells[4].TypeofCell
                && Cells[4].TypeofCell == Cells[5].TypeofCell)
                switch (Cells[3].TypeofCell)
                {
                    case TypeofCell.O:
                        return 'O';
                    case TypeofCell.X:
                        return 'X';
                    case TypeofCell.Null:
                        break;
                }
            if (Cells[6].TypeofCell != Cells[7].TypeofCell || Cells[7].TypeofCell != Cells[8].TypeofCell)
                return default(char);
            switch (Cells[6].TypeofCell)
            {
                case TypeofCell.O:
                    return 'O';
                case TypeofCell.X:
                    return 'X';
                case TypeofCell.Null:
                    break;
            }          
            return default(char);
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
