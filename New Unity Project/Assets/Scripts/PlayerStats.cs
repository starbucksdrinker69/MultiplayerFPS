using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text killCount;
    public Text deathCount;

    // Start is called before the first frame update
    void Start()
    {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData); 
    }

    void OnReceivedData (string data)
    {
        if (killCount == null || deathCount == null)
            return;

        killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }
}
