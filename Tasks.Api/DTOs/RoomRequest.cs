using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.DTOs
{
    public class RoomRequest
    {
        [Required]
        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }
    }
}