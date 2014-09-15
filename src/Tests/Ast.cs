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

            ast = new RewriteIf().Rewrite(ast);
            ast = new RewriteIfElse().Rewrite(ast);

            //ast.Dupa().Dupa2();

            var codeWriter = new WriteCode();
            codeWriter.Visit(ast.Body);

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

                if (loc_3 > 20)
                {
                    Console.WriteLine("Even greater");
                }

                if (loc_3 > 30)
                {
                    Console.WriteLine("Even event greater");
                }
                else
                {
                    Console.WriteLine("No so much");
                }
            }

            while (loc_3 < 1000)
            {
                loc_3++;
            }
        }
    }

    public class WriteCode : AstVisitorBase
    {
        private readonly StringBuilder code;
        private int indentLevel;

        public WriteCode()
        {
            this.code = new StringBuilder();
            this.indentLevel = 0;
        }

        public string Write()
        {
            return this.code.ToString();
        }

        private StringBuilder Indent()
        {
            return this.code.Append(' ', this.indentLevel*4);
        }

        private void Nested(Action action)
        {
            this.indentLevel++;

            action();
            
            this.indentLevel--;
        }

        public void On(StoreLocal storeLocal)
        {
            this.Indent()
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
            this.Indent();
            this.Visit(statement.Expression);
            this.code.Append(";").AppendLine();
        }

        public void On(ConditionalBranchStatement branch)
        {
            this.Indent().Append("if(");
            this.Visit(branch.Condition);
            this.code.Append(") goto IL_").Append(branch.Offset.ToString("X4")).Append(";").AppendLine();
        }

        public void On(ILStatement il)
        {
            this.Indent().Append(il.Instruction).AppendLine();
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
            this.Indent().Append("return");

            if (ret.Value != null)
            {
                this.code.Append(" ");
                this.Visit(ret.Value);
            }

            this.code.Append(";").AppendLine();
        }

        public void On(LabelStatement label)
        {
            this.Indent().Append("IL_").Append(label.Offset.ToString("X4")).AppendLine(": ");
        }

        public void On(IfStatement ifStatement)
        {
            this.Indent().Append("if(");
            this.Visit(ifStatement.Condition);
            this.code.Append(")").AppendLine();
            this.Visit(ifStatement.TrueStatement);

            if (ifStatement.ElseStatement != null)
            {
                this.Indent().Append("else").AppendLine();
                this.Visit(ifStatement.ElseStatement);
            }
        }


        public void On(BlockStatement block)
        {
            this.Indent().Append("{").AppendLine();
            this.Nested(() => this.Visit(block.Statements));
            this.Indent().Append("}").AppendLine();
        }

        public void On(GotoStatement @goto)
        {
            this.Indent().Append("goto ").Append("IL_").Append(@goto.Offset.ToString("X4")).Append(";").AppendLine();
        }
    }
}
