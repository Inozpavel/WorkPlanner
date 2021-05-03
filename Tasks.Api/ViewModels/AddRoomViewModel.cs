using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.ViewModels
{
    public class AddRoomViewModel
    {
        [StringLength(30)]
        [Required]
        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }
    }
}