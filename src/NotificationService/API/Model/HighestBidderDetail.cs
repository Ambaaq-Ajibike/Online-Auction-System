namespace API.Model
{
    public class HighestBidderDetail
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }
}
