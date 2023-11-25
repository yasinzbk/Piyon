using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData 
{
    public void LoadData(SaveData data);

    public void SaveData(SaveData data);
}
