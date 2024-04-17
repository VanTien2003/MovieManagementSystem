namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditCinema
    {
        public string Address { get; set; } = "";
        public string Description { get; set; } = "";
        public string Code { get; set; } = "";
        public string NameOfCinema { get; set; } = "";

        public List<Request_Room>? EditRooms { get; set; }
    }
}
