using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralGenerationV1 : MonoBehaviour
{
    [SerializeField] GameObject[] roomObjs;
    [SerializeField] GameObject hallwayObj;
    [SerializeField] GameObject[] wallObjs;
    [SerializeField] GameObject[] doorwayObjs;
    [SerializeField] GameObject windTunnel;
    //[SerializeField] int targetRoomCount; //NOT IMPLEMENTED
    [SerializeField] float spawnChance, spawnChanceDecrRate, linearity, yElevation;
    float startingSpawnChance;
    enum algorithm {DFS, BFS };
    [SerializeField] algorithm currentAlgorithm;
    //linearity: 0 - 1.0 denoting how linear the dungeon will be (e.g. proportion of BFS to DFS. 0.4 = 40% DFS)
    [SerializeField] int gridSpaceSize;
    [SerializeField] int floors = 1;
    [SerializeField] int linearityPersistence = 5;
    [SerializeField] float skipChance = 0.5f;
    int currentLinearityPersistence, numRooms;

    List<List<Room>> allRooms = new List<List<Room>>();
    List<Room> ungeneratedRooms = new List<Room>();
    List<Room> rooms;
    bool doneGenerating = false;

    const int ROOM_8x8 = 0, ROOM__16x16 = 1, ROOM__24x24 = 2, ROOM__16x16__STAIR = 3;
    //const int HALLWAY_2 = 0, HALLWAY_10 = 1, HALLWAY_18 = 2;
    const int POS_X = 0, NEG_X = 1, POS_Y = 2, NEG_Y = 3;
    const int UNAVAILABLE = 1, AVAILABLE = 0, SELECTED = 2, OPENED = 3;

    [SerializeField] GameObject teleporter;
    [SerializeField] string nextScene;

    class Room
    {
        public Room(int pRoomSize, Vector2 pgridPosition, float yPosition)
        {
            roomSize = pRoomSize;
            gridPosition = pgridPosition;
            this.yPosition = yPosition;
        }
        public float yPosition;
        public int roomSize;
        public Vector2 gridPosition;
        public int[] nodeStatus = new int[4];
    }

    Vector3 GridToWorldCoordinates(Vector2 coords, float yPosition)
    {
        return new Vector3(coords.x * gridSpaceSize, yPosition, coords.y * gridSpaceSize);
    }
    Vector2 WorldTogridPosition(Vector3 coords)
    {
        return new Vector2(coords.x / gridSpaceSize, coords.z / gridSpaceSize);
    }



    void Start()
    {
        startingSpawnChance = spawnChance;
        for(int i = 0; i < floors; i++)
        {
            allRooms.Add(new List<Room>());
            rooms = allRooms[i];
            currentLinearityPersistence = linearityPersistence;
            Room startingRoom;
            if (i == 0)
            {
                startingRoom = new Room(ROOM__16x16, Vector2.zero, yElevation);

            }
            else
            {
                List<Room> potentialStairRooms = allRooms[i - 1].FindAll(room => room.roomSize == ROOM__16x16);

                Room stairRoom = potentialStairRooms[Random.Range(0, potentialStairRooms.Count)];

                Vector2 startingPoint = stairRoom.gridPosition;
                startingRoom = new Room(ROOM__16x16__STAIR, startingPoint, yElevation);

            }
            for (int j = 0; j < 4; j++)
            {
                startingRoom.nodeStatus[j] = UNAVAILABLE;
            }
            startingRoom.nodeStatus[POS_Y] = AVAILABLE;

            rooms.Add(startingRoom);
            GenerateRoomsFromRoom(startingRoom);
            yElevation += 10;
            spawnChance = startingSpawnChance;
        }

        doneGenerating = true;
        rooms = allRooms.SelectMany(x => x).ToList();
    }

    int roomIndex;
    void FixedUpdate()
    {
        //for(int x = 0; x < floors; x++)
        //{
        //    rooms = allRooms[x];
        //    if (roomIndex < rooms.Count)
        //    {
        //        InstantiateRoomObject(rooms[roomIndex]);
        //        InstantiateWalls(rooms[roomIndex]);

        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (rooms[roomIndex].nodeStatus[i] == SELECTED)
        //            {
        //                InstantiateHallway(rooms[roomIndex], GetRoomAtPosition(rooms[roomIndex].gridPosition + DirectionToGridVector(i)));
        //                //InstantiateHallway(rooms[roomIndex], i);
        //            }
        //        }
        //        roomIndex++;
        //    }
        //    else
        //    {

        //    }
        //}
        if (!doneGenerating) return;
        if (roomIndex < rooms.Count)
        {
            if(Random.Range(0f, 1f) > skipChance)
            {
                InstantiateRoomObject(rooms[roomIndex]);
                InstantiateWalls(rooms[roomIndex]);
                for (int i = 0; i < 4; i++)
                {
                    if (rooms[roomIndex].nodeStatus[i] == SELECTED)
                    {
                        InstantiateHallway(rooms[roomIndex], GetRoomAtPosition(rooms[roomIndex].gridPosition + DirectionToGridVector(i), rooms[roomIndex].yPosition));
                        //InstantiateHallway(rooms[roomIndex], i);
                    }
                }

                if (roomIndex == rooms.Count - 1)
                {
                    if (latestRoom.GetComponent<RoomPopulator>())
                    {
                        Destroy(latestRoom.GetComponent<RoomPopulator>());
                    }

                    Instantiate(teleporter, GridToWorldCoordinates(rooms[roomIndex].gridPosition, rooms[roomIndex].yPosition) + Vector3.up * 3, Quaternion.identity).GetComponent<Teleporter>().Scene = nextScene;
                }
            }
            

           
            roomIndex++;
        }
    }

    //int roomHeads;

    void GenerateRoomsFromRoom(Room room)
    {
        List<Room> newRooms = new List<Room>();
        Room newRoom;
        int randomIndex = Random.Range(0,4);

        for (int i = 0; i < 4; i++)
        {
            if (room.nodeStatus[randomIndex] == AVAILABLE)
            {
                if (Random.Range(0f, 1f) < spawnChance)
                {
                    spawnChance -= spawnChanceDecrRate;

                    room.nodeStatus[randomIndex] = SELECTED;

                    //InstantiateHallway(room, i);

                    newRoom = GetRoomAtPosition(room.gridPosition + DirectionToGridVector(randomIndex), room.yPosition);

                    if (newRoom == null)
                    {
                        newRoom = new Room(Random.Range(0,3), room.gridPosition + DirectionToGridVector(randomIndex), room.yPosition);
                        newRooms.Add(newRoom);
                        newRoom.nodeStatus[GetOppositeNode(randomIndex)] = OPENED;
                        //InstantiateRoomObject(newRoom);
                    }
                    else
                    {
                        newRoom.nodeStatus[GetOppositeNode(randomIndex)] = OPENED;
                    }
                }
            }
            randomIndex = (randomIndex + 1) % 4;
        }

        rooms.AddRange(newRooms);
        ungeneratedRooms.Remove(room);
        ungeneratedRooms.AddRange(newRooms);
        currentLinearityPersistence--;
        if (currentLinearityPersistence <= 0)
        {
            currentAlgorithm = Random.Range(0f, 1f) < linearity ? algorithm.DFS : algorithm.BFS;
            currentLinearityPersistence = linearityPersistence;
        }
        if (currentAlgorithm == algorithm.DFS)
        {
            if(newRooms.Count <= 0)
            {
                return;
            }
            randomIndex = Random.Range(0, newRooms.Count);
            for (int i = 0; i < newRooms.Count; i++)
            {
                randomIndex = (randomIndex + 1) % newRooms.Count;
                GenerateRoomsFromRoom(newRooms[randomIndex]);
            }
        }
        else
        {
            randomIndex = Random.Range(0, ungeneratedRooms.Count);
            for (int i = 0; i < ungeneratedRooms.Count; i++)
            {
                randomIndex = (randomIndex + 1) % ungeneratedRooms.Count;
                GenerateRoomsFromRoom(ungeneratedRooms[randomIndex]);
            }
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

    GameObject latestRoom;
    void InstantiateRoomObject(Room room)
    {
        latestRoom = Instantiate(roomObjs[room.roomSize], GridToWorldCoordinates(room.gridPosition, room.yPosition), Quaternion.identity);
        if (room.roomSize == ROOM__16x16__STAIR)
        {
            Instantiate(windTunnel, GridToWorldCoordinates(room.gridPosition, room.yPosition - 10), Quaternion.identity);
        }

        numRooms++;

        if (numRooms > 1)
        {
            if (latestRoom.GetComponent<RoomPopulator>())
            {
                latestRoom.GetComponent<RoomPopulator>().targetDifficulty = 30 + numRooms * 5;
            }
        }
        else
        {
            Destroy(latestRoom.GetComponent<RoomPopulator>());
        }
    }

    //void InstantiateHallway(Room room, int direction)
    //{
    //    if (direction == POS_Y || direction == NEG_Y)
    //    {
    //        Instantiate(hallwayObj, GridToWorldCoordinates(GetHallwayPositionInDirection(room.gridPosition, direction), room.yPosition), Quaternion.identity).transform.Rotate(Vector3.up * 90);
    //    }
    //    else
    //    {
    //        Instantiate(hallwayObj, GridToWorldCoordinates(GetHallwayPositionInDirection(room.gridPosition, direction), room.yPosition), Quaternion.identity);
    //    }
    //}
    void InstantiateHallway(Room room1, Room room2)
    {
        Transform hallwayTrfm = Instantiate(hallwayObj, Vector3.zero, Quaternion.identity).transform;

        if (Mathf.Abs(room2.gridPosition.x - room1.gridPosition.x) < .001)
        {
            hallwayTrfm.Rotate(Vector3.up * 90);

            if (room2.gridPosition.y - room1.gridPosition.y > 0)
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, NEG_Y) + GetRoomEdge(room1, POS_Y)) / 2, room1.yPosition);

                float length = (GetRoomEdge(room2, NEG_Y).y - GetRoomEdge(room1, POS_Y).y) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
            else
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, POS_Y) + GetRoomEdge(room1, NEG_Y)) / 2, room1.yPosition);

                float length = (GetRoomEdge(room1, NEG_Y).y - GetRoomEdge(room2, POS_Y).y) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
        }
        else
        {
            if (room2.gridPosition.x - room1.gridPosition.x > 0)
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, NEG_X) + GetRoomEdge(room1, POS_X)) / 2, room1.yPosition);

                float length = (GetRoomEdge(room2, NEG_X).x - GetRoomEdge(room1, POS_X).x) * gridSpaceSize;
                hallwayTrfm.localScale = new Vector3(length, 1, 5);
            }
            else
            {
                hallwayTrfm.position = GridToWorldCoordinates((GetRoomEdge(room2, POS_X) + GetRoomEdge(room1, NEG_X)) / 2, room1.yPosition);

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
        else if (room.roomSize == ROOM__16x16 || room.roomSize == ROOM__16x16__STAIR)
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
                wallTrfm = Instantiate(doorwayObjs[room.roomSize], GridToWorldCoordinates(GetRoomEdge(room, i), room.yPosition) + Vector3.up * 2, Quaternion.identity).transform;
                if (i == POS_Y || i == NEG_Y) { wallTrfm.Rotate(Vector3.up * 90); }
            }
            else
            {
                wallTrfm = Instantiate(wallObjs[room.roomSize], GridToWorldCoordinates(GetRoomEdge(room, i), room.yPosition) + Vector3.up * 2, Quaternion.identity).transform;
                if (i == POS_Y || i == NEG_Y) { wallTrfm.Rotate(Vector3.up * 90); }
            }
        }
    }

    Room GetRoomAtPosition(Vector2 position, float yPosition)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (Mathf.Abs(rooms[i].gridPosition.x - position.x) < .01f && Mathf.Abs(rooms[i].gridPosition.y - position.y) < .01f && Mathf.Abs(rooms[i].yPosition - yPosition) < .01f)
            {
                return rooms[i];
            }
        }

        return null;
    }
}
