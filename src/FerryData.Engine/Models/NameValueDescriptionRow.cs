namespace FerryData.Engine.Models
{
    public class NameValueDescriptionRow : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public NameValueDescriptionRow()
            : base()
        {

        }
    }
}
