using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeed : MonoBehaviour
{
    [SerializeField]
    GameObject killfeedItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
        go.GetComponent<KillfeedItem>().Setup(player, source);

        Destroy(go, 4f);
    }
}
