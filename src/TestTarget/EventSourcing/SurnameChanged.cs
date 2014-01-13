namespace TestTarget.EventSourcing
{
    public class SurnameChanged
    {
        public string NewSurname { get; private set; }

        public SurnameChanged(string newSurname)
        {
            this.NewSurname = newSurname;
        }
    }
}