using System;

namespace CodeModel.FlowAnalysis
{
    public class PotentialType : IEquatable<PotentialType>
    {
        public static readonly PotentialType String = PotentialType.Simple(typeof(string));        
        public static readonly PotentialType Integer = PotentialType.Simple(typeof(int));        

        public Type Type { get; private set; }        

        public static PotentialType Simple(Type type)
        {
            return new PotentialType { Type = type };
        }

        public bool Equals(PotentialType other)
        {
            return this.Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            var type = obj as PotentialType;
            return type != null && Equals(type);
        }

        public override int GetHashCode()
        {
            return (this.Type != null ? this.Type.GetHashCode() : 0);
        }

        public static implicit operator PotentialType(Type t)
        {
            return PotentialType.Simple(t);
        }

        public override string ToString()
        {
            return this.Type.Name;
        }

        public PotentialType Box(Type boxedType)
        {
            return new PotentialType {Type = boxedType};
        }
    }
}