namespace FinalProgram_9.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public ICollection<QuestionTag> QuestionTags { get; set; }

        public Tag(string name)
        {
            this.Name = name;
            QuestionTags = new HashSet<QuestionTag>();
        }
    }
}
