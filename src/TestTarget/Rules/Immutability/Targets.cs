namespace TestTarget.Rules.Immutability
{
    public class SetFieldOutsideOfCtor
    {
        private int myField;

        public void SetField()
        {
            this.myField = 4;
        }
    }

    public class SetPropertyOutsideOfCtor
    {
        public int Property { get; private set; }

        public void SetProperty()
        {
            this.Property = 66;
        }
    }

    public class SetFieldInCtor
    {
        private readonly int myField;

        public SetFieldInCtor()
        {
            this.myField = 8;
        }
    }

    public class SetPropertyInCtor
    {
        public int Property { get; private set; }

        public SetPropertyInCtor()
        {
            this.Property = 66;
        }
    }

    public class PublicPropertySetter
    {
        public string PublicSetter { get; set; }
    }

    public class WriteableField
    {
        private int field;
    }

    public class ReadonlyField
    {
        public readonly int Field = 0;
    }
}