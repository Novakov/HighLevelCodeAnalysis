using System;

namespace CodeModel.FlowAnalysis
{
    public class PotentialType : IEquatable<PotentialType>
    {
        public static readonly PotentialType String = PotentialType.Simple(typeof(string));        
        public static readonly PotentialType Integer = PotentialType.Simple(typeof(int));        
        public static readonly PotentialType Decimal = PotentialType.Simple(typeof(decimal));        
        public static readonly PotentialType Double = PotentialType.Simple(typeof(double));        
        public static readonly PotentialType Float = PotentialType.Simple(typeof(float));        
        public static readonly PotentialType Long = PotentialType.Simple(typeof(long));        
        public static readonly PotentialType UnsignedLong = PotentialType.Simple(typeof(ulong));        
        public static readonly PotentialType UnsignedInt = PotentialType.Simple(typeof(uint));        
        public static readonly PotentialType UnsignedShort = PotentialType.Simple(typeof(ushort));        
        public static readonly PotentialType SignedByte = PotentialType.Simple(typeof(sbyte));        
        public static readonly PotentialType Byte = PotentialType.Simple(typeof(byte));        
        public static readonly PotentialType Short = PotentialType.Simple(typeof(short));        

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

        public PotentialType AfterBinaryOperationWith(PotentialType other)
        {
            if (this.Equals(Decimal) || other.Equals(Decimal))
            {
                return Decimal;
            }

            if (this.Equals(Double) || other.Equals(Double))
            {
                return Double;
            }

            if (this.Equals(Float) || other.Equals(Float))
            {
                return Float;
            }

            if (this.Equals(UnsignedLong) || other.Equals(UnsignedLong))
            {
                return UnsignedLong;
            }

            if (this.Equals(Long) || other.Equals(Long))
            {
                return Long;
            }

            if (this.Equals(UnsignedInt) && (other.Equals(SignedByte) || other.Equals(Short) || other.Equals(Integer))
             ||other.Equals(UnsignedInt) && (this.Equals(SignedByte) || this.Equals(Short) || this.Equals(Integer)))
            {
                return Long;
            }

            if (this.Equals(UnsignedInt) || other.Equals(UnsignedInt))
            {
                return UnsignedInt;
            }

            return Integer;
        }

        public PotentialType ArithmeticBinaryWith(PotentialType other)
        {
            if (this.Equals(Integer) || other.Equals(Integer))
            {
                return Integer;
            }

            if (this.Equals(Long) || other.Equals(Long))
            {
                return Long;
            }

            if (this.Equals(Float) || other.Equals(Float))
            {
                return Float;
            }

            if (this.Equals(Double) || other.Equals(Double))
            {
                return Double;
            }

            throw new NotImplementedException();
        }

        public PotentialType BitwiseBinaryWith(PotentialType other)
        {
            if (this.Equals(Integer) || other.Equals(Integer))
            {
                return Integer;
            }

            if (this.Equals(Long) || other.Equals(Long))
            {
                return Long;
            }

            return Integer;
        }

        public PotentialType Signed()
        {
            if (this.Equals(UnsignedLong)) return Long;
            if (this.Equals(UnsignedInt)) return Integer;
            if (this.Equals(UnsignedShort)) return Short;
            if (this.Equals(Byte)) return SignedByte;

            return this;
        }
    }
}