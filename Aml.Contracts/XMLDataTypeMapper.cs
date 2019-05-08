using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Aml.Contracts
{
	public static class XMLDataTypeMapper
	{
		public const string StringTypeName = "xs:string" ;
		public const string ByteTypeName = "xs:unsignedByte";
		public const string UshortTypeName = "xs:unsignedShort";
		public const string UintTypeName = "xs:unsignedInt";
		public const string UlongTypeName = "xs:unsignedLong";
		public const string BooleanTypeName = "xs:boolean";
		public const string ShortTypeName = "xs:short";
		public const string IntTypeName = "xs:int";
		public const string LongTypeName = "xs:long";
		public const string FloatTypeName = "xs:float";
		public const string DoubleTypeName = "xs:double";
		public const string DecimalTypeName = "xs:decimal";
		public const string DateTimeTypeName = "xs:dateTime";
		public const string TimeSpanTypeName = "xs:duration";
		public const string ColorTypeName = "xs:color";
		public const string UriTypeName = "xs:anyURI";
		public const string StringArrayTypeName = "xs:ENTITIES";

		private static readonly Dictionary<object, string> Mappings = new Dictionary<object, string>
		{
			{typeof (string), StringTypeName},
			{typeof (byte), ByteTypeName},
			{typeof (ushort), UshortTypeName},
			{typeof (uint), UintTypeName},
			{typeof (ulong), UlongTypeName},
			{typeof (bool), BooleanTypeName},
			{typeof (short), ShortTypeName},
			{typeof (int), IntTypeName},
			{typeof (long), LongTypeName},
			{typeof (float), FloatTypeName},
			{typeof (double), DoubleTypeName},
			{typeof (decimal), DecimalTypeName},
			{typeof (DateTime), DateTimeTypeName},
			{typeof (TimeSpan), TimeSpanTypeName},
			{typeof (Color), ColorTypeName},
			{typeof (Uri), UriTypeName},
			{typeof (string[]), StringArrayTypeName}
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
