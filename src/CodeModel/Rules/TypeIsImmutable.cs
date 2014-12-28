using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Annotations;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    public class TypeIsImmutable : INodeRule
    {
        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            var typeNode = (TypeNode)node;

            return VerifyNotSettingField(context, typeNode)
                .Union(VerifyNotSettingProperty(context, typeNode))
                .Union(VerifyNoNotPrivatePropertySetters(context, typeNode))
                .Union(VerifyNoWritableFields(context, typeNode))
                ;
        }

        public bool IsApplicableTo(Node node)
        {
            return node.HasAnnotation<Immutable>();
        }

        private IEnumerable<Violation> VerifyNoWritableFields(VerificationContext context, TypeNode typeNode)
        {
            var violatingFields = from field in typeNode.InboundFrom<FieldNode, ContainedInLink>()
                                  where !field.Field.IsInitOnly
                                  select field;

            foreach (var field in violatingFields)
            {
               yield return new ImmutableTypeHasWritableFieldViolation(typeNode, field);                
            }
        }

        private IEnumerable<Violation> VerifyNoNotPrivatePropertySetters(VerificationContext context, TypeNode typeNode)
        {
            var violatingProperties = from property in typeNode.InboundFrom<PropertyNode, ContainedInLink>()
                                      where property.Property.CanWrite && !property.Property.SetMethod.IsPrivate
                                      select property;

            foreach (var property in violatingProperties)
            {
                yield return new ImmutableTypeHasNonPrivateSetterViolation(typeNode, property);                
            }
        }

        private IEnumerable<Violation> VerifyNotSettingProperty(VerificationContext context, TypeNode typeNode)
        {
            var violatingMethods = from method in typeNode.InboundFrom<MethodNode, ContainedInLink>()
                                   where method.OutboundLinks.OfType<SetPropertyLink>().Any()
                                   select method;

            foreach (var method in violatingMethods)
            {
                yield return new ImmutableTypeSetsPropertyOutsideOfConstructorViolation(typeNode, method);                
            }
        }

        private IEnumerable<Violation> VerifyNotSettingField(VerificationContext context, TypeNode typeNode)
        {
            var violatingMethods = from method in typeNode.InboundFrom<MethodNode, ContainedInLink>()
                                   where method.OutboundLinks.OfType<SetFieldLink>().Any()
                                   select method;

            foreach (var method in violatingMethods)
            {
                yield return new ImmutableTypeSetsFieldOutsideOfConstructorViolation(typeNode, method);                
            }
        }
    }
}