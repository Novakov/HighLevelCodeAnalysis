.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}

.class public TestTarget.IL.WalkCfg
{
	.method public static void MethodWithIf()
	{	
		ldc.i4 0

		call void [TestTarget.IL]TestTarget.IL.WalkCfg::Mark1()

		call bool [TestTarget.IL]TestTarget.IL.WalkCfg::Condition()				
		brtrue block2
	
		block1:
			call void [TestTarget.IL]TestTarget.IL.WalkCfg::Mark2()
			ldc.i4 1			
			br end_if1

		block2:
			call void [TestTarget.IL]TestTarget.IL.WalkCfg::Mark3()
			ldc.i4 2


		end_if1:

		call void [TestTarget.IL]TestTarget.IL.WalkCfg::Mark4()

		pop
		pop

		ret
	}		

	.method public static void Mark1()
	{
		ret
	}

	.method public static void Mark2()
	{
		ret
	}

	.method public static void Mark3()
	{
		ret
	}

	.method public static void Mark4()
	{
		ret
	}

	.method public static bool Condition()
	{
		ldc.i4.0

		ret
	}
}