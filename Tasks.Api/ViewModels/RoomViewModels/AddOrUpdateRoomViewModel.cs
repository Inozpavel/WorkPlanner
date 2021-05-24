using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.ViewModels.RoomViewModels
{
    public class AddOrUpdateRoomViewModel
    {
        [StringLength(30)]
        [Required]
        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }
    }
}