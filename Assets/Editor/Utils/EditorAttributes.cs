using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor.Utils
{
    public class ReadOnlyDictionaryKeysAttribute : Attribute { }

    public class ReadOnlyDictionaryKeysAttributeProcessor<T1, T2> : OdinAttributeProcessor<EditableKeyValuePair<T1, T2>>
    {
        public override bool CanProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member)
        {
            return parentProperty.Attributes.HasAttribute<ReadOnlyDictionaryKeysAttribute>();
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Key")
            {
                attributes.Add(new ReadOnlyAttribute());
            }
        }
    }
}