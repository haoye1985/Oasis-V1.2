using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pan.Settings;

namespace Oasis_v1._1
{
    public class Setting : SettingsBook
    {
        [InfoEdit("δeλos", "Backup file before saving", eInfoType.Bool)]
        public bool AutoBackUp = true;

        [InfoEdit("δeλos", "On load, open last opened file", eInfoType.Integer)]
        public int ConsoleBuffer = 50;
    }
}
