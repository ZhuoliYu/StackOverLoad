using FinalProgram_9.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProgram_9
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
