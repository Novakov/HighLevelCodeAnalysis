.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}

.class public TestTarget.IL.Base
{
	.method public void .ctor()
	{
		.maxstack 1
    
		ldarg.0
		call instance void [mscorlib]System.Object::.ctor()

		ret
	}
}

.class public TestTarget.IL.Derived1 extends TestTarget.IL.Base
{
	.method public void .ctor()
	{
		.maxstack 1
    
		ldarg.0
		call instance void TestTarget.IL.Base::.ctor()

		ret
	}
}

.class public TestTarget.IL.Derived2 extends TestTarget.IL.Base
{
	.method public void .ctor()
	{
		.maxstack 1
    
		ldarg.0
		call instance void TestTarget.IL.Base::.ctor()

		ret
	}
}

.class public TestTarget.IL.Derived3 extends TestTarget.IL.Derived1
{
	.method public void .ctor()
	{
		.maxstack 1
    
		ldarg.0
		call instance void TestTarget.IL.Derived1::.ctor()

		ret
	}
}

.class public TestTarget.IL.Derived4 extends TestTarget.IL.Derived1
{
	.method public void .ctor()
	{
		.maxstack 1
    
		ldarg.0
		call instance void TestTarget.IL.Derived1::.ctor()

		ret
	}
}