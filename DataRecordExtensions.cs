using System;
using System.Data;

namespace AspNet.Identity.MySQL
{
    public static class DataRecordExtensions
    {
        public static bool IsDBNull(this IDataRecord record, String column)
        {
            return record.IsDBNull(record.GetOrdinal(column));
        }

        public static Object GetValue(this IDataRecord record, String column)
        {
            return record.GetValue(record.GetOrdinal(column));
        }

        public static String GetString(this IDataRecord record, String column)
        {
            return record.GetString(record.GetOrdinal(column));
        }

        public static Int16 GetInt16(this IDataRecord record, String column)
        {
            return record.GetInt16(record.GetOrdinal(column));
        }

        public static Int32 GetInt32(this IDataRecord record, String column)
        {
            return record.GetInt32(record.GetOrdinal(column));
        }

        public static Int64 GetInt64(this IDataRecord record, String column)
        {
            return record.GetInt64(record.GetOrdinal(column));
        }

        public static Decimal GetDecimal(this IDataRecord record, String column)
        {
            return record.GetDecimal(record.GetOrdinal(column));
        }

        // overload to match IDataRecord methods
        public static Single GetFloat(this IDataRecord record, String column)
        {
            return GetSingle(record, column);
        }

        // the proper name for Float in .NET
        public static Single GetSingle(this IDataRecord record, String column)
        {
            return record.GetFloat(record.GetOrdinal(column));
        }

        public static Double GetDouble(this IDataRecord record, String column)
        {
            return record.GetDouble(record.GetOrdinal(column));
        }

        public static Boolean GetBoolean(this IDataRecord record, String column)
        {
            return record.GetBoolean(record.GetOrdinal(column));
        }

        public static Char GetChar(this IDataRecord record, String column)
        {
            return record.GetChar(record.GetOrdinal(column));
        }

        public static Byte GetByte(this IDataRecord record, String column)
        {
            return record.GetByte(record.GetOrdinal(column));
        }

        public static long GetChars(this IDataRecord record, String column, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return record.GetChars(record.GetOrdinal(column), fieldOffset, buffer, bufferOffset, length);
        }

        public static long GetBytes(this IDataRecord record, String name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            return record.GetBytes(record.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        public static DateTime GetDateTime(this IDataRecord record, String column)
        {
            return record.GetDateTime(record.GetOrdinal(column));
        }

        public static Guid GetGuid(this IDataRecord record, String column)
        {
            return record.GetGuid(record.GetOrdinal(column));
        }

        public static IDataReader GetData(this IDataRecord record, String column)
        {
            return record.GetData(record.GetOrdinal(column));
        }

        public static String GetDataTypeName(this IDataRecord record, String column)
        {
            return record.GetDataTypeName(record.GetOrdinal(column));
        }

        public static Type GetFieldType(this IDataRecord record, String column)
        {
            return record.GetFieldType(record.GetOrdinal(column));
        }

        #region special signed/unsigned cases not available in IDataRecord/IDataRecord

        public static SByte GetSByte(this IDataRecord record, String column)
        {
            return record.GetSByte(record.GetOrdinal(column));
        }

        public static SByte GetSByte(this IDataRecord record, int index)
        {
            var value = record.GetValue(index);
            if (value is SByte)
            {
                return (SByte) value;
            }

            return Convert.ToSByte(value);
        }

        public static UInt16 GetUInt16(this IDataRecord record, String column)
        {
            return record.GetUInt16(record.GetOrdinal(column));
        }

        public static UInt16 GetUInt16(this IDataRecord record, int index)
        {
            var value = record.GetValue(index);
            if (value is UInt16)
            {
                return (UInt16) value;
            }

            return Convert.ToUInt16(value);
        }

        public static UInt32 GetUInt32(this IDataRecord record, String column)
        {
            return record.GetUInt32(record.GetOrdinal(column));
        }

        public static UInt32 GetUInt32(this IDataRecord record, int index)
        {
            var value = record.GetValue(index);
            if (value is UInt32)
            {
                return (UInt32) value;
            }

            return Convert.ToUInt32(value);
        }

        public static UInt64 GetUInt64(this IDataRecord record, String column)
        {
            return record.GetUInt64(record.GetOrdinal(column));
        }

        public static UInt64 GetUInt64(this IDataRecord record, int index)
        {
            var value = record.GetValue(index);
            if (value is UInt64)
            {
                return (UInt64) value;
            }

            return Convert.ToUInt64(value);
        }

        #endregion

        #region altogether new methods not part of IDataRecord

        // simpler to just catch exception then check the entire field count
        public static bool HasColumn(this IDataRecord record, String columnName)
        {
            try
            {
                record.GetOrdinal(columnName);
                return true;
            }
            catch (IndexOutOfRangeException)
                // we don't want to catch ALL exceptions, just the one that indicates failure
            {
                return false;
            }
        }

        // useful for enums
        public static T GetField<T>(this IDataRecord record, String column)
        {
            return (T) record.GetValue(record.GetOrdinal(column));
        }

        #endregion

        #region get as nullable

        // not truly as a Nullable, simply casted to Object (which can be null)
        public static Object GetValueAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Object) null : record.GetValue(ordinal);
        }

        // again doesn't return a Nullable because Strings themselves can be null
        public static String GetStringAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (String) null : record.GetString(ordinal);
        }

        public static Nullable<Int16> GetInt16AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Int16>) null : record.GetInt16(ordinal);
        }

        public static Nullable<Int32> GetInt32AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Int32>) null : record.GetInt32(ordinal);
        }

        public static Nullable<Int64> GetInt64AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Int64>) null : record.GetInt64(ordinal);
        }

        public static Nullable<Decimal> GetDecimalAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Decimal>) null : record.GetDecimal(ordinal);
        }

        public static Nullable<Single> GetFloatAsNullable(this IDataRecord record, String column)
        {
            return GetSingleAsNullable(record, column);
        }

        public static Nullable<Single> GetSingleAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Single>) null : record.GetFloat(ordinal);
        }

        public static Nullable<Double> GetDoubleAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Double>) null : record.GetDouble(ordinal);
        }

        public static Nullable<Boolean> GetBooleanAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Boolean>) null : record.GetBoolean(ordinal);
        }

        public static Nullable<Char> GetCharAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Char>) null : record.GetChar(ordinal);
        }

        public static Nullable<Byte> GetByteAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Byte>) null : record.GetByte(ordinal);
        }

        public static Nullable<DateTime> GetDateTimeAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<DateTime>) null : record.GetDateTime(ordinal);
        }

        public static Nullable<Guid> GetGuidAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<Guid>) null : record.GetGuid(ordinal);
        }

        public static Nullable<SByte> GetSByteAsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<SByte>) null : record.GetSByte(ordinal);
        }

        public static Nullable<UInt16> GetUInt16AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<UInt16>) null : record.GetUInt16(ordinal);
        }

        public static Nullable<UInt32> GetUInt32AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<UInt32>) null : record.GetUInt32(ordinal);
        }

        public static Nullable<UInt64> GetUInt64AsNullable(this IDataRecord record, String column)
        {
            var ordinal = record.GetOrdinal(column);
            return record.IsDBNull(ordinal) ? (Nullable<UInt64>) null : record.GetUInt64(ordinal);
        }

        // similar to GetValueAsNullable except object is required to be a struct and we'll cast to the Nullable<T> of it
        // GetFieldValue is a new method in DbDataReader but not in DataRecord
        // we created this similar functionality here with different method name
        public static Nullable<T> GetFieldAsNullable<T>(this IDataRecord record, String column)
            where T : struct
        {
            var ordinal = record.GetOrdinal(column);
            return (T) (record.IsDBNull(ordinal) ? null : record.GetValue(ordinal));
        }

        #endregion
    }
}
