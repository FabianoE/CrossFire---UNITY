﻿using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class RoomListData
{
    public int Room_Id { get; set; }
    public string Room_Name { get; set; }
    public int Room_Mode { get; set; }
    public int Room_Map { get; set; }
    public string Room_Players { get; set; }
}
