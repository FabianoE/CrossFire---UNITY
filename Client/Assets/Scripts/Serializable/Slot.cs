using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class SlotRoom  
{
    public bool host { get; set; }
    public int slotId { get; set; }
    public int playerid { get; set; }
    public int slotStatus { get; set; }

    public int playerExp { get; set; }
    public string playername { get; set; }
}
