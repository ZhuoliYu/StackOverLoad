﻿namespace FinalProgram_9.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string ContentOfComment { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Question Question { get; set; }
        public virtual Answer? Answer { get; set; }

        public Comment(string userId, int questionId, string contentOfComment)
        {
            this.UserId = userId;
            this.QuestionId = questionId;
            this.ContentOfComment = contentOfComment;

        }

        public Comment(string userId, int? answerId, int questionId, string contentOfComment)
        {
            this.UserId = userId;
            this.AnswerId = answerId;
            this.ContentOfComment = contentOfComment;
        }
    }
}
