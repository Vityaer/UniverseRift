//-----------------------------------------------------------------------
// <copyright file="OdinAddressableReflection.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;

namespace Sirenix.OdinInspector.Modules.Addressables.Editor.Internal
{
	internal static class OdinAddressableReflection
	{
		public static FieldInfo AddressableAssetEntry_mGUID_Field;

		static OdinAddressableReflection()
		{
#if UNITY_EDITOR
			AddressableAssetEntry_mGUID_Field = typeof(UnityEditor.AddressableAssets.Settings.AddressableAssetEntry).GetField("m_GUID", BindingFlags.Instance | BindingFlags.NonPublic);
#endif
		}

		internal static void EnsureConstructed() { }
	}
}