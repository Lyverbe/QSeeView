namespace QSeeView.Models
{
    public class HardDiskInfoModel
    {
        public int Id { get; set; }
        public uint Capacity { get; set; }
        public uint FreeSpace { get; set; }

        public double PercentFreeSpace => (Capacity > 0) ? (FreeSpace / (double)Capacity) * 100 : 0;
    }
}
