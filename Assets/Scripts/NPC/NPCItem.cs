using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCItem
{
    public NPCData dataNPC { get; private set; }

    public NPCItem(NPCData data)
    {
        this.dataNPC = data;
    }
}
