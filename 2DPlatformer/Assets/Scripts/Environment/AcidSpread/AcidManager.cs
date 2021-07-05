using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AcidManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase acidTile;

    [SerializeField]
    private MapManager mapManger;

    [SerializeField]
    private AcidSpread acidPrefab;

    private List<Vector3Int> activeAcids = new List<Vector3Int>();
    public void FinishedAcid(Vector3Int position)
    {
        map.SetTile(position, acidTile);
        activeAcids.Remove(position);
    }

    public void TryToSpread(Vector3Int position, float spreadChance)
    {
        for(int x = position.x -1; x < position.x + 2; x++)
        {
            for (int y = position.y -1; y< position.y + 2; y++)
            {
                TryToBurnTile(new Vector3Int(x, y, 0));
            }
        }

        void TryToBurnTile(Vector3Int tilePosition)
        {
            if (activeAcids.Contains(tilePosition)) return;

            TileData data = mapManger.GetTileData(tilePosition);

            if(data != null && data.canSetAcid)
            {
                if (UnityEngine.Random.Range(0f, 100f) <= data.spreadChance)
                    SetTileOnAcid(tilePosition, data);
            }
        }
    }

    private void SetTileOnAcid(Vector3Int tilePosition, TileData data)
    {
        AcidSpread newAcid = Instantiate(acidPrefab);
        newAcid.transform.position = map.GetCellCenterWorld(tilePosition);
        newAcid.StartAcid(tilePosition, data, this);

        activeAcids.Add(tilePosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileData data = mapManger.GetTileData(gridPosition);

            SetTileOnAcid(gridPosition, data);
        }
    }
}
