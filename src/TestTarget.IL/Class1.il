.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}

.class public TestTarget.IL.Test
{
	.method public static void Kopytko() cil managed
	{
		newobj instance void TestTarget.IL.Derived1::.ctor()

		ldc.i4.2
		ldc.i4.3
		bgt greater
		
		newobj instance void TestTarget.IL.Derived2::.ctor()
		br end

		greater:
		newobj instance void TestTarget.IL.Derived3::.ctor()
		end:

		newobj instance void TestTarget.IL.Derived4::.ctor()

		call void TestTarget.IL.Test::AcceptBases(class TestTarget.IL.Base, class TestTarget.IL.Base, class TestTarget.IL.Base)		
		ret
	}

	.method public static void ManyInts(int32 a, int32 b, int32 c) cil managed
	{
		ret
	}

	.method public static void AcceptBases(class TestTarget.IL.Base a, class TestTarget.IL.Base b, class TestTarget.IL.Base c) cil managed
	{
		ret
	}
}