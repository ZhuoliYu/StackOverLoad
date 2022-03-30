﻿namespace FinalProgram_9.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DateTime? DateOfClose { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }

        // vote can be tracked by user, then influence reputation.
        //who&what vote track on 



        public string? UserId { get; set; } 
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Question(string title, string body, string userId)
        {
            this.Title = title;
            this.Body = body;
            this.UserId = userId;
            this.DateOfCreate = DateTime.Now;

            QuestionTags = new HashSet<QuestionTag>();
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
        }
        public Question()
        {
            QuestionTags = new HashSet<QuestionTag>();
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
        }
    }
}
