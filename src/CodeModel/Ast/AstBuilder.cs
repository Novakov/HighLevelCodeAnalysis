using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CodeModel.FlowAnalysis;
using Mono.Reflection;

namespace CodeModel.Ast
{
    public class AstBuilder : ResolvingInstructionVisitor<object>
    {
        private readonly Stack<Expression> expressions;
        private readonly List<Statement> statements;
        private readonly HashSet<int> labels;

        public AstBuilder(IEnumerable<int> labels)
        {
            this.expressions = new Stack<Expression>();
            this.statements = new List<Statement>();
            this.labels = new HashSet<int>(labels);
        }

        public static MethodDeclaration BuildForMethod(MethodInfo method)
        {
            var cfg = ControlFlowGraphFactory.BuildForMethod(method);

            var labels = cfg.Blocks.OfType<InstructionBlockNode>().Select(x => x.First.Offset).Where(x => x != 0);

            var builder = new AstBuilder(labels);

            builder.Initialize(method);

            builder.Visit(null, method.GetInstructions());

            return new MethodDeclaration(method, new BlockStatement(builder.statements));
        }

        protected override void RegisterHandlers(Dictionary<OpCode, Func<object, Instruction, object>> registry)
        {
            base.RegisterHandlers(registry);

            registry[OpCodes.Callvirt] = registry[OpCodes.Call];
        }

        protected override object HandleLdstr(object state, Instruction instruction)
        {
            this.expressions.Push(new ConstantExpression(instruction.Operand.GetType(), instruction.Operand));

            return null;
        }

        protected override object HandleLoadInt32(object state, Instruction instruction, int constant)
        {
            this.expressions.Push(new ConstantExpression(constant.GetType(), constant));

            return null;
        }

        protected override object HandleStoreVariable(object state, Instruction instruction, LocalVariableInfo variable)
        {
            var expression = this.expressions.Pop();

            this.statements.Add(new StoreLocal(variable, expression).WithInstruction(instruction));
            return null;
        }

        protected override object HandleLoadVariable(object state, Instruction instruction, LocalVariableInfo variable)
        {
            this.expressions.Push(new LoadVariableExpression(variable));

            return null;
        }

        protected override object HandleCall(object state, Instruction instruction)
        {
            var callee = (MethodInfo)instruction.Operand;

            Expression @this = null;

            if (!callee.IsStatic)
            {
                @this = this.expressions.Pop();
            }

            var parameters = new List<Expression>();
            foreach (var parameter in callee.GetParameters())
            {
                parameters.Add(this.expressions.Pop());
            }

            var callExpression = new MethodCallExpression(callee.ReturnType, callee, @this, parameters);

            if (callee.ReturnType == typeof(void))
            {
                this.statements.Add(new ExpressionStatement(callExpression).WithInstruction(instruction));
            }
            else
            {
                this.expressions.Push(callExpression);
            }

            return null;
        }

        protected override object HandleBrtrue_S(object state, Instruction instruction)
        {
            var condition = this.expressions.Pop();

            var offset = ((Instruction)instruction.Operand).Offset;
            this.statements.Add(new ConditionalBranchStatement(condition, offset).WithInstruction(instruction));

            return null;
        }

        protected override object HandleBr_S(object state, Instruction instruction)
        {
            var offset = ((Instruction)instruction.Operand).Offset;

            this.statements.Add(new GotoStatement(offset).WithInstruction(instruction));

            return null;
        }

        protected override object HandleNop(object state, Instruction instruction)
        {
            return null;
        }

        protected override object HandleUnrecognized(object state, Instruction instruction)
        {
            this.statements.Add(new ILStatement(instruction).WithInstruction(instruction));

            return null;
        }

        protected override object HandleBinaryOperator(object state, Instruction instruction, BinaryOperator @operator)
        {
            var left = this.expressions.Pop();
            var right = this.expressions.Pop();

            this.expressions.Push(new BinaryExpression(left.Type, @operator, left, right));

            return null;
        }

        protected override object HandleRet(object state, Instruction instruction)
        {
            if (this.AnalyzedMethod.ReturnType == typeof(void))
            {
                this.statements.Add(new ReturnStatement().WithInstruction(instruction));
            }
            else
            {
                var value = this.expressions.Pop();
                this.statements.Add(new ReturnStatement(value).WithInstruction(instruction));
            }

            return null;
        }

        protected override object BeforeInstruction(object state, Instruction instruction)
        {
            if (this.labels.Contains(instruction.Offset))
            {
                this.labels.Remove(instruction.Offset);

                this.statements.Add(new LabelStatement(instruction.Offset).WithInstruction(instruction));
            }

            return base.BeforeInstruction(state, instruction);
        }
    }

    public class GotoStatement : Statement
    {
        public int Offset { get; private set; }

        public GotoStatement(int offset)
        {
            this.Offset = offset;
        }
    }

    public class MethodDeclaration : AstNode
    {
        public MethodInfo Method { get; private set; }
        public BlockStatement Body { get; private set; }

        public MethodDeclaration(MethodInfo method, BlockStatement body)
        {
            this.Method = method;
            this.Body = body;
        }
    }

    public abstract class AstNode : IAnnotable
    {
        private readonly List<object> annotations;

        protected AstNode()
        {
            this.annotations = new List<object>();
        }

        public void Annonate(object annotation)
        {
            this.annotations.Add(annotation);
        }

        public IEnumerable<object> Annotations
        {
            get { return this.annotations; }
        }

        public void RemoveAnnotation(object annotation)
        {
            this.annotations.Remove(annotation);
        }

        public TAnnotation Annotation<TAnnotation>()
        {
            return this.annotations.OfType<TAnnotation>().SingleOrDefault();
        }
    }

    public class LabelStatement : Statement
    {
        public int Offset { get; private set; }

        public LabelStatement(int offset)
        {
            this.Offset = offset;
        }
    }

    public class ReturnStatement : Statement
    {
        public Expression Value { get; private set; }

        public ReturnStatement(Expression value)
        {
            this.Value = value;
        }

        public ReturnStatement()
        {
            this.Value = null;
        }
    }

    public class BinaryExpression : Expression
    {
        public BinaryOperator Operator { get; private set; }
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public BinaryExpression(Type type, BinaryOperator @operator, Expression left, Expression right)
            : base(type)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
        }
    }

    public class ILStatement : Statement
    {
        public Instruction Instruction { get; private set; }

        public ILStatement(Instruction instruction)
        {
            this.Instruction = instruction;
        }
    }

    public class ConditionalBranchStatement : Statement
    {
        public Expression Condition { get; private set; }
        public int Offset { get; private set; }

        public ConditionalBranchStatement(Expression condition, int offset)
        {
            this.Condition = condition;
            this.Offset = offset;
        }
    }

    public class LoadVariableExpression : Expression
    {
        public LocalVariableInfo Variable { get; private set; }

        public LoadVariableExpression(LocalVariableInfo variable)
            : base(variable.LocalType)
        {
            this.Variable = variable;
        }
    }

    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; private set; }

        public ExpressionStatement(Expression expression)
        {
            this.Expression = expression;
        }
    }

    public class StoreLocal : Statement
    {
        public LocalVariableInfo Variable { get; private set; }
        public Expression Value { get; private set; }

        public StoreLocal(LocalVariableInfo variable, Expression value)
        {
            this.Variable = variable;
            this.Value = value;
        }
    }

    public abstract class Statement : AstNode
    {
        public Statement WithInstruction(Instruction instruction)
        {
            this.Annonate(instruction);

            return this;
        }
    }

    public abstract class Expression : AstNode
    {
        public Type Type { get; private set; }

        protected Expression(Type type)
        {
            this.Type = type;
        }
    }

    public class ConstantExpression : Expression
    {
        public object Value { get; private set; }

        public ConstantExpression(Type type, object value)
            : base(type)
        {
            this.Value = value;
        }
    }

    public class MethodCallExpression : Expression
    {
        public MethodInfo Method { get; private set; }
        public Expression Target { get; private set; }
        public ICollection<Expression> Parameters { get; private set; }

        public MethodCallExpression(Type returnType, MethodInfo method, Expression target, ICollection<Expression> parameters)
            : base(returnType)
        {
            this.Method = method;
            this.Target = target;
            this.Parameters = parameters;
        }
    }

    public interface IRewritable<T>
        where T : AstNode
    {
        T Rewrite(AstRewriterBase visitor);
    }

    public abstract class AstVisitorBase
    {
        public void Visit(object value)
        {
            ((dynamic)this).On((dynamic)value);
        }

        public virtual void On(IEnumerable<Statement> statements)
        {
            foreach (var statement in statements)
            {
                this.Visit(statement);
            }
        }

        public virtual void On(Expression expression)
        {
            throw new InvalidOperationException("Unhandled expression " + expression.GetType().Name);
        }

        public virtual void On(Statement statement)
        {
            throw new InvalidOperationException("Unhandled statement " + statement.GetType().Name);
        }
    }

    public abstract class AstRewriterBase
    {
        public T Rewrite<T>(T value)
            where T : AstNode
        {
            if (value == null)
            {
                return null;
            }

            return ((dynamic)this).On((dynamic)value);
        }

        public MethodDeclaration On(MethodDeclaration method)
        {
            return new MethodDeclaration(method.Method, this.Rewrite(method.Body)).CopyAnnotations(method);
        }       

        public virtual T On<T>(T node)
            where T : AstNode
        {
            var rewritable = node as IRewritable<T>;

            if (rewritable != null)
            {
                return rewritable.Rewrite(this).CopyAnnotations(node);   
            }

            //throw new InvalidOperationException("Unhandled node " + node.GetType().Name);
            return node;
        }
    }
}
