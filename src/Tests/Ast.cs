using System;
using System.Text;
using CodeModel.Ast;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class AstTest
    {
        [Test]
        public void Test()
        {
            var method = Get.MethodOf<AstTest>(x => x.Target());

            var ast = AstBuilder.BuildForMethod(method);

            var codeWriter = new WriteCode();
            codeWriter.Visit(ast);

            Console.WriteLine(codeWriter.Write());
        }

        public void Target()
        {
            var loc_0 = "Var1";
            var loc_1 = "Var2";
            var loc_2 = loc_0 + loc_1;
            var loc_3 = loc_2.Length;

            if (loc_3 > 10)
            {
                Console.WriteLine("Greater");
            }
        }
    }

    public class WriteCode : AstVisitorBase
    {
        private readonly StringBuilder code;

        public WriteCode()
        {
            this.code = new StringBuilder();
        }

        public string Write()
        {
            return this.code.ToString();
        }

        public void On(StoreLocal storeLocal)
        {
            this.code
                .Append("loc_")
                .Append(storeLocal.Variable.LocalIndex)
                .Append(" = ");
            
            this.Visit(storeLocal.Value);

            this.code.Append(";").AppendLine();
        }

        public void On(LoadVariableExpression loadVariable)
        {
            this.code.Append("loc_").Append(loadVariable.Variable.LocalIndex);
        }

        public void On(ConstantExpression constant)
        {
            if (constant.Type == typeof(string))
            {
                this.code.Append("\"").Append(constant.Value).Append("\"");
            }
            else
            {
                this.code.Append(constant.Value);
            }
        }

        public void On(MethodCallExpression call)
        {
            if (call.Method.IsStatic)
            {
                this.code.Append(call.Method.DeclaringType.FullName);
            }
            else
            {
                this.Visit(call.Target);
            }

            this.code.Append(".").Append(call.Method.Name).Append("(");

            foreach (var parameter in call.Parameters)
            {
                this.Visit(parameter);
                this.code.Append(", ");
            }            

            this.code.Append(")");
        }

        public void On(ExpressionStatement statement)
        {
            this.Visit(statement.Expression);
            this.code.Append(";").AppendLine();
        }

        public void On(ConditionalBranchStatement branch)
        {
            this.code.Append("if(");
            this.Visit(branch.Condition);
            this.code.Append(") goto IL_").Append(branch.Offset.ToString("X4")).Append(";").AppendLine();
        }

        public void On(ILStatement il)
        {
            this.code.Append(il.Instruction).AppendLine();
        }

        public void On(BinaryExpression binary)
        {
            this.code.Append("(");

            this.Visit(binary.Left);

            this.code.Append(" ").Append(binary.Operator).Append(" ");

            this.Visit(binary.Right);

            this.code.Append(")");
        }

        public void On(ReturnStatement ret)
        {
            this.code.Append("return");

            if (ret.Value != null)
            {
                this.code.Append(" ");
                this.Visit(ret.Value);                
            }

            this.code.Append(";").AppendLine();
        }

        public void On(LabelStatement label)
        {
            this.code.Append("IL_").Append(label.Offset.ToString("X4")).Append(": ");
        }
    }
}
