using UnityEngine;

public class RoomGridManager : MonoBehaviour
{
    public int rows = 3;
    public int columns = 10;
    public GameObject roomPrefab;
    public Transform gridOrigin;
    public float cellSpacing = 2.5f;

    private BuyingRoom[,] roomGrid;

    void Start()
    {
        roomGrid = new BuyingRoom[rows, columns];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        roomGrid = new BuyingRoom[rows, columns];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 spawnPos = gridOrigin.position + new Vector3(x * cellSpacing, -y * cellSpacing, 0);
                GameObject roomGO = Instantiate(roomPrefab, spawnPos, Quaternion.identity, gridOrigin);

                BuyingRoom room = roomGO.GetComponent<BuyingRoom>();
                room.Initialize(x, y, this, baseCost: 10 + x * 5);
                roomGrid[y, x] = room;
            }
        }
    }
    public void OnRoomPurchased(int x, int y)
    {
        TryUnlock(x + 1, y);
        TryUnlock(x - 1, y);
        TryUnlock(x, y + 1);
        TryUnlock(x, y - 1);
    }

    void TryUnlock(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            BuyingRoom room = roomGrid[y, x];
            if (room != null && room.Locked)
                room.Unlock();
        }
    }
    private void OnDrawGizmos()
    {
        if (gridOrigin == null) return;

        Gizmos.color = Color.cyan;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 pos = gridOrigin.position + new Vector3(x * cellSpacing, -y * cellSpacing, 0);
                Gizmos.DrawWireCube(pos, new Vector3(1.5f, 1.5f, 0));
            }
        }
    }
    public void UnlockAdjacentRooms(int x, int y)
    {
        TryUnlockRoom(x + 1, y); // Right
        TryUnlockRoom(x - 1, y); // Left
        TryUnlockRoom(x, y + 1); // Up
        TryUnlockRoom(x, y - 1); // Down
    }

    private void TryUnlockRoom(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            var room = roomGrid[y, x];
            if (room != null && room.Locked)
            {
                room.Unlock();
            }
        }
    }
}
