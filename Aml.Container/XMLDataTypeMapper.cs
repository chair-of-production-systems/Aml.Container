using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Aml.Container
{
	public static class XMLDataTypeMapper
	{
		private static readonly Dictionary<object, string> Mappings = new Dictionary<object, string>
		{
			{typeof (string), "xs:string"},
			{typeof (Byte), "xs:unsignedByte"},
			{typeof (UInt16), "xs:unsignedShort"},
			{typeof (UInt32), "xs:unsignedInt"},
			{typeof (UInt64), "xs:unsignedLong"},
			{typeof (bool), "xs:boolean"},
			{typeof (Int16), "xs:short"},
			{typeof (Int32), "xs:int"},
			{typeof (Int64), "xs:long"},
			{typeof (Single), "xs:float"},
			{typeof (Double), "xs:double"},
			{typeof (Decimal), "xs:decimal"},
			{typeof (DateTime), "xs:dateTime"},
			{typeof (TimeSpan), "xs:duration"},
			{typeof (Color), "xs:color"},
			{typeof (Uri), "xs:anyURI"},
			{typeof (String[]), "xs:ENTITIES"}
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
