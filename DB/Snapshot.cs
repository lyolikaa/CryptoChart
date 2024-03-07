namespace DB;

public class Snapshot
{
    public int Id { get; set; }
    public ulong SnapshotId { get; set; }
    public DateTime Date { get; set; }
    public string Bids { get; set; }
    public string Asks { get; set; }
}

