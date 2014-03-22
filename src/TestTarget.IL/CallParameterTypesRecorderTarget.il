.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}

.class public TestTarget.IL.CallParameterTypesRecorderTarget
{
	.field private static class [mscorlib]System.String staticField;
	.field private class [mscorlib]System.String instanceField;

	.method public newslot virtual instance void VirtualMeth() cil managed
	{
		ret
	}

	.method public static void Ldtoken()
	{
		ldtoken TestTarget.IL.Derived1

		nop

		pop

		ret
	}

	.method public static void Ldlen()
	{
		ldc.i4 5
		newarr [mscorlib]System.Object
		ldlen

		nop

		pop
		ret
	}	
	
	.method public static void Newarr()
	{
		ldc.i4 5
		newarr [mscorlib]System.Object

		nop

		pop

		ret
	}		

	.method public static void Ldftn()
	{
		ldftn void TestTarget.IL.CallParameterTypesRecorderTarget::Ldftn()

		nop

		pop

		ret
	}	

	.method public static void LdelemRef()
	{
		ldc.i4 5
		newarr [mscorlib]System.Object
		ldc.i4 2
		ldelem.ref
	
		nop

		pop

		ret
	}	

	.method public static void Ldelema()
	{
		ldc.i4 5
		newarr [mscorlib]System.Object
		ldc.i4 2
		ldelema [mscorlib]System.Object
	
		nop

		pop

		ret
	}	

	.method public static void ConvI()
	{
		ldc.i4 6
		conv.i

		nop

		pop	

		ret
	}	

	.method public static void ConvI1()
	{
		ldc.i4 6
		conv.i1

		nop

		pop	

		ret
	}	

	.method public static void ConvI2()
	{
		ldc.i4 6
		conv.i2

		nop

		pop	

		ret
	}	

	.method public static void ConvI4()
	{
		ldc.i4 6
		conv.i4

		nop

		pop	

		ret
	}
	
	.method public static void ConvI8()
	{
		ldc.i4 6
		conv.i4

		nop

		pop	

		ret
	}	

	.method public static void Ldsfld()
	{
		ldsfld class [mscorlib]System.String TestTarget.IL.CallParameterTypesRecorderTarget::staticField
		
		nop

		pop

		ret
	}

	.method public void Ldvirtftn()
	{
		ldarg.0
		ldvirtftn instance void TestTarget.IL.CallParameterTypesRecorderTarget::VirtualMeth()

		nop

		pop

		ret
	}		

	.method public static void Neg()
	{
		ldc.i4 17
		neg

		nop

		pop

		ret
	}

	.method public static void Not()
	{
		ldc.i4.1
		not

		nop

		pop

		ret
	}

	.method public static void Isinst()
	{
		ldstr "aaa"
		isinst [mscorlib]System.Object

		nop

		pop
		ret
	}

	.method public void Ldflda()
	{
		ldarg.0
		ldflda class [mscorlib]System.String TestTarget.IL.CallParameterTypesRecorderTarget::instanceField

		nop

		pop
		ret
	}

	.method public static void UnboxAny()
	{
		ldc.i4.5
		box [mscorlib]System.Int32
		unbox.any [mscorlib]System.Int32

		nop

		pop
		ret
	}

	.method public static void Unbox()
	{
		ldc.i4.5
		box [mscorlib]System.Int32
		unbox [mscorlib]System.Int32

		nop

		pop
		ret
	}
}