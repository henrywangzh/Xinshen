using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerationV1 : MonoBehaviour
{
    [SerializeField] GameObject[] roomObjs;
    [SerializeField] GameObject[] hallwayObjs;
    [SerializeField] GameObject[] wallObjs;
    [SerializeField] GameObject[] doorwayObjs;
    [SerializeField] int targetRoomCount; //NOT IMPLEMENTED
    [SerializeField] float spawnChance, spawnChanceDecrRate, yElevation;
    [SerializeField] int gridSpaceSize;

    List<Room> rooms = new List<Room>();

    const int ROOM_8x8 = 0, ROOM__16x16 = 1, ROOM__24x24 = 2;
    const int HALLWAY_2 = 0, HALLWAY_10 = 1, HALLWAY_18 = 2;
    const int POS_X = 0, NEG_X = 1, POS_Y = 2, NEG_Y = 3;
    const int UNAVAILABLE = 1, AVAILABLE = 0, SELECTED = 2, OPENED = 3;

    class Room
    {
        public Room(int pRoomSize, Vector2 pgridPosition)
        {
            roomSize = pRoomSize;
            gridPosition = pgridPosition;
        }

        public int roomSize;
        public Vector2 gridPosition;
        public int[] nodeStatus = new int[4];
    }

    Vector3 GridToWorldCoordinates(Vector2 coords)
    {
        return new Vector3(coords.x * gridSpaceSize, yElevation, coords.y * gridSpaceSize);
    }
    Vector2 WorldTogridPosition(Vector3 coords)
    {
        return new Vector2(coords.x / gridSpaceSize, coords.z / gridSpaceSize);
    }



    void Start()
    {
        Room newRoom = new Room(ROOM__16x16, Vector2.zero);
        for (int i = 0; i < 4; i++)
        {
            newRoom.nodeStatus[i] = UNAVAILABLE;
        }
        newRoom.nodeStatus[POS_Y] = AVAILABLE;

        rooms.Add(newRoom);

        GenerateRoomsFromRoom(newRoom);
    }

    int roomIndex;
    void FixedUpdate()
    {
        if (roomIndex < rooms.Count)
        {
            InstantiateRoomObject(rooms[roomIndex]);
            InstantiateWalls(rooms[roomIndex]);

            for (int i = 0; i < 4; i++)
            {
                if (rooms[roomIndex].nodeStatus[i] == SELECTED)
                {
                    InstantiateHallway(rooms[roomIndex], GetRoomAtPosition(rooms[roomIndex].gridPosition + DirectionToGridVector(i)));
                    //InstantiateHallway(rooms[roomIndex], i);
                }
            }
            roomIndex++;
        }
        else
        {

        }
    }

    List<Room> newRooms = new List<Room>();
    int roomHeads;

    void GenerateRoomsFromRoom(Room room)
    {
        newRooms.Clear();
        Room newRoom;

        for (int i = 0; i < 4; i++)
        {
            if (room.nodeStatus[i] == AVAILABLE)
            {
                if (Random.Range(0f, 1f) < spawnChance)
                {
                    room.nodeStatus[i] = SELECTED;

                    //InstantiateHallway(room, i);

                    newRoom = GetRoomAtPosition(room.gridPosition + DirectionToGridVector(i));

                    if (newRoom == null)
                    {
                        newRoom = new Room(Random.Range(0,3), room.gridPosition + DirectionToGridVector(i));
                        newRooms.Add(newRoom);
                        newRoom.nodeStatus[GetOppositeNode(i)] = OPENED;
                        //InstantiateRoomObject(newRoom);
                    }
                    else
                    {
                        newRoom.nodeStatus[GetOppositeNode(i)] = OPENED;
                    }
                }
            }
        }

        rooms.AddRange(newRooms);

        spawnChance -= spawnChanceDecrRate;
        for (int i = 0; i < newRooms.Count; i++)
        {
            GenerateRoomsFromRoom(newRooms[i]);
        }
    }

    int GetOppositeNode(int node)
    {
        switch (node)
        {
            case POS_X:
                return NEG_X;
            case NEG_X:
                return POS_X;
            case POS_Y:
                return NEG_Y;
            case NEG_Y:
                return POS_Y;
            default:
                Debug.LogError("invalid argument");
                return -1;
        }
    }

    Vector2 DirectionToGridVector(int direction)
    {
        switch (direction)
        {
            case POS_X:
                return Vector2.right;
            case NEG_X:
                return -Vector2.right;
            case POS_Y:
                return Vector2.up;
            case NEG_Y:
                return -Vector2.up;
            default:
                Debug.LogError("invalid argument");
                return Vector2.zero;
        }
    }

    Vector2 GetHallwayPositionInDirection(Vector2 position, int direction)
    {
        switch (direction)
        {
            case POS_X:
                return position + Vector2.right * .5f;
            case NEG_X:
                return position - Vector2.right * .5f;
            case POS_Y:
                return position + Vector2.up * .5f;
            case NEG_Y:
                return position - Vector2.up * .5f;
            default:
                Debug.LogError("invalid argument");
                return position;
        }
    }

    void InstantiateRoomObject(Room room)
    {
        Instantiate(roomObjs[room.roomSize], GridToWorldCoordinates(room.gridPosition), Quaternion.identity);
    }

    void InstantiateHallway(Room room, int direction)
    {
        if (direction == POS_Y || direction == NEG_Y)
        {
            Instantiate(hallwayObjs[HALLWAY_18], GridToWorldCoordinates(GetHallwayPositionInDirection(room.gridPosition, direction)), Quaternion.identity).transform.Rotate(Vector3.up * 90);
        }
        else
        {
            Instantiate(hallwayObjs[HALLWAY_18], GridToWorldCoordinates(GetHallwayPositionInDirection(room.gridPosition, direction)), Quaternion.identity);
        }
    }
    void InstantiateHallway(Room room1, Room room2)
    {
        Transform hallwayTrfm = Instantiate(hallwayObjs[HALLWAY_18], Vector3.zero, Quaternion.identity).transform;

        if (Mathf.Abs(room2.gridPosition.x - room1.gridPosition.x) < .001)
        {
            hallwayTrfm.Rotate(Vector3.up * 90);

            if (room2.gridPosition.y - room1.gridPosition.y > 0)
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, NEG_Y) + GetRoomEdge(room1, POS_Y)) / 2);

                float length = (GetRoomEdge(room2, NEG_Y).y - GetRoomEdge(room1, POS_Y).y) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
            else
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, POS_Y) + GetRoomEdge(room1, NEG_Y)) / 2);

                float length = (GetRoomEdge(room1, NEG_Y).y - GetRoomEdge(room2, POS_Y).y) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
        }
        else
        {
            if (room2.gridPosition.x - room1.gridPosition.x > 0)
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, NEG_X) + GetRoomEdge(room1, POS_X)) / 2);

                float length = (GetRoomEdge(room2, NEG_X).x - GetRoomEdge(room1, POS_X).x) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
            else
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, POS_X) + GetRoomEdge(room1, NEG_X)) / 2);

                float length = (GetRoomEdge(room1, NEG_X).x - GetRoomEdge(room2, POS_X).x) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
        }
    }

    Vector2 GetRoomEdge(Room room, int direction)
    {
        if (room.roomSize == ROOM_8x8)
        {
            return room.gridPosition + DirectionToGridVector(direction) * 4f / gridSpaceSize;
        }
        else if (room.roomSize == ROOM__16x16)
        {
            return room.gridPosition + DirectionToGridVector(direction) * 8f / gridSpaceSize;
        }
        else if (room.roomSize == ROOM__24x24)
        {
            return room.gridPosition + DirectionToGridVector(direction) * 12f / gridSpaceSize;
        }

        return Vector2.zero;
    }

    void InstantiateWalls(Room room)
    {
        Transform wallTrfm;
        for (int i = 0; i < 4; i++)
        {
            if (room.nodeStatus[i] == SELECTED || room.nodeStatus[i] == OPENED)
            {
                wallTrfm = Instantiate(doorwayObjs[room.roomSize], GridToWorldCoordinates(GetRoomEdge(room, i)) + Vector3.up * 2, Quaternion.identity).transform;
                if (i == POS_Y || i == NEG_Y) { wallTrfm.Rotate(Vector3.up * 90); }
            }
            else
            {
                wallTrfm = Instantiate(wallObjs[room.roomSize], GridToWorldCoordinates(GetRoomEdge(room, i)) + Vector3.up * 2, Quaternion.identity).transform;
                if (i == POS_Y || i == NEG_Y) { wallTrfm.Rotate(Vector3.up * 90); }
            }
        }
    }

    Room GetRoomAtPosition(Vector2 position)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (Mathf.Abs(rooms[i].gridPosition.x - position.x) < .01f && Mathf.Abs(rooms[i].gridPosition.y - position.y) < .01f)
            {
                return rooms[i];
            }
        }

        return null;
    }
}
