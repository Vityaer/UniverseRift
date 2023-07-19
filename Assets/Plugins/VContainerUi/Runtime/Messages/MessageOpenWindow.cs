using System;
using VContainerUi.Interfaces;
using VContainerUi.Model;

namespace VContainerUi.Messages
{
	public readonly struct MessageOpenWindow : IUiMessage
	{
		public readonly Type Type;
		public readonly string Name;
		public readonly OpenType OpenType;

		public UiScope UiScope { get; }

		public MessageOpenWindow(Type type, UiScope scope = UiScope.Local, OpenType openType = OpenType.Exclusive)
		{
			Type = type;
			Name = null;
			UiScope = scope;
			OpenType = openType;
		}

		public MessageOpenWindow(string name, UiScope scope = UiScope.Local, OpenType openType = OpenType.Exclusive)
		{
			Type = null;
			Name = name;
			UiScope = scope;
			OpenType = openType;
		}
	}
}