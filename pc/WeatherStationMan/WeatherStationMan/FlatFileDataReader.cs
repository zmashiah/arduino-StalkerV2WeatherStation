//===============================================================================
// Copyright 2006© Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
//Version 1.0.0.1 Added GetSchemaTable support for Loading DataTables from FlatFileDataReaders


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace WeatherDataAnalyzer
{
   
    abstract class FlatFileDataReader : IDataReader 
    {

        public enum LineResult
        {
            READ,
            SKIP,
            END_OF_FILE
        }

        TextReader reader;
        
        protected long LineNumber = 0;
        
        public FlatFileDataReader(string FileName)
            :this(new StreamReader(FileName, System.Text.Encoding.ASCII))
        {
            
        }

        public FlatFileDataReader(TextReader Reader)
        {
            this.reader = Reader;
        }

        public virtual bool Read()
        {
            LineResult result;
            string currentLine;
            do
            {
                currentLine = reader.ReadLine();
                if (currentLine == null)
                {
                    LineNumber = -1L;
                    return false;
                }
                LineNumber += 1L;
                result = ReadLine(ref currentLine);
            } while (result == LineResult.SKIP);
            
            if (result == LineResult.READ)
                return true;

            if (result == LineResult.END_OF_FILE)
                return false;

            throw new InvalidOperationException("Unexpected code path");

        }

        /**
         * 
         * Implement this to receive each line in the file;
         * After each row is read from the file, this method
         * will be fired.
         * 
         * Return END_OF_FILE if this is the trailer record
         * or to otherwise single an End-of-file.
         * 
         * Return SKIP if this row is just ignored.
         * 
         * Return READ if this is a valid row
         * 
         * Optionally alter the line (it's passed as a ref).
         * 
         * Throw an exception if the line is invalid.
         * 
         */
        public abstract LineResult ReadLine(ref string line);

        /**
         * Readers will call this to get a value
         * Return value should be the proper .NET type for the field         * 
         */
        public abstract object GetValue(int i);

        /**
         * These methods return metadata about the fields
         * */
        public abstract Type GetFieldType(int i);
        public abstract string GetName(int i);
        public abstract int FieldCount
        {
            get;
        }

        #region IDataRecord Typed Accessor Members


        /**
         * All of these methods just call GetValue and attemt to cast to the requested type
         */
          
        T GetFieldValue<T>(int i)
        {

            object o = GetValue(i);
            if (o == DBNull.Value && typeof(T).IsValueType)
            {
                throw new InvalidOperationException("Value is Null");
            }
            return (T)o;
        }

        public virtual bool GetBoolean(int i)
        {
            return GetFieldValue<bool>(i);
        }

        public virtual byte GetByte(int i)
        {
            return GetFieldValue<byte>(i);
        }

        public virtual char GetChar(int i)
        {
            return GetFieldValue<char>(i);
        }
         
        public virtual DateTime GetDateTime(int i)
        {
            return GetFieldValue<DateTime>(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return GetFieldValue<decimal>(i);
        }

        public virtual double GetDouble(int i)
        {
            return GetFieldValue<double>(i);
        }

        public virtual float GetFloat(int i)
        {
            return GetFieldValue<float>(i);
        }

        public virtual Guid GetGuid(int i)
        {
            return GetFieldValue<Guid>(i);
        }

        public virtual short GetInt16(int i)
        {
            return GetFieldValue<short>(i);
        }

        public virtual int GetInt32(int i)
        {
            return GetFieldValue<int>(i);
        }

        public virtual long GetInt64(int i)
        {
            return GetFieldValue<long>(i);
        }

        public virtual string GetString(int i)
        {
            return GetFieldValue<string>(i);
        }


        #endregion


        #region Virtual Methods

        /**
         * All these methods have default implementations
         * But you may want to override them for performance
         * or convenience
         * 
         **/

        public virtual void Close()
        {
            reader.Close();
            reader = null;
        }

        public virtual int Depth
        {
            get { return 1; }
        }



        public virtual bool IsClosed
        {
            get { return reader == null; }
        }
        public virtual int GetOrdinal(string name)
        {
            for (int i = 0; i < FieldCount; i++)
            {
                if (GetName(i) == name)
                {
                    return i;
                }
            }
            for (int i = 0; i < FieldCount; i++)
            {
                if (GetName(i).ToUpper() == name.ToUpper())
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual string GetDataTypeName(int i)
        {
            return GetFieldType(i).Name;
        }


        public virtual bool NextResult()
        {
            return false;
        }

        public virtual int GetValues(object[] values)
        {
            int fields = 0;
            int totalFields = FieldCount;
            for (int i = 0; i < values.Length && i < totalFields; i++)
            {
                fields += 1;
                values[i] = GetValue(i);
            }
            return fields;
        }

        public virtual bool IsDBNull(int i)
        {
            return (GetValue(i) == DBNull.Value);
        }

        public virtual object this[string name]
        {
            get { return GetValue(GetOrdinal(name)); }
        }

        public virtual object this[int i]
        {
            get { return GetValue(i); }
        }

        #endregion

        #region GetSchemaTable


        const string shemaTableSchema = @"<?xml version=""1.0"" standalone=""yes""?>
<xs:schema id=""NewDataSet"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""NewDataSet"" msdata:IsDataSet=""true"" msdata:MainDataTable=""SchemaTable"" msdata:Locale="""">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""SchemaTable"" msdata:Locale="""" msdata:MinimumCapacity=""1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""ColumnOrdinal"" msdata:ReadOnly=""true"" type=""xs:int"" default=""0"" minOccurs=""0"" />
              <xs:element name=""ColumnSize"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""NumericPrecision"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""NumericScale"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""IsUnique"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsKey"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""BaseServerName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseCatalogName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseSchemaName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseTableName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""AllowDBNull"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""IsAliased"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsExpression"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsIdentity"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsAutoIncrement"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsRowVersion"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsHidden"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsLong"" msdata:ReadOnly=""true"" type=""xs:boolean"" default=""false"" minOccurs=""0"" />
              <xs:element name=""IsReadOnly"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderSpecificDataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataTypeName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionDatabase"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionOwningSchema"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""UdtAssemblyQualifiedName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""NonVersionedProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        public virtual DataTable GetSchemaTable()
        {
          DataSet s = new DataSet();
          s.ReadXmlSchema(new StringReader(shemaTableSchema));
          DataTable t = s.Tables[0];
          for (int i = 0; i < this.FieldCount; i++)
          {
            DataRow row = t.NewRow();
            row["ColumnName"] = this.GetName(i);
            row["ColumnOrdinal"] = i;
            row["DataType"] = this.GetFieldType(i);
            row["DataTypeName"] = this.GetDataTypeName(i);
            row["ColumnSize"] = -1;
            t.Rows.Add(row);
          }
          return t;

        }
        #endregion

        #region Not Implemented Methods

        public virtual IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }


        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public virtual int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }
        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Close();
        }

        #endregion

        public class ParseException : Exception
        {
            
            public ParseException(string Line, long LineNumber, string Message) 
                : base(string.Format("Error {0}, on line {1}, text [{2}]", Message, Line, LineNumber))
            {
                
            }
        }
    }
}
