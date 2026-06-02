using Domain.Enums;
namespace Application.DTOs.Task
{
    public class UpdateTaskStatusRequest
    {
        public Domain.Enums.TaskStatus Status { get; set; }
    }
}