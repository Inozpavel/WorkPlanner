using System;

namespace Tasks.Api.ViewModels.RoomViewModels
{
    public class RoomViewModel
    {
        public Guid RoomId { get; set; }

        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }

        public DateTime CreationDate { get; set; }
    }
}