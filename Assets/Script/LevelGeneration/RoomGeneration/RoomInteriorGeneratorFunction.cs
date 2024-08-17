using System;

namespace RoomInteriorGeneratorTag
{
    public partial class Room
    {
        private void RectangleFloorGenerate()
        {
            DrawFunction.FillRectangle(roomObjects, 0, 0, roomObjects.GetLength(0) - 1, roomObjects.GetLength(1) - 1, Surface.Floor);
        } //TODO: Сделать алгоритм универсального заполнения

        private void RectangleWallGenerate()
        {
            for (int x = 0; x < roomObjects.GetLength(0); x++)
            {
                roomObjects[x, 0] = Surface.Wall;
                roomObjects[x, roomObjects.GetLength(1) - 1] = Surface.Wall;
            }
            for (int y = 0; y < roomObjects.GetLength(1); y++)
            {
                roomObjects[0, y] = Surface.Wall;
                roomObjects[roomObjects.GetLength(0) - 1, y] = Surface.Wall;
            }
        }

        private void RectangleDoorGenerate(Direction direction, int DoorLenght) //Функция упадет, есть размер двери не уложиться в (размер стены - 4)
        {
            Random random = new Random();   
            int x = 0, y = 0;

            switch (InvertDirection(direction))
            {
                case Direction.Left:
                    x = 0; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                    break;
                case Direction.Right:
                    x = roomObjects.GetLength(0) - 1; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                    break;
                case Direction.Top:
                    x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = 0;
                    break;
                case Direction.Bottom:
                    x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = roomObjects.GetLength(1) - 1;
                    break;
            }
            FirstDoor.x = x; FirstDoor.y = y;   
            doors.Add(new Door(x, y, DoorLenght, InvertDirection(direction)));

            if ((direction != Direction.Left) && (random.NextDouble() > 0.5))
            {
                x = roomObjects.GetLength(0) - 1; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                doors.Add(new Door(x, y, DoorLenght, InvertDirection(Direction.Left)));
            }
            if ((direction != Direction.Right) && (random.NextDouble() > 0.5))
            {
                x = 0; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                doors.Add(new Door(x, y, DoorLenght, InvertDirection(Direction.Right)));
            }
            if ((direction != Direction.Top) && (random.NextDouble() > 0.5))
            {
                x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = roomObjects.GetLength(1) - 1;
                doors.Add(new Door(x, y, DoorLenght, InvertDirection(Direction.Top)));
            }
            if ((direction != Direction.Bottom) && (random.NextDouble() > 0.5))
            {
                x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = 0;
                doors.Add(new Door(x, y, DoorLenght, InvertDirection(Direction.Bottom)));
            }

            if (doors.Count < 2)
            {
                switch (direction)
                {
                    case Direction.Left:
                        x = 0; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                        break;
                    case Direction.Right:
                        x = roomObjects.GetLength(0) - 1; y = random.Next(2, roomObjects.GetLength(1) - DoorLenght - 1);
                        break;
                    case Direction.Top:
                        x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = 0;
                        break;
                    case Direction.Bottom:
                        x = random.Next(2, roomObjects.GetLength(0) - DoorLenght - 1); y = roomObjects.GetLength(1) - 1;
                        break;
                }

                doors.Add(new Door(x, y, DoorLenght, direction));
            }

            foreach (Door l in doors)
            {
                DrawFunction.FillRectangle(roomObjects, l.x, l.y, l.width, l.height, Surface.Door);
            }
        }

        public static Direction InvertDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Bottom:
                    return Direction.Top;
                default:
                    break;
            }
            return 0;
        }

        private void RectangleObjectSiteGenerate()
        {
            Random random = new Random();

            objectSites.Add(new ObjectSite(1, 1, roomObjects.GetLength(0) - 2, roomObjects.GetLength(1) - 2));

            int Index = 0;
            while (Index < objectSites.Count)
            {
                if ((objectSites[Index].width > minObjectSiteSize) || (objectSites[Index].height > minObjectSiteSize))
                {
                    if ((objectSites[Index].width > maxObjectSiteSize) || 
                        (objectSites[Index].height > maxObjectSiteSize) || 
                        (random.NextDouble() > 0.10))
                    {
                        Pair<ObjectSite, ObjectSite>? splitReturn = ObjectSite.split(objectSites[Index], minObjectSiteSize, maxObjectSiteSize);

                        if (splitReturn == null) { Index++; } 
                        else
                        {
                            objectSites.Remove(objectSites[Index]);
                            objectSites.Add(splitReturn.Value.First);
                            objectSites.Add(splitReturn.Value.Second);
                        }
                    } 
                    else { Index++; }
                } 
                else { Index++; }
            }
            
            foreach (ObjectSite l in objectSites)
            {
                //DrawFunction.FillRectangle(roomObjects, l.x, l.y, l.width - 1, l.height - 1, Surface.StaticObject);
                l.createRooms();
                DrawFunction.FillRectangle(roomObjects, l.x, l.y, l.width, l.height, l.typeSite);
            }
        }

        private void RectangleTriggerGenerate(int width, int height)
        {
            for (int i = objectSites.Count - 1; i > 0; i--)
            {
                if ((width <= objectSites[i].width) && (height <= objectSites[i].height))
                {
                    objectSites[i].typeSite = Surface.TriggerObject;
                    DrawFunction.FillRectangle(roomObjects, objectSites[i].x, objectSites[i].y,
                        objectSites[i].width, objectSites[i].height, Surface.Floor);

                    objectSites[i].width = width; objectSites[i].height = height;
                    DrawFunction.FillRectangle(roomObjects, objectSites[i].x, objectSites[i].y,
                        objectSites[i].width, objectSites[i].height, objectSites[i].typeSite);
                    return;
                }
            }
        }

        private void RectangleStaticObjectGenerate()
        {
            int minStaticObjectSize = 1;
            int maxStaticObjectSize = 3;
            Random random = new Random();

            int Index = 0;
            while (Index < objectSites.Count)
            {
                if (objectSites[Index].typeSite == Surface.TriggerObject) { Index++; continue; }

                if ((objectSites[Index].width > minStaticObjectSize) || (objectSites[Index].height > minStaticObjectSize)) 
                {
                    if ((objectSites[Index].width > maxStaticObjectSize) ||
                        (objectSites[Index].height > maxStaticObjectSize) ||
                        (random.NextDouble() > 0.10))
                    {
                        Pair<ObjectSite, ObjectSite>? splitReturn = ObjectSite.split(objectSites[Index], minStaticObjectSize, maxStaticObjectSize);

                        if (splitReturn == null) 
                        { 
                            objectSites[Index].typeSite = RandomSiteInterpritation(random.NextDouble());
                            Index++;
                        }
                        else
                        {
                            objectSites.Remove(objectSites[Index]);
                            splitReturn.Value.First.typeSite = RandomSiteInterpritation(random.NextDouble());
                            objectSites.Add(splitReturn.Value.First);
                            splitReturn.Value.Second.typeSite = RandomSiteInterpritation(random.NextDouble());
                            objectSites.Add(splitReturn.Value.Second);
                        }
                    }
                    else 
                    { 
                        objectSites[Index].typeSite = RandomSiteInterpritation(random.NextDouble());
                        Index++;
                    }
                }
                else 
                { 
                    objectSites[Index].typeSite = RandomSiteInterpritation(random.NextDouble());
                    Index++;
                }
            }

            foreach (ObjectSite l in objectSites)
            {
                DrawFunction.FillRectangle(roomObjects, l.x, l.y, l.width, l.height, l.typeSite);
            }
        }

        private Surface RandomSiteInterpritation(double random)
        {
            if (random > 0.8)
            {
                return Surface.StorageObject;
            }
            else if (random > 0.33)
            {
                return Surface.StaticObject;
            }
            else
            {
                return Surface.Floor;
            }
        }

        private void RectangleInteriorObjectGenerate()
        {
            Random random = new Random();

            for (int x = 0; x < roomObjects.GetLength(0); x++)
            {
                for(int y = 0; y < roomObjects.GetLength(1); y++)
                {
                    if (roomObjects[x, y] == Surface.Floor)
                    {
                        double temp = random.NextDouble();
                        if (NeighborCount(x, y, Surface.Door) == 0)
                        {
                            if (temp > 0.95)
                            {
                                interactiveObjects[x, y] = Interactive.NonStaticObject;
                            }
                            else if (temp > 0.90)
                            {
                                interactiveObjects[x, y] = Interactive.StaticObject;
                            }
                            else if (temp > 0.85)
                            {
                                interactiveObjects[x, y] = Interactive.PassiveEnemy;
                            }
                            else if (temp > 0.82)
                            {
                                interactiveObjects[x, y] = Interactive.ActiveEnemy;
                            }
                        }
                    }
                    if (roomObjects[x, y] == Surface.Wall)
                    {

                    }
                    if (roomObjects[x, y] == Surface.StorageObject)
                    {
                        double temp = random.NextDouble();
                        if (NeighborCount(x, y, Surface.Floor) == 0) 
                        {
                            roomObjects[x, y] = Surface.StaticObject;
                        } 
                        else if (temp > 0.75)
                        {
                            interactiveObjects[x, y] = Interactive.PickUp;
                        }
                        else if (temp > 0.60)
                        {
                            interactiveObjects[x, y] = Interactive.StaticObject;
                        }
                    }
                    if (roomObjects[x, y] == Surface.StaticObject)
                    {
                        if (random.NextDouble() > 0.70)
                        {
                            interactiveObjects[x, y] = Interactive.StaticObject;
                        }
                    }
                }
            }
        }

        public int NeighborCount(int x, int y, Surface neighbor)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for( int j = y - 1; j <= y + 1; j++)
                {
                    if (roomObjects[i, j] == neighbor) { count++; }
                }    
            }
            return count;
        }


        public static class DrawFunction
        {
            public static void FillRectangle<U>(U[,] Array, int x, int y, int width, int height, U newNumber)
            {
                for (int i = x; i < x + width; i++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        Array[i, j] = newNumber;
                    }
                }
            }
        }
    }
}
