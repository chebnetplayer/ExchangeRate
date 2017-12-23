namespace HWLibrary.Domain
{
    public class Cell
    {
        public int Id { get;}
        public TypeofCell TypeofCell { get; internal set; }
        public Cell(int id)
        {
            TypeofCell = TypeofCell.Null;
            Id = id;
        }
    }
}
