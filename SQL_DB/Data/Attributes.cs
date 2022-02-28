using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SQL_DB.Data
{

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class czaIgnoreAttribute : Attribute { }



	public class czaTableAttribute : Attribute { }




	public class czaTable_ValueAttribute : czaTableAttribute
	{
		protected czaTable_ValueAttribute(string cxValue)
		{
			Value = cxValue;
		}
		public string Value { get; private set; }
	}



	//[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	//public class czaTable_ValuesAttribute : czaTableAttribute
	//{
	//	protected czaTable_ValuesAttribute(string[] cxValues)
	//	{
	//		Values = cxValues;
	//	}
	//	public string[] Values { get; private set; }
	//}




	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class czaNullAttribute : czaTable_ValueAttribute
	{
		public czaNullAttribute() : base("NULL")
		{

		}
	}

	[AttributeUsage(AttributeTargets.Property,AllowMultiple =false)]
	public class czaNotNullAttribute : czaTable_ValueAttribute
	{
        public czaNotNullAttribute():base("NOT NULL")
        {

        }
	}


	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class czaDefaultAttribute : czaTable_ValueAttribute
	{
		public czaDefaultAttribute(object cxValue) : base(cfGet_Value(cxValue))
		{

		}

		private static string cfGet_Value(object cxValue)
		{
			string cxFormat = "DEFAULT {1}{0}{1}";
			string cxParam = string.Empty;
			if (cxValue.GetType()==typeof(string)) cxParam= "'";
			return string.Format(cxFormat, cxValue.ToString(), cxParam);
		}

	}






	[AttributeUsage(AttributeTargets.Property ,AllowMultiple = false)]
	public class czaUniqueOneAttribute : czaTable_ValueAttribute
	{
		protected const string ccUniq = "UNIQUE";
		public czaUniqueOneAttribute() : base(ccUniq)
		{

		}
        protected czaUniqueOneAttribute(string cxValue) : base(cxValue)
        {
        }
    }


	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class czaUniqueManyAttribute : czaUniqueOneAttribute
	{
		public czaUniqueManyAttribute(params string[] cxColumns) : base(string.Format("{0} ({1})", ccUniq, string.Join(",", cxColumns)))
		{
			if (cxColumns.Length<2) throw new Exception("czaUniqueAttribute for class cxColumns.Length<2 have no sence. Use on field");
		}
	}








	[AttributeUsage(AttributeTargets.Property , AllowMultiple = false)]
	public class czaCheckOneAttribute : czaTable_ValueAttribute
	{
		const string format = "CHECK ({0})";
		public czaCheckOneAttribute(string cxField_Exp_Format, [CallerMemberName] string cxPropertyName = null) : this(string.Format(cxField_Exp_Format, cxPropertyName)) //base(string.Format(format,string.Format(cxField_Exp_Format, cxPropertyName)))
		{

		}
		protected czaCheckOneAttribute(string cxValue) : base(string.Format(format, cxValue))
		{

		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class czaCheckManyAttribute : czaCheckOneAttribute
	{
		public czaCheckManyAttribute(string cxField_Exp_Format, params string[] cxColumns) : base(string.Format(cxField_Exp_Format, cxColumns)) //base(string.Format(format, string.Format(cxField_Exp_Format, cxColumns)))
		{

		}
	}







	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class czaForeignKeyAttribute : czaTable_ValueAttribute
	{
		public czaForeignKeyAttribute(string cxReference_Table, string cxReference_Column) : base(string.Format("REFERENCES {0}({1})", cxReference_Table, cxReference_Column))
		{

		}
	}









	[AttributeUsage(AttributeTargets.Property)]
	public class czaPrimaryKeyAttribute : czaTableAttribute
	{
		public bool AutoIncrement = false;

		readonly bool _New_Id_Not_Always_New;
		readonly int _ai_type;//0=no auto, 1 - standart, 2 - IDENTITY(x,y)
		readonly int _Start;
		readonly int _Inc;


		private czaPrimaryKeyAttribute(bool cxNew_Id_Not_Always_New) 
		{
			_New_Id_Not_Always_New=cxNew_Id_Not_Always_New;
		}

		public czaPrimaryKeyAttribute(bool cxAutoIncrement, bool cxNew_Id_Not_Always_New = false) : this(cxNew_Id_Not_Always_New)
		{
			if (cxAutoIncrement) { _Start=1; _Inc=1; _ai_type=1; }
			else _ai_type=0;
			AutoIncrement=cxAutoIncrement;
		}

		public czaPrimaryKeyAttribute(int cxStart, int cxInc) : this(false)
		{
			if (cxStart==1 && cxInc==1) throw new Exception("IDENTITY(1,1) This is standart of AutoIncrement=true use it");
			_ai_type=2;
			_Start=cxStart;
			_Inc=cxInc;
		}



		public string cfGet_Value(ceTable_SQL_Type cxTable_Type)
		{
			//By default in sql PRIMARY KEY cannot be Null and Unique always
			List<string> cxParams = new List<string>() { "PRIMARY KEY" };

			string cxAutoInc = cfGet_AutoInc(cxTable_Type==ceTable_SQL_Type.MSQL);
			if (!string.IsNullOrWhiteSpace(cxAutoInc)) cxParams.Add(cxAutoInc);

			if (_New_Id_Not_Always_New)
			{
				if (cxTable_Type==ceTable_SQL_Type.SQLite) cxParams.Add("NOT NULL");  //cxParams.Add("UNIQUE"); ????? have no sence
				else throw new Exception("czaPrimaryKeyAttribute _New_Id_Not_Always_New not used in "+ cxTable_Type.ToString());
			}

			return string.Join(" ", cxParams);
		}

		private string cfGet_AutoInc(bool cxIDENTITY)
		{
			if (_ai_type==0) return null;
			if (cxIDENTITY)
			{
				string cxFormat = "IDENTITY({0},{1})";
				return string.Format(cxFormat, _Start, _Inc);
			}
			else return "AUTOINCREMENT";
		}


	}



}
