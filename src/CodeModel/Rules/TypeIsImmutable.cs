﻿using System.Linq;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Rules
{
    public class TypeIsImmutable : INodeRule
    {
        public const string NonPrivatePropertySetter = "NonPrivatePropertySetter";
        public const string SettingPropertyOutsideOfConstructor = "SetPropertyOutsideOfCtor";
        public const string SettingFieldOutsideOfConstructor = "SetFieldOutsideOfCtor";
        public const string WritableField = "WritableField";

        public void Verify(VerificationContext context, Node node)
        {
            var typeNode = (TypeNode)node;

            VerifyNotSettingField(context, typeNode);
            VerifyNotSettingProperty(context, typeNode);
            VerifyNoNotPrivatePropertySetters(context, typeNode);
            VerifyNoWritableFields(context, typeNode);
        }

        public bool IsApplicableTo(Node node)
        {
            return node is TypeNode;//TODO: check if immutable (convention/annotation?)
        }

        private void VerifyNoWritableFields(VerificationContext context, TypeNode typeNode)
        {
            var violatingFields = from field in typeNode.InboundFrom<FieldNode, ContainedInLink>()
                                  where !field.Field.IsInitOnly
                                  select field;

            foreach (var field in violatingFields)
            {
                context.RecordViolation(this, field, WritableField, null);
            }
        }

        private void VerifyNoNotPrivatePropertySetters(VerificationContext context, TypeNode typeNode)
        {
            var violatingProperties = from property in typeNode.InboundFrom<PropertyNode, ContainedInLink>()
                                      where property.Property.CanWrite && !property.Property.SetMethod.IsPrivate
                                      select property;

            foreach (var property in violatingProperties)
            {
                context.RecordViolation(this, property, NonPrivatePropertySetter, null);
            }
        }

        private void VerifyNotSettingProperty(VerificationContext context, TypeNode typeNode)
        {
            var violatingMethods = from method in typeNode.InboundFrom<MethodNode, ContainedInLink>()
                                   where method.OutboundLinks.OfType<SetPropertyLink>().Any()
                                   select method;

            foreach (var method in violatingMethods)
            {
                context.RecordViolation(this, method, SettingPropertyOutsideOfConstructor, null);
            }
        }

        private void VerifyNotSettingField(VerificationContext context, TypeNode typeNode)
        {
            var violatingMethods = from method in typeNode.InboundFrom<MethodNode, ContainedInLink>()
                                   where method.OutboundLinks.OfType<SetFieldLink>().Any()
                                   select method;

            foreach (var method in violatingMethods)
            {
                context.RecordViolation(this, method, SettingFieldOutsideOfConstructor, null);
            }
        }
    }
}