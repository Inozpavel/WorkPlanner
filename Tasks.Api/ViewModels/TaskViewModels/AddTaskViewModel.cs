using System;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Api.ViewModels.TaskViewModels
{
    public class AddTaskViewModel
    {
        [Required]
        [MinLength(1)]
        public string TaskName { get; set; }

        [Required]
        [MinLength(1)]
        public string? TaskContent { get; set; }

        public string? Details { get; set; }

        public DateTime DeadlineTime { get; set; }
    }
}