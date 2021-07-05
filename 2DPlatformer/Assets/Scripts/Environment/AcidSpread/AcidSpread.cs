using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpread : MonoBehaviour
{
    private Vector3Int position;

    private TileData data;

    private AcidManager acidManager;

    private float acidTimeCounter, spreadIntervallCounter;


    public void StartAcid(Vector3Int position, TileData data, AcidManager am)
    {
        this.position = position;
        this.data = data;
        acidManager = am;

        acidTimeCounter = data.acidTime;
        spreadIntervallCounter = data.spreadIntervall;
    }

    private void Update()
    {
        acidTimeCounter -= Time.deltaTime;

        if(acidTimeCounter <= 0)
        {
            acidManager.FinishedAcid(position);
            Destroy(gameObject);
        }

        spreadIntervallCounter -= Time.deltaTime;
        if(spreadIntervallCounter <= 0)
        {
            spreadIntervallCounter = data.spreadIntervall;
            acidManager.TryToSpread(position, data.spreadChance);
        }
    }
}
