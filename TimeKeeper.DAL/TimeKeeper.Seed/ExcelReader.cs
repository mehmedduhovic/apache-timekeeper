using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Seed
{
    public static class ExcelReader
    {
        public static string DataSource { get; set; }

        public static DataTable Open(this DataTable table, string sheet)
        {
            OleDbConnection conn = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DataSource};Extended Properties=Excel 12.0");
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "$]", conn);
            OleDbDataAdapter da = new OleDbDataAdapter() { SelectCommand = cmd };

            table = new DataTable();
            da.Fill(table);
            conn.Close();

            return table;
        }

        public static object Read(this DataRow row, int index, Type type)
        {
            try
            {
                var content = row.ItemArray.GetValue(index);
                switch (type.Name)
                {
                    case "Int32": return (content == DBNull.Value) ? 0 : Convert.ToInt32(content);
                    case "String": return (content == null) ? " " : content.ToString();
                    case "Boolean": return (content.ToString() == "-1");
                    case "DateTime": return (content == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(content);
                    case "Decimal": return (content == null) ? 0 : (decimal)Convert.ToDouble(content);
                    case "Double": return (content == null) ? 0 : (decimal)Convert.ToDouble(content);
                    default: return (content.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("****** ERROR *******\n" + ex.Message);
                Console.ReadKey();
                return 0;
            }
            //return 0;
        }
    }
}
