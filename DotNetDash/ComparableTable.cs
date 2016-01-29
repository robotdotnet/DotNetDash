using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    public struct ComparableTable : IComparable, IComparable<ComparableTable>
    {
        public ComparableTable(string tableName, ITable table)
            :this()
        {
            Name = tableName;
            Table = table;
        }
        public ITable Table { get; }
        public string Name { get; }

        public int CompareTo(object obj)
        {
            var rhs = obj as ComparableTable?;
            if (rhs == null) return -1;
            return CompareTo(rhs.Value);
        }

        public int CompareTo(ComparableTable other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
