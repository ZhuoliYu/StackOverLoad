namespace FinalProgram_9.Models
{
    public class QuestionTag
    {
        private int? tagId2;

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int TagId { get; set; }

        public Question Question { get; set; }
        public Tag Tag { get; set; }


        public QuestionTag(int questionId, int tagId)
        {
            this.QuestionId = questionId;
            this.TagId = tagId;
        }
        public QuestionTag(int questionId, int? tagId2)
        {
            this.QuestionId = questionId;
            this.tagId2 = tagId2;
        }
    }
}
