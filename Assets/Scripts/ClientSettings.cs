using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


//Handles settings
public class ClientSettings : MonoBehaviour
{
    
    public static Dictionary<string, dynamic> items = new Dictionary<string, dynamic>();
    [RuntimeInitializeOnLoadMethod]
    public static void LoadSettings()
    {
        return;
        using (StreamReader file = File.OpenText(Application.persistentDataPath + "/clientSettings.json"))
        {

        }
    }
}
