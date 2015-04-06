using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Ast
{
    public class InlineStoreLoadVariable : AstRewriterBase
    {
        public BlockStatement Rewrite(BlockStatement block)
        {
            var result = new List<Statement>();

            var statements = block.Statements.ToList();

            Statement previousStatement = null;

            foreach (var statement in statements)
            {
                if (previousStatement == null)
                {
                    previousStatement = statement;
                }               
                else
                {
                    var previousStore = previousStatement as StoreLocal;
                    //var currentLoad = statement as IHaveSingleExpression;

                    result.Add(previousStatement);
                    previousStatement = statement;
                }
            }

            if (previousStatement != null)
            {
                statements.Add(previousStatement);
            }

            return new BlockStatement(result);
        }
    }
}