namespace TestTarget.EventSourcing
{
    public class Person : EntityBase
    {
        private string surname;

        public void ChangeSurname(string newSurname)
        {
            this.Apply(new SurnameChanged(newSurname));
        }

        private void On(SurnameChanged @event)
        {
            this.surname = @event.NewSurname;
        }
    }
}
