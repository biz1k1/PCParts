namespace PCParts.API.Model.Models
{
    public record CreatePendingUser
    {
        public string Phone { get; init; }
        public string Password { get; init; }
    }
}
