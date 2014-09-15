using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Reflection;

namespace CodeModel.Ast
{    
    public class RewriteIf : AstRewriterBase
    {
        public BlockStatement On(BlockStatement block)
        {            
            var statementByOffset = block.Statements.GroupBy(x => x.Annotation<Instruction>().Offset).ToDictionary(x => x.Key, x => x.First());

            var statements = block.Statements.ToList();

            var resultStatements = new List<Statement>();

            for (int i = 0; i < statements.Count; i++)
            {
                var condBranch = statements[i] as ConditionalBranchStatement;

                if (condBranch == null)
                {
                    resultStatements.Add(statements[i]);
                    continue;                    
                }

                var targetStatement = statementByOffset[condBranch.Offset];

                if (condBranch.Offset <= condBranch.Annotation<Instruction>().Offset)
                {
                    resultStatements.Add(statements[i]);
                    continue;
                }

                var trueBlock = new List<Statement>(statements.Range(i + 2, statements.IndexOf(targetStatement) - 1));

                var @if = new IfStatement(condBranch.Condition, new BlockStatement(trueBlock).WithInstruction(trueBlock[0].Annotation<Instruction>())).WithInstruction(condBranch.Annotation<Instruction>());

                resultStatements.Add(this.Rewrite(@if));

                i = statements.IndexOf(targetStatement);
            }

            return new BlockStatement(resultStatements);
        }
    }

    public class RewriteIfElse : AstRewriterBase
    {
        public BlockStatement On(BlockStatement block)
        {
            var statementByOffset = block.Statements.GroupBy(x => x.Annotation<Instruction>().Offset).ToDictionary(x => x.Key, x => x.First());

            var statements = block.Statements.ToList();

            var resultStatements = new List<Statement>();

            for (int i = 0; i < statements.Count; i++)
            {
                var ifStatement = statements[i] as IfStatement;

                if (ifStatement == null)
                {
                    resultStatements.Add(this.Rewrite(statements[i]));
                    continue;
                }

                var gotoAtEnd = ((BlockStatement) ifStatement.TrueStatement).Statements.Last() as GotoStatement;

                if (gotoAtEnd == null || gotoAtEnd.Offset <= gotoAtEnd.Annotation<Instruction>().Offset)
                {
                    resultStatements.Add(this.Rewrite(statements[i]));
                    continue;                    
                }

                var targetStatement = statementByOffset[gotoAtEnd.Offset];

                var elseBlock = new List<Statement>(statements.Range(i + 1, statements.IndexOf(targetStatement) - 1));
                var trueBlock = new List<Statement>(((BlockStatement) ifStatement.TrueStatement).Statements.Take(((BlockStatement) ifStatement.TrueStatement).Statements.Count() - 1));


                var ifElse = new IfStatement(ifStatement.Condition, new BlockStatement(trueBlock), new BlockStatement(elseBlock))
                    .CopyAnnotations(ifStatement);

                resultStatements.Add(this.Rewrite(ifElse));

                i = statements.IndexOf(targetStatement);
            }

            return new BlockStatement(resultStatements);
        }
    }

    public class IfStatement : Statement, IRewritable<IfStatement>
    {
        public Expression Condition { get; private set; }
        public Statement TrueStatement { get; private set; }

        public Statement ElseStatement { get; private set; }

        public IfStatement(Expression condition, Statement trueStatement)
        {
            this.Condition = condition;
            this.TrueStatement = trueStatement;
        }

        public IfStatement(Expression condition, Statement trueStatement, Statement elseStatement)
            : this(condition, trueStatement)
        {
            this.ElseStatement = elseStatement;
        }

        public IfStatement Rewrite(AstRewriterBase visitor)
        {
            return new IfStatement(visitor.Rewrite(this.Condition), visitor.Rewrite(this.TrueStatement), visitor.Rewrite(this.ElseStatement));
        }

        public IfStatement WithElseBlock(Statement statement)
        {
            return new IfStatement(this.Condition, this.TrueStatement, statement);
        }
    }

    public class BlockStatement : Statement
    {
        public IEnumerable<Statement> Statements { get; private set; }

        public BlockStatement(IEnumerable<Statement> statements)
        {
            this.Statements = statements;
        }
    }
}
