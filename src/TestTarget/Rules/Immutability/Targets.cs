namespace TestTarget.Rules.Immutability
{
    [Immutable]
    public class SetFieldOutsideOfCtor
    {
        private int myField;

        public void SetField()
        {
            this.myField = 4;
        }
    }

    [Immutable]
    public class SetPropertyOutsideOfCtor
    {
        public int Property { get; private set; }

        public void SetProperty()
        {
            this.Property = 66;
        }
    }

    [Immutable]
    public class SetFieldInCtor
    {
        private readonly int myField;

        public SetFieldInCtor()
        {
            this.myField = 8;
        }
    }

    [Immutable]
    public class SetPropertyInCtor
    {
        public int Property { get; private set; }

        public SetPropertyInCtor()
        {
            this.Property = 66;
        }
    }

    [Immutable]
    public class PublicPropertySetter
    {
        public string PublicSetter { get; set; }
    }

    [Immutable]
    public class WriteableField
    {
        private int field;
    }

    [Immutable]
    public class ReadonlyField
    {
        public readonly int Field = 0;
    }
}