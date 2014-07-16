using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
	public partial class InstructionVisitor<TState>
	{
		protected virtual TState HandleNop(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBreak(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg_0(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg_1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg_2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg_3(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc_0(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc_1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc_2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc_3(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc_0(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc_1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc_2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc_3(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarga_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStarg_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloca_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdnull(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_M1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_0(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_3(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_5(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_6(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_7(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdc_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleDup(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePop(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleJmp(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCall(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCalli(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRet(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBr_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBrfalse_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBrtrue_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBeq_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBge_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBgt_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBle_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBlt_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBne_Un_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBge_Un_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBgt_Un_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBle_Un_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBlt_Un_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBr(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBrfalse(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBrtrue(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBeq(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBge(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBgt(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBle(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBlt(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBne_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBge_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBgt_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBle_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBlt_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleSwitch(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_U1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_U2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_U4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdind_Ref(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_Ref(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleAdd(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleSub(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleMul(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleDiv(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleDiv_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRem(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRem_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleAnd(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleOr(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleXor(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleShl(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleShr(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleShr_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleNeg(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleNot(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_U4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_U8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCallvirt(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCpobj(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdobj(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdstr(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleNewobj(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCastclass(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleIsinst(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_R_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleUnbox(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleThrow(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdfld(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdflda(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStfld(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdsfld(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdsflda(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStsfld(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStobj(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I1_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I2_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I4_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I8_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U1_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U2_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U4_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U8_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleBox(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleNewarr(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdlen(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelema(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_U1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_U2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_U4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem_Ref(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_R4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_R8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem_Ref(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdelem(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStelem(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleUnbox_Any(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U8(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRefanyval(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCkfinite(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleMkrefany(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdtoken(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_U2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_U1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_Ovf_U(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleAdd_Ovf(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleAdd_Ovf_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleMul_Ovf(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleMul_Ovf_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleSub_Ovf(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleSub_Ovf_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleEndfinally(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLeave(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLeave_S(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStind_I(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConv_U(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix7(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix6(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix5(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix4(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix3(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix2(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefix1(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandlePrefixref(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleArglist(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCeq(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCgt(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCgt_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleClt(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleClt_Un(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdftn(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdvirtftn(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarg(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdarga(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStarg(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloc(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLdloca(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleStloc(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleLocalloc(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleEndfilter(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleUnaligned(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleVolatile(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleTailcall(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleInitobj(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleConstrained(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleCpblk(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleInitblk(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRethrow(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleSizeof(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleRefanytype(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

		protected virtual TState HandleReadonly(TState state, Instruction instruction)
		{
			return this.HandleUnrecognized(state, instruction);
		}

				
		protected virtual void RegisterHandlers(Dictionary<OpCode, Func<TState, Instruction, TState>> registry)
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