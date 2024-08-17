using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine;


namespace RoomInteriorGeneratorTag
{
    public partial class Room
    {

        public List<Door> doors; 
        public Surface[,] roomObjects;
        public List<ObjectSite> objectSites;
        public Interactive[,] interactiveObjects;

        public Point UpperLeftCorner { get; private set; }
        public Point UpperRightCorner { get; private set; }
        public Point LowerLeftCorner { get; private set; }
        public Point LowerRightCorner { get; private set; }
        public PointDouble Center { get; private set; }
        public PointDouble FirstDoor;

        private int maxObjectSiteSize;
        private int minObjectSiteSize;

        public Room(RoomShape roomShape, int Width, int Height, int MaxObjectSiteSize, int MinObjectSiteSize
            , int TriggerWidth, int TriggerHeight, int DoorSize, Direction direction)
        {
            if (Width < 5 || Height < 5) { throw new Exception("The generator does not support such small rooms"); }

            doors = new List<Door>();
            roomObjects = new Surface[Width, Height];
            objectSites = new List<ObjectSite>();
            interactiveObjects = new Interactive[Width, Height];

            UpperLeftCorner = new Point(0, 0);
            UpperRightCorner = new Point(Width - 1, 0);
            LowerLeftCorner = new Point(0, Height - 1);
            LowerRightCorner = new Point(Width - 1, Height - 1);

            Center = new PointDouble(Width / 2, Height / 2);

            maxObjectSiteSize = MaxObjectSiteSize;
            minObjectSiteSize = MinObjectSiteSize;

            FloorGenerator(roomShape);

            WallGenerator(roomShape);
            DoorGenerator(roomShape, DoorSize, direction);

            ObjectSiteGenerator(roomShape);
            TriggerGenerator(roomShape, TriggerWidth, TriggerHeight);

            StaticObjectGenerator(roomShape);

            InteriorObjectGenerator(roomShape); 
        }

        private void FloorGenerator(RoomShape roomShape)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleFloorGenerate();
                    break;
            }
        }

        private void WallGenerator(RoomShape roomShape)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleWallGenerate();
                    break;
            }
        }

        private void DoorGenerator(RoomShape roomShape, int DoorSize, Direction direction)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleDoorGenerate(direction, DoorSize);
                    break;
            }
        }

        private void ObjectSiteGenerator(RoomShape roomShape)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleObjectSiteGenerate();
                    break;
            }
        }

        private void TriggerGenerator(RoomShape roomShape, int TriggerWidth, int TriggerHeight)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleTriggerGenerate(TriggerWidth, TriggerHeight);
                    break;
            }
        }

        private void StaticObjectGenerator(RoomShape roomShape)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleStaticObjectGenerate();
                    break;
            }
        }

        private void InteriorObjectGenerator(RoomShape roomShape)
        {
            switch (roomShape)
            {
                case RoomShape.Rectangle:
                    RectangleInteriorObjectGenerate();
                    break;
            }
        }
    }

    //Direction InvertDirection(Direction direction)
    //{
    //    switch (direction)
    //    {
    //        case Direction.Top:
    //            return Direction.Bottom;
    //        case Direction.Left:
    //            return Direction.Right;
    //        case Direction.Right:
    //            return Direction.Left;
    //        case Direction.Bottom:
    //            return Direction.Top;
    //        default:
    //            break;
    //    }
    //    return 0;
    //}

    //struct Door
    //{
    //    Direction direction;
    //    Pair<int, int> coord;
    //    GameObject door;
    //}
}
