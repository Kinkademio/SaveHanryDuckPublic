using System;
using System.Collections.Generic;
using System.Reflection;
using RoomInteriorGeneratorTag;
using Unity.Mathematics;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerDrone;
    public GameObject MainCamera;

    public List<GameObject> interactiveObjects = new List<GameObject>();
    public List<Pair <GameObject[,], bool>> rooms = new List<Pair<GameObject[,], bool>>();

    public GameObject floor;
    public GameObject wall;
    public GameObject door;
    public GameObject staticObject;
    public Sprite wardrobeStaticObject;

    public GameObject triggerObject;
    public GameObject eluminatorObject;
    public GameObject storageObject;

    public GameObject interactiveStaticObject;
    public Sprite boxNonStaticObject;
    public Sprite provisionNonStaticObject;
    public Sprite bloknoteInteractiveStaticObject;
    public Sprite laptopInteractiveStaticObject;
    public Sprite laptopBrokeInteractiveStaticObject;
    public Sprite pizzaInteractiveStaticObject;
    public Sprite glassInteractiveStaticObject;
    public Sprite liafietInteractiveStaticObject;
    public Sprite radioInteractiveStaticObject;

    public GameObject nonStaticObject;
    public Sprite chairNonStaticObject;

    public GameObject pickUp;

    public GameObject passiveEnemy;
    public GameObject activeEnemy;

    public int BaseRoomSize;

    public void Start()
    {
        MainCamera = GameObject.Find("Main Camera");
        BaseRoomSize = 0;
    }

    public void GenerationManagerFirstRoom()
    {
        double x = 0; double y = -1;
        SpawnRoom(new PointDouble(x, y), Direction.Top, 0);
    }

    public void DestroyAllWithout(int index)
    {
        for(int i = rooms.Count - 1; i >= 0; i--)
        {
            if (i != index)
            {
                DestroyRoom(i);
            }
        }

        if (rooms.Count != 0)
        {
            float Dx = rooms[0].First[0, 0].transform.position.x;
            float Dy = rooms[0].First[0, 0].transform.position.y;

            for (int i = 0; i < rooms[0].First.GetLength(0); i++)
            {
                for (int j = 0; j < rooms[0].First.GetLength(1); j++)
                {
                    if (rooms[0].First[i, j] != null)
                    {
                        rooms[0].First[i, j].transform.position = new Vector3
                            (rooms[0].First[i, j].transform.position.x - Dx,
                             rooms[0].First[i, j].transform.position.y - Dy,
                             rooms[0].First[i, j].transform.position.z + 1);
                    }
                }
            }

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                interactiveObjects[i].transform.position = new Vector3
                    (interactiveObjects[i].transform.position.x - Dx,
                     interactiveObjects[i].transform.position.y - Dy,
                     interactiveObjects[i].transform.position.z + 1);
            }

            Player.transform.position = new Vector3(Player.transform.position.x - Dx, Player.transform.position.y - Dy, -2);
            PlayerDrone.transform.position = new Vector3(PlayerDrone.transform.position.x - Dx, PlayerDrone.transform.position.y - Dy, -3);

            MainCamera.transform.position = new Vector3
                (MainCamera.transform.position.x - Dx,
                 MainCamera.transform.position.y - Dy,
                 MainCamera.transform.position.z);
        }
    }
    public void DestroyAllWithout()
    {
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            DestroyRoom(i);
        }
    }


    public int SpawnRoom(PointDouble startDoor, Direction direction, int z)
    {

        int SizeX = UnityEngine.Random.Range(BaseRoomSize + 11, BaseRoomSize + 18), SizeY = UnityEngine.Random.Range(BaseRoomSize + 11, BaseRoomSize + 18);
        Room room = new Room(RoomShape.Rectangle, SizeX, SizeY, 4, 3, 2, 2, 2, direction);

        float xStart = 0;
        float yStart = 0;

        switch (direction)
        {
            case Direction.Top:
                xStart = (float)(startDoor.x - room.doors[0].center.x);
                yStart = (float)(startDoor.y + room.doors[0].center.y);
                break;
            case Direction.Left:
                xStart = (float)(startDoor.x - room.doors[0].center.x);
                yStart = (float)(startDoor.y + room.doors[0].center.y);
                break;
            case Direction.Right:
                xStart = (float)(startDoor.x + room.doors[0].center.x);
                yStart = (float)(startDoor.y + room.doors[0].center.y);
                break;
            case Direction.Bottom:
                xStart = (float)(startDoor.x - room.doors[0].center.x);
                yStart = (float)(startDoor.y - room.doors[0].center.y);
                break;
            default:
                break;
        }

        rooms.Add(new Pair<GameObject[,], bool>(new GameObject[SizeX, SizeY], true));

        for (int x = 0; x < room.roomObjects.GetLength(0); x++)
        {
            for (int y = 0; y < room.roomObjects.GetLength(1); y++)
            {
                if (room.roomObjects[x, y] == Surface.Floor)
                {
                    rooms[rooms.Count - 1].First[x, y] = Instantiate(floor, new Vector3(xStart + x, yStart - y, z), Quaternion.identity);
                }
                if (room.roomObjects[x, y] == Surface.Wall)
                {
                    rooms[rooms.Count - 1].First[x, y] = Instantiate(wall, new Vector3(xStart + x, yStart - y, z), Quaternion.identity);
                }
                if (room.roomObjects[x, y] == Surface.StaticObject)
                {
                    rooms[rooms.Count - 1].First[x, y] = Instantiate(staticObject, new Vector3(xStart + x, yStart - y, z), Quaternion.identity);
                    if ((room.NeighborCount(x, y, Surface.StorageObject) == 0) && (room.NeighborCount(x, y, Surface.StaticObject) == 1))
                    {
                        rooms[rooms.Count - 1].First[x, y].GetComponent<SpriteRenderer>().sprite = wardrobeStaticObject;
                        Quaternion quaternion = Quaternion.identity * Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90);
                        rooms[rooms.Count - 1].First[x, y].GetComponent<Transform>().rotation = quaternion;
                    }
                }
                if (room.roomObjects[x, y] == Surface.StorageObject)
                {
                    rooms[rooms.Count - 1].First[x, y] = Instantiate(storageObject, new Vector3(xStart + x, yStart - y, z), Quaternion.identity);
                }
                if (room.interactiveObjects[x, y] == Interactive.StaticObject)
                {
                    Quaternion quaternion = Quaternion.identity * Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    interactiveObjects.Add(Instantiate(interactiveStaticObject, new Vector3(xStart + x, yStart - y, z - 1), quaternion));
                    System.Random random = new System.Random();

                    if ((room.roomObjects[x, y] == Surface.StaticObject) && (room.NeighborCount(x, y, Surface.StorageObject) == 0) && (room.NeighborCount(x, y, Surface.StaticObject) == 1))
                    {
                        interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = radioInteractiveStaticObject;
                    }
                    else if (room.roomObjects[x, y] == Surface.StaticObject)
                    {
                        double temp = random.NextDouble();
                        if (temp > 0.75)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = boxNonStaticObject;
                        }
                        else if (temp > 0.5)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = provisionNonStaticObject;
                        }
                        else if (temp > 0.25)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = liafietInteractiveStaticObject;
                        }
                        else
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = radioInteractiveStaticObject;
                        }
                    }
                    else if (room.roomObjects[x, y] == Surface.StorageObject)
                    {
                        double temp = random.NextDouble();

                        if (temp > 0.8)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = bloknoteInteractiveStaticObject;
                        }
                        else if (temp > 0.6)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = laptopInteractiveStaticObject;
                        }
                        else if (temp > 0.4)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = pizzaInteractiveStaticObject;
                        }
                        else if (temp > 0.2)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = liafietInteractiveStaticObject;
                        }
                    }
                    else if(room.roomObjects[x, y] == Surface.Floor)
                    {
                        double temp = random.NextDouble();

                        if (temp > 0.90)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = laptopBrokeInteractiveStaticObject;
                        }
                        else if (temp > 0.80)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = glassInteractiveStaticObject;
                        }
                        else if (temp > 0.50)
                        {
                            interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = liafietInteractiveStaticObject;
                        }
                    }

                }
                if (room.interactiveObjects[x, y] == Interactive.NonStaticObject)
                {
                    interactiveObjects.Add(Instantiate(nonStaticObject, new Vector3(xStart + x, yStart - y, z - 1), Quaternion.identity));
                    if (room.NeighborCount(x, y, Surface.StorageObject) > 0)
                    {
                        interactiveObjects[interactiveObjects.Count - 1].GetComponent<SpriteRenderer>().sprite = chairNonStaticObject;
                        Quaternion quaternion = Quaternion.identity * Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                        interactiveObjects[interactiveObjects.Count - 1].GetComponent<Transform>().rotation = quaternion;
                    }
                }
                if (room.interactiveObjects[x, y] == Interactive.PickUp)
                {
                    Quaternion quaternion = Quaternion.identity * Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    interactiveObjects.Add(Instantiate(pickUp, new Vector3(xStart + x, yStart - y, z - 2), quaternion));
                }
                if (room.interactiveObjects[x, y] == Interactive.PassiveEnemy)
                {
                    interactiveObjects.Add(Instantiate(passiveEnemy, new Vector3(xStart + x, yStart - y, z - 2), Quaternion.identity));
                }
                if (room.interactiveObjects[x, y] == Interactive.ActiveEnemy)
                {
                    interactiveObjects.Add(Instantiate(activeEnemy, new Vector3(xStart + x, yStart - y, z - 2), Quaternion.identity));
                }
            }
        }

        for (int i = 0; i < room.objectSites.Count; i++)
        {
            if (room.objectSites[i].typeSite == Surface.TriggerObject)
            {
                System.Random random = new System.Random();
                double triggerCenterX = room.objectSites[i].x + (double)(room.objectSites[i].width - 1) / 2;
                double triggerCenterY = room.objectSites[i].y + (double)(room.objectSites[i].height - 1) / 2;

                double temp = random.NextDouble();

                if (temp > 0.75)
                {
                    rooms[rooms.Count - 1].First[room.objectSites[i].x, room.objectSites[i].y] =
                        Instantiate(triggerObject, new Vector3(xStart + (float)triggerCenterX, yStart - (float)triggerCenterY, z), Quaternion.identity);
                } 
                else
                {
                    rooms[rooms.Count - 1].First[room.objectSites[i].x, room.objectSites[i].y] =
                        Instantiate(eluminatorObject, new Vector3(xStart + (float)triggerCenterX, yStart - (float)triggerCenterY, z), Quaternion.identity);
                }
            }
        }

        for (int i = 0; i < room.doors.Count; i++)
        {
            Quaternion quaternion;
            switch (room.doors[i].direction)
            {
                case Direction.Top:
                    quaternion = Quaternion.identity * Quaternion.Euler(0, 0, 180);
                    rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y] =
                        Instantiate(door, new Vector3(xStart + (float)room.doors[i].center.x, yStart - (float)room.doors[i].center.y, z - 1), quaternion);
                    break;
                case Direction.Bottom:
                    rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y] =
                        Instantiate(door, new Vector3(xStart + (float)room.doors[i].center.x, yStart - (float)room.doors[i].center.y, z - 1), Quaternion.identity);
                    break;
                case Direction.Left:
                    quaternion = Quaternion.identity * Quaternion.Euler(0, 0, 270);
                    rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y] =
                        Instantiate(door, new Vector3(xStart + (float)room.doors[i].center.x, yStart - (float)room.doors[i].center.y, z - 1), quaternion);
                    break;
                case Direction.Right:
                    quaternion = Quaternion.identity * Quaternion.Euler(0, 0, 90);
                    rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y] =
                        Instantiate(door, new Vector3(xStart + (float)room.doors[i].center.x, yStart - (float)room.doors[i].center.y, z - 1), quaternion);
                    break;           
                default:
                    break;
            }

            if (i == 0)
            {
                rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y].GetComponent<DoorTrigger>().firstDoor = true;
            }

            rooms[rooms.Count - 1].First[room.doors[i].x, room.doors[i].y].GetComponent<DoorTrigger>().direction = room.doors[i].direction;
        }

        return rooms.Count - 1;
    }

    //Direction DoorDirection(Direction direction, int i, int j, Pair<int, int> sizeRoom)
    //{
    //    switch (direction) {
    //        case Direction.Top:
    //            if(i == 0)
    //            {
    //                return Direction.Top;
    //            }
    //            if (i == sizeRoom.First - 1)
    //            {
    //                return Direction.Bottom;
    //            }
    //            if (j == 0)
    //            {
    //                return Direction.Left;
    //            }
    //            if (j == sizeRoom.Second - 1)
    //            {
    //                return Direction.Right;
    //            }
    //            break;
    //        case Direction.Left:
    //            if (i == 0)
    //            {
    //                return Direction.Left; // Top
    //            }
    //            if (i == sizeRoom.First - 1)
    //            {
    //                return Direction.Right; // Bottom
    //            }
    //            if (j == 0)
    //            {
    //                return Direction.Bottom; // Left
    //            }
    //            if (j == sizeRoom.Second - 1)
    //            {
    //                return Direction.Top; // Right
    //            }
    //            break;
    //        case Direction.Right:
    //            if (i == 0)
    //            {
    //                return Direction.Right; // Top
    //            }
    //            if (i == sizeRoom.First - 1)
    //            {
    //                return Direction.Left; // Bottom
    //            }
    //            if (j == 0)
    //            {
    //                return Direction.Bottom; // Right
    //            }
    //            if (j == sizeRoom.Second - 1)
    //            {
    //                return Direction.Top; // Left
    //            }
    //            break;
    //        case Direction.Bottom:
    //            if (i == 0)
    //            {
    //                return Direction.Bottom;
    //            }
    //            if (i == sizeRoom.First - 1)
    //            {
    //                return Direction.Top;
    //            }
    //            if (j == 0)
    //            {
    //                return Direction.Left;
    //            }
    //            if (j == sizeRoom.Second - 1)
    //            {
    //                return Direction.Right;
    //            }
    //            break;
    //        default:
    //            break;

    //    }
    //    return direction;
    //}

    public void DestroyRoom(int index)
    {
        for (int i = 0; i < interactiveObjects.Count; i++)
        {
            if (interactiveObjects[i] == null)
            {
                interactiveObjects.Remove(interactiveObjects[i]);
                i--;
                continue;
            }

            if ((rooms[index].First[0, 0].transform.position.x < interactiveObjects[i].transform.position.x) &&
                (rooms[index].First[0, 0].transform.position.y > interactiveObjects[i].transform.position.y) &&
                (rooms[index].First[rooms[index].First.GetLength(0) - 1, rooms[index].First.GetLength(1) - 1].transform.position.x > interactiveObjects[i].transform.position.x) &&
                (rooms[index].First[rooms[index].First.GetLength(0) - 1, rooms[index].First.GetLength(1) - 1].transform.position.y < interactiveObjects[i].transform.position.y))
            {
                Destroy(interactiveObjects[i]);
                interactiveObjects.Remove(interactiveObjects[i]);
                i--;
            }
                
        }

        for (int x = 0; x < rooms[index].First.GetLength(0); x++)
        {
            for (int y = 0; y < rooms[index].First.GetLength(1); y++)
            {
                if (rooms[index].First[x, y] != null)
                {
                    Destroy(rooms[index].First[x, y]);
                }
            }
        }
        
        rooms.Remove(rooms[index]);
    }
    public void DisActiveRoom(Pair<GameObject[,], bool> room)
    {
       
        for (int x = 0; x < room.First.GetLength(0); x++)
        {
            for (int y = 0; y < room.First.GetLength(1); y++)
            {
                if (room.First[x, y] != null)
                {
                    room.First[x, y].SetActive(false);
                }
            }
        }

        for (int i = 0; i < interactiveObjects.Count; i++)
        {
            if (interactiveObjects[i] == null)
            {
                interactiveObjects.Remove(interactiveObjects[i]);
                i--;
                continue;
            }

            if ((room.First[0, 0].transform.position.x < interactiveObjects[i].transform.position.x) &&
                (room.First[0, 0].transform.position.y > interactiveObjects[i].transform.position.y) &&
                (room.First[room.First.GetLength(0) - 1, room.First.GetLength(1) - 1].transform.position.x > interactiveObjects[i].transform.position.x) &&
                (room.First[room.First.GetLength(0) - 1, room.First.GetLength(1) - 1].transform.position.y < interactiveObjects[i].transform.position.y))
            {
                interactiveObjects[i].SetActive(false);
            }
        }

        room.Second = false;
    }
    public void ActiveRoom(Pair<GameObject[,], bool> room)
    {
        for (int x = 0; x < room.First.GetLength(0); x++)
        {
            for (int y = 0; y < room.First.GetLength(1); y++)
            {
                if (room.First[x, y] != null)
                {
                    room.First[x, y].SetActive(true);
                }
            }
        }

        for (int i = 0; i < interactiveObjects.Count; i++)
        {
            if (interactiveObjects[i] == null)
            {
                interactiveObjects.Remove(interactiveObjects[i]);
                i--;
                continue;
            }


            if ((room.First[0, 0].transform.position.x < interactiveObjects[i].transform.position.x) &&
                (room.First[0, 0].transform.position.y > interactiveObjects[i].transform.position.y) &&
                (room.First[room.First.GetLength(0) - 1, room.First.GetLength(1) - 1].transform.position.x > interactiveObjects[i].transform.position.x) &&
                (room.First[room.First.GetLength(0) - 1, room.First.GetLength(1) - 1].transform.position.y < interactiveObjects[i].transform.position.y))
            {
                interactiveObjects[i].SetActive(true);
            }
        }

        room.Second = true;
    }

}

