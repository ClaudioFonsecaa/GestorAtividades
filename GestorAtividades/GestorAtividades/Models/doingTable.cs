using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorAtividades.Models
{
    public class doingTable
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataInicio { get; set; }

        public string DataFim { get; set; }

        public string Tempo { get; set; }

        public string Description { get; set; }

        public Boolean Alarme { get; set; }

        public int NotificationID { get; set; }
    }
}
