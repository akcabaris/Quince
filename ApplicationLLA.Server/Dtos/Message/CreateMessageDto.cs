using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Message
{
    public class CreateMessageDto
    {
        [StringLength(300, MinimumLength = 1)]
        public required string Content { get; set; }
    }
}
