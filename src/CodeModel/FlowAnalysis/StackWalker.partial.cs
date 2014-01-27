using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
	public partial class StackWalker
	{
		protected virtual void HandleNop(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBreak(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg_0(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg_1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg_2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg_3(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc_0(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc_1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc_2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc_3(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc_0(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc_1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc_2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc_3(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarga_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStarg_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloca_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdnull(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_M1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_0(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_3(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_5(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_6(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_7(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdc_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleDup(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePop(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleJmp(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCall(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCalli(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRet(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBr_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBrfalse_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBrtrue_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBeq_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBge_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBgt_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBle_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBlt_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBne_Un_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBge_Un_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBgt_Un_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBle_Un_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBlt_Un_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBr(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBrfalse(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBrtrue(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBeq(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBge(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBgt(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBle(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBlt(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBne_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBge_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBgt_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBle_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBlt_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleSwitch(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_U1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_U2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_U4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdind_Ref(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_Ref(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleAdd(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleSub(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleMul(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleDiv(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleDiv_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRem(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRem_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleAnd(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleOr(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleXor(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleShl(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleShr(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleShr_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleNeg(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleNot(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_U4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_U8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCallvirt(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCpobj(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdobj(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdstr(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleNewobj(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCastclass(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleIsinst(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_R_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleUnbox(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleThrow(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdfld(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdflda(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStfld(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdsfld(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdsflda(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStsfld(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStobj(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I1_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I2_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I4_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I8_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U1_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U2_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U4_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U8_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleBox(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleNewarr(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdlen(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelema(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_U1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_U2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_U4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem_Ref(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_R4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_R8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem_Ref(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdelem(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStelem(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleUnbox_Any(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U8(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRefanyval(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCkfinite(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleMkrefany(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdtoken(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_U2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_U1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_Ovf_U(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleAdd_Ovf(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleAdd_Ovf_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleMul_Ovf(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleMul_Ovf_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleSub_Ovf(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleSub_Ovf_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleEndfinally(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLeave(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLeave_S(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStind_I(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConv_U(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix7(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix6(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix5(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix4(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix3(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix2(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefix1(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandlePrefixref(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleArglist(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCeq(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCgt(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCgt_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleClt(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleClt_Un(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdftn(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdvirtftn(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarg(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdarga(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStarg(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloc(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLdloca(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleStloc(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleLocalloc(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleEndfilter(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleUnaligned(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleVolatile(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleTailcall(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleInitobj(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleConstrained(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleCpblk(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleInitblk(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRethrow(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleSizeof(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleRefanytype(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

		protected virtual void HandleReadonly(Instruction instruction)
		{
			this.HandleUnrecognized(instruction);
		}

				
		protected virtual void RegisterHandlers(Dictionary<OpCode, Action<Instruction>> registry)
		{
			registry[OpCodes.Nop] = HandleNop;
			registry[OpCodes.Break] = HandleBreak;
			registry[OpCodes.Ldarg_0] = HandleLdarg_0;
			registry[OpCodes.Ldarg_1] = HandleLdarg_1;
			registry[OpCodes.Ldarg_2] = HandleLdarg_2;
			registry[OpCodes.Ldarg_3] = HandleLdarg_3;
			registry[OpCodes.Ldloc_0] = HandleLdloc_0;
			registry[OpCodes.Ldloc_1] = HandleLdloc_1;
			registry[OpCodes.Ldloc_2] = HandleLdloc_2;
			registry[OpCodes.Ldloc_3] = HandleLdloc_3;
			registry[OpCodes.Stloc_0] = HandleStloc_0;
			registry[OpCodes.Stloc_1] = HandleStloc_1;
			registry[OpCodes.Stloc_2] = HandleStloc_2;
			registry[OpCodes.Stloc_3] = HandleStloc_3;
			registry[OpCodes.Ldarg_S] = HandleLdarg_S;
			registry[OpCodes.Ldarga_S] = HandleLdarga_S;
			registry[OpCodes.Starg_S] = HandleStarg_S;
			registry[OpCodes.Ldloc_S] = HandleLdloc_S;
			registry[OpCodes.Ldloca_S] = HandleLdloca_S;
			registry[OpCodes.Stloc_S] = HandleStloc_S;
			registry[OpCodes.Ldnull] = HandleLdnull;
			registry[OpCodes.Ldc_I4_M1] = HandleLdc_I4_M1;
			registry[OpCodes.Ldc_I4_0] = HandleLdc_I4_0;
			registry[OpCodes.Ldc_I4_1] = HandleLdc_I4_1;
			registry[OpCodes.Ldc_I4_2] = HandleLdc_I4_2;
			registry[OpCodes.Ldc_I4_3] = HandleLdc_I4_3;
			registry[OpCodes.Ldc_I4_4] = HandleLdc_I4_4;
			registry[OpCodes.Ldc_I4_5] = HandleLdc_I4_5;
			registry[OpCodes.Ldc_I4_6] = HandleLdc_I4_6;
			registry[OpCodes.Ldc_I4_7] = HandleLdc_I4_7;
			registry[OpCodes.Ldc_I4_8] = HandleLdc_I4_8;
			registry[OpCodes.Ldc_I4_S] = HandleLdc_I4_S;
			registry[OpCodes.Ldc_I4] = HandleLdc_I4;
			registry[OpCodes.Ldc_I8] = HandleLdc_I8;
			registry[OpCodes.Ldc_R4] = HandleLdc_R4;
			registry[OpCodes.Ldc_R8] = HandleLdc_R8;
			registry[OpCodes.Dup] = HandleDup;
			registry[OpCodes.Pop] = HandlePop;
			registry[OpCodes.Jmp] = HandleJmp;
			registry[OpCodes.Call] = HandleCall;
			registry[OpCodes.Calli] = HandleCalli;
			registry[OpCodes.Ret] = HandleRet;
			registry[OpCodes.Br_S] = HandleBr_S;
			registry[OpCodes.Brfalse_S] = HandleBrfalse_S;
			registry[OpCodes.Brtrue_S] = HandleBrtrue_S;
			registry[OpCodes.Beq_S] = HandleBeq_S;
			registry[OpCodes.Bge_S] = HandleBge_S;
			registry[OpCodes.Bgt_S] = HandleBgt_S;
			registry[OpCodes.Ble_S] = HandleBle_S;
			registry[OpCodes.Blt_S] = HandleBlt_S;
			registry[OpCodes.Bne_Un_S] = HandleBne_Un_S;
			registry[OpCodes.Bge_Un_S] = HandleBge_Un_S;
			registry[OpCodes.Bgt_Un_S] = HandleBgt_Un_S;
			registry[OpCodes.Ble_Un_S] = HandleBle_Un_S;
			registry[OpCodes.Blt_Un_S] = HandleBlt_Un_S;
			registry[OpCodes.Br] = HandleBr;
			registry[OpCodes.Brfalse] = HandleBrfalse;
			registry[OpCodes.Brtrue] = HandleBrtrue;
			registry[OpCodes.Beq] = HandleBeq;
			registry[OpCodes.Bge] = HandleBge;
			registry[OpCodes.Bgt] = HandleBgt;
			registry[OpCodes.Ble] = HandleBle;
			registry[OpCodes.Blt] = HandleBlt;
			registry[OpCodes.Bne_Un] = HandleBne_Un;
			registry[OpCodes.Bge_Un] = HandleBge_Un;
			registry[OpCodes.Bgt_Un] = HandleBgt_Un;
			registry[OpCodes.Ble_Un] = HandleBle_Un;
			registry[OpCodes.Blt_Un] = HandleBlt_Un;
			registry[OpCodes.Switch] = HandleSwitch;
			registry[OpCodes.Ldind_I1] = HandleLdind_I1;
			registry[OpCodes.Ldind_U1] = HandleLdind_U1;
			registry[OpCodes.Ldind_I2] = HandleLdind_I2;
			registry[OpCodes.Ldind_U2] = HandleLdind_U2;
			registry[OpCodes.Ldind_I4] = HandleLdind_I4;
			registry[OpCodes.Ldind_U4] = HandleLdind_U4;
			registry[OpCodes.Ldind_I8] = HandleLdind_I8;
			registry[OpCodes.Ldind_I] = HandleLdind_I;
			registry[OpCodes.Ldind_R4] = HandleLdind_R4;
			registry[OpCodes.Ldind_R8] = HandleLdind_R8;
			registry[OpCodes.Ldind_Ref] = HandleLdind_Ref;
			registry[OpCodes.Stind_Ref] = HandleStind_Ref;
			registry[OpCodes.Stind_I1] = HandleStind_I1;
			registry[OpCodes.Stind_I2] = HandleStind_I2;
			registry[OpCodes.Stind_I4] = HandleStind_I4;
			registry[OpCodes.Stind_I8] = HandleStind_I8;
			registry[OpCodes.Stind_R4] = HandleStind_R4;
			registry[OpCodes.Stind_R8] = HandleStind_R8;
			registry[OpCodes.Add] = HandleAdd;
			registry[OpCodes.Sub] = HandleSub;
			registry[OpCodes.Mul] = HandleMul;
			registry[OpCodes.Div] = HandleDiv;
			registry[OpCodes.Div_Un] = HandleDiv_Un;
			registry[OpCodes.Rem] = HandleRem;
			registry[OpCodes.Rem_Un] = HandleRem_Un;
			registry[OpCodes.And] = HandleAnd;
			registry[OpCodes.Or] = HandleOr;
			registry[OpCodes.Xor] = HandleXor;
			registry[OpCodes.Shl] = HandleShl;
			registry[OpCodes.Shr] = HandleShr;
			registry[OpCodes.Shr_Un] = HandleShr_Un;
			registry[OpCodes.Neg] = HandleNeg;
			registry[OpCodes.Not] = HandleNot;
			registry[OpCodes.Conv_I1] = HandleConv_I1;
			registry[OpCodes.Conv_I2] = HandleConv_I2;
			registry[OpCodes.Conv_I4] = HandleConv_I4;
			registry[OpCodes.Conv_I8] = HandleConv_I8;
			registry[OpCodes.Conv_R4] = HandleConv_R4;
			registry[OpCodes.Conv_R8] = HandleConv_R8;
			registry[OpCodes.Conv_U4] = HandleConv_U4;
			registry[OpCodes.Conv_U8] = HandleConv_U8;
			registry[OpCodes.Callvirt] = HandleCallvirt;
			registry[OpCodes.Cpobj] = HandleCpobj;
			registry[OpCodes.Ldobj] = HandleLdobj;
			registry[OpCodes.Ldstr] = HandleLdstr;
			registry[OpCodes.Newobj] = HandleNewobj;
			registry[OpCodes.Castclass] = HandleCastclass;
			registry[OpCodes.Isinst] = HandleIsinst;
			registry[OpCodes.Conv_R_Un] = HandleConv_R_Un;
			registry[OpCodes.Unbox] = HandleUnbox;
			registry[OpCodes.Throw] = HandleThrow;
			registry[OpCodes.Ldfld] = HandleLdfld;
			registry[OpCodes.Ldflda] = HandleLdflda;
			registry[OpCodes.Stfld] = HandleStfld;
			registry[OpCodes.Ldsfld] = HandleLdsfld;
			registry[OpCodes.Ldsflda] = HandleLdsflda;
			registry[OpCodes.Stsfld] = HandleStsfld;
			registry[OpCodes.Stobj] = HandleStobj;
			registry[OpCodes.Conv_Ovf_I1_Un] = HandleConv_Ovf_I1_Un;
			registry[OpCodes.Conv_Ovf_I2_Un] = HandleConv_Ovf_I2_Un;
			registry[OpCodes.Conv_Ovf_I4_Un] = HandleConv_Ovf_I4_Un;
			registry[OpCodes.Conv_Ovf_I8_Un] = HandleConv_Ovf_I8_Un;
			registry[OpCodes.Conv_Ovf_U1_Un] = HandleConv_Ovf_U1_Un;
			registry[OpCodes.Conv_Ovf_U2_Un] = HandleConv_Ovf_U2_Un;
			registry[OpCodes.Conv_Ovf_U4_Un] = HandleConv_Ovf_U4_Un;
			registry[OpCodes.Conv_Ovf_U8_Un] = HandleConv_Ovf_U8_Un;
			registry[OpCodes.Conv_Ovf_I_Un] = HandleConv_Ovf_I_Un;
			registry[OpCodes.Conv_Ovf_U_Un] = HandleConv_Ovf_U_Un;
			registry[OpCodes.Box] = HandleBox;
			registry[OpCodes.Newarr] = HandleNewarr;
			registry[OpCodes.Ldlen] = HandleLdlen;
			registry[OpCodes.Ldelema] = HandleLdelema;
			registry[OpCodes.Ldelem_I1] = HandleLdelem_I1;
			registry[OpCodes.Ldelem_U1] = HandleLdelem_U1;
			registry[OpCodes.Ldelem_I2] = HandleLdelem_I2;
			registry[OpCodes.Ldelem_U2] = HandleLdelem_U2;
			registry[OpCodes.Ldelem_I4] = HandleLdelem_I4;
			registry[OpCodes.Ldelem_U4] = HandleLdelem_U4;
			registry[OpCodes.Ldelem_I8] = HandleLdelem_I8;
			registry[OpCodes.Ldelem_I] = HandleLdelem_I;
			registry[OpCodes.Ldelem_R4] = HandleLdelem_R4;
			registry[OpCodes.Ldelem_R8] = HandleLdelem_R8;
			registry[OpCodes.Ldelem_Ref] = HandleLdelem_Ref;
			registry[OpCodes.Stelem_I] = HandleStelem_I;
			registry[OpCodes.Stelem_I1] = HandleStelem_I1;
			registry[OpCodes.Stelem_I2] = HandleStelem_I2;
			registry[OpCodes.Stelem_I4] = HandleStelem_I4;
			registry[OpCodes.Stelem_I8] = HandleStelem_I8;
			registry[OpCodes.Stelem_R4] = HandleStelem_R4;
			registry[OpCodes.Stelem_R8] = HandleStelem_R8;
			registry[OpCodes.Stelem_Ref] = HandleStelem_Ref;
			registry[OpCodes.Ldelem] = HandleLdelem;
			registry[OpCodes.Stelem] = HandleStelem;
			registry[OpCodes.Unbox_Any] = HandleUnbox_Any;
			registry[OpCodes.Conv_Ovf_I1] = HandleConv_Ovf_I1;
			registry[OpCodes.Conv_Ovf_U1] = HandleConv_Ovf_U1;
			registry[OpCodes.Conv_Ovf_I2] = HandleConv_Ovf_I2;
			registry[OpCodes.Conv_Ovf_U2] = HandleConv_Ovf_U2;
			registry[OpCodes.Conv_Ovf_I4] = HandleConv_Ovf_I4;
			registry[OpCodes.Conv_Ovf_U4] = HandleConv_Ovf_U4;
			registry[OpCodes.Conv_Ovf_I8] = HandleConv_Ovf_I8;
			registry[OpCodes.Conv_Ovf_U8] = HandleConv_Ovf_U8;
			registry[OpCodes.Refanyval] = HandleRefanyval;
			registry[OpCodes.Ckfinite] = HandleCkfinite;
			registry[OpCodes.Mkrefany] = HandleMkrefany;
			registry[OpCodes.Ldtoken] = HandleLdtoken;
			registry[OpCodes.Conv_U2] = HandleConv_U2;
			registry[OpCodes.Conv_U1] = HandleConv_U1;
			registry[OpCodes.Conv_I] = HandleConv_I;
			registry[OpCodes.Conv_Ovf_I] = HandleConv_Ovf_I;
			registry[OpCodes.Conv_Ovf_U] = HandleConv_Ovf_U;
			registry[OpCodes.Add_Ovf] = HandleAdd_Ovf;
			registry[OpCodes.Add_Ovf_Un] = HandleAdd_Ovf_Un;
			registry[OpCodes.Mul_Ovf] = HandleMul_Ovf;
			registry[OpCodes.Mul_Ovf_Un] = HandleMul_Ovf_Un;
			registry[OpCodes.Sub_Ovf] = HandleSub_Ovf;
			registry[OpCodes.Sub_Ovf_Un] = HandleSub_Ovf_Un;
			registry[OpCodes.Endfinally] = HandleEndfinally;
			registry[OpCodes.Leave] = HandleLeave;
			registry[OpCodes.Leave_S] = HandleLeave_S;
			registry[OpCodes.Stind_I] = HandleStind_I;
			registry[OpCodes.Conv_U] = HandleConv_U;
			registry[OpCodes.Prefix7] = HandlePrefix7;
			registry[OpCodes.Prefix6] = HandlePrefix6;
			registry[OpCodes.Prefix5] = HandlePrefix5;
			registry[OpCodes.Prefix4] = HandlePrefix4;
			registry[OpCodes.Prefix3] = HandlePrefix3;
			registry[OpCodes.Prefix2] = HandlePrefix2;
			registry[OpCodes.Prefix1] = HandlePrefix1;
			registry[OpCodes.Prefixref] = HandlePrefixref;
			registry[OpCodes.Arglist] = HandleArglist;
			registry[OpCodes.Ceq] = HandleCeq;
			registry[OpCodes.Cgt] = HandleCgt;
			registry[OpCodes.Cgt_Un] = HandleCgt_Un;
			registry[OpCodes.Clt] = HandleClt;
			registry[OpCodes.Clt_Un] = HandleClt_Un;
			registry[OpCodes.Ldftn] = HandleLdftn;
			registry[OpCodes.Ldvirtftn] = HandleLdvirtftn;
			registry[OpCodes.Ldarg] = HandleLdarg;
			registry[OpCodes.Ldarga] = HandleLdarga;
			registry[OpCodes.Starg] = HandleStarg;
			registry[OpCodes.Ldloc] = HandleLdloc;
			registry[OpCodes.Ldloca] = HandleLdloca;
			registry[OpCodes.Stloc] = HandleStloc;
			registry[OpCodes.Localloc] = HandleLocalloc;
			registry[OpCodes.Endfilter] = HandleEndfilter;
			registry[OpCodes.Unaligned] = HandleUnaligned;
			registry[OpCodes.Volatile] = HandleVolatile;
			registry[OpCodes.Tailcall] = HandleTailcall;
			registry[OpCodes.Initobj] = HandleInitobj;
			registry[OpCodes.Constrained] = HandleConstrained;
			registry[OpCodes.Cpblk] = HandleCpblk;
			registry[OpCodes.Initblk] = HandleInitblk;
			registry[OpCodes.Rethrow] = HandleRethrow;
			registry[OpCodes.Sizeof] = HandleSizeof;
			registry[OpCodes.Refanytype] = HandleRefanytype;
			registry[OpCodes.Readonly] = HandleReadonly;
						
		}		
	}
}