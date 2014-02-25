using System;
using System.Linq;

namespace CodeModel.FlowAnalysis
{
    public class PotentialType : IEquatable<PotentialType>
    {
        private static readonly Type[] NumericTypes = {typeof (decimal), typeof (double), typeof (float), typeof (UInt64), typeof (Int64), typeof (UInt32), typeof (Int32), typeof (UInt16), typeof (Int16), typeof (Byte), typeof (SByte)};

        public static readonly PotentialType String = PotentialType.FromType(typeof(string));        
        public static readonly PotentialType Boolean = PotentialType.FromType(typeof(bool));             

        public static readonly PotentialType Numeric = new NumericPotentialType();
        public static readonly PotentialType Token = new TokenPotentialType();
        public static readonly PotentialType MethodHandle = new MethodHandlePotentialType();

        public Type Type { get; private set; }        

        public static PotentialType FromType(Type type)
        {
            if (NumericTypes.Contains(type))
            {
                return Numeric;
            }

            return new PotentialType { Type = type };
        }

        public bool Equals(PotentialType other)
        {
            return this.GetType() == other.GetType() && this.Type == other.Type;
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
            return PotentialType.FromType(t);
        }

        public override string ToString()
        {
            return this.Type.Name;
        }

        public virtual PotentialType Box(Type boxedType)
        {
            return new PotentialType {Type = boxedType};
        }

        public PotentialType GetArrayElement()
        {
            return this.Type.GetElementType();
        }

        private class NumericPotentialType : PotentialType
        {
            public override string ToString()
            {
                return "Numeric";
            }

            public override PotentialType Box(Type boxedType)
            {
                if (boxedType == typeof(Boolean))
                {
                    return PotentialType.Boolean;
                }

                return this;
            }
        }
       
        private class TokenPotentialType : PotentialType
        {
            public override string ToString()
            {
                return "Token";
            }           
        }

        private class MethodHandlePotentialType : PotentialType
        {
            public override string ToString()
            {
                return "MethodHandle";
            }
        }
        
    }
}