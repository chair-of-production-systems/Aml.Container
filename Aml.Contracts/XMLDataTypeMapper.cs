using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Aml.Contracts
{
	public static class XMLDataTypeMapper
	{
		private static readonly Dictionary<object, string> Mappings = new Dictionary<object, string>
		{
			{typeof (string), "xs:string"},
			{typeof (byte), "xs:unsignedByte"},
			{typeof (ushort), "xs:unsignedShort"},
			{typeof (uint), "xs:unsignedInt"},
			{typeof (ulong), "xs:unsignedLong"},
			{typeof (bool), "xs:boolean"},
			{typeof (short), "xs:short"},
			{typeof (int), "xs:int"},
			{typeof (long), "xs:long"},
			{typeof (float), "xs:float"},
			{typeof (double), "xs:double"},
			{typeof (decimal), "xs:decimal"},
			{typeof (DateTime), "xs:dateTime"},
			{typeof (TimeSpan), "xs:duration"},
			{typeof (Color), "xs:color"},
			{typeof (Uri), "xs:anyURI"},
			{typeof (string[]), "xs:ENTITIES"}
		};

		public static string GetXmlDataType(object dataTypeOrValue)
		{
			if (dataTypeOrValue == null) throw new ArgumentNullException(nameof(dataTypeOrValue));
			var type = dataTypeOrValue as Type ?? dataTypeOrValue.GetType();
			Debug.Assert(type != null);

			Mappings.TryGetValue(type, out var item);
			if (string.IsNullOrEmpty(item))
			{
				item = type.AssemblyQualifiedName;
			}
			return item;
		}
	}
}
