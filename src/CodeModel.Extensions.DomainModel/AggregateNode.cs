using System;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel
{
    public class AggregateNode : EntityNode
    {
        public AggregateNode(Type type) : base(type)
        {
        }
    }
}