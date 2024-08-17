using Microsoft.Win32;
using RoomInteriorGeneratorTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomInteriorGeneratorTag
{
    public class Door
    {
        public int x, y, width, height;
        public PointDouble center;
        public Direction direction;

        public Door(int x, int y, int Lenght, Direction direction)
        {
            this.x = x;
            this.y = y;

            switch (direction)
            {
                case Direction.Left: case Direction.Right:
                    this.width = 1;
                    this.height = Lenght;
                    break;
                case Direction.Top: case Direction.Bottom:
                    this.width = Lenght;
                    this.height = 1;
                    break;
            }

            this.center.x = (double)x + (double)(width-1) / 2;
            this.center.y = (double)y + (double)(height-1) / 2;

            this.direction = direction;
        }
    }


    public class ObjectSite
    {
        public int y, x, width, height; // положение и размер этого листа
        public Surface typeSite;

        public ObjectSite(int X, int Y, int Width, int Height, Surface TypeSite = Surface.UniversalObject)
        {
            typeSite = TypeSite;

            x = X;
            y = Y;
            width = Width;
            height = Height;
        }

        static public Pair<ObjectSite, ObjectSite>? split(ObjectSite objectSite, int minObjectSiteSize, int maxObjectSiteSize)
        {
            Random random = new Random();
            // начинаем разрезать лист на два дочерних листа
            if ((objectSite.width < minObjectSiteSize) && (objectSite.height < minObjectSiteSize))
                return null; // мы уже его разрезали! прекращаем!

            // определяем направление разрезания
            // если ширина более чем на 50% больше высоты, то разрезаем вертикально
            // если высота более чем на 50% больше ширины, то разрезаем горизонтально
            // иначе выбираем направление разрезания случайным образом
            bool splitH = random.NextDouble() > 0.5;
            if (objectSite.width > objectSite.height && objectSite.width / objectSite.height >= 1.25)
                splitH = false;
            else if (objectSite.height > objectSite.width && objectSite.height / objectSite.width >= 1.25)
                splitH = true;

            int max = (splitH ? objectSite.height : objectSite.width) - minObjectSiteSize; // определяем максимальную высоту или ширину
            if (max <= minObjectSiteSize)
                return null; // область слишком мала, больше её делить нельзя...

            int split = random.Next(minObjectSiteSize, max); // определяемся, где будем разрезать

            // создаём левый и правый дочерние листы на основании направления разрезания
            if (splitH)
            {
                return new Pair <ObjectSite, ObjectSite> (
                    new ObjectSite(objectSite.x, objectSite.y, objectSite.width, split), 
                    new ObjectSite(objectSite.x, objectSite.y + split, objectSite.width, objectSite.height - split));
            }
            else
            {
                return new Pair<ObjectSite, ObjectSite>(
                    new ObjectSite(objectSite.x, objectSite.y, split, objectSite.height),
                    new ObjectSite(objectSite.x + split, objectSite.y, objectSite.width - split, objectSite.height));
            }
        }

        public void createRooms()
        {
            Random random = new Random();

            x += 1; y += 1;
            width -= 2; height -= 2;

            // этот лист готов к созданию комнаты
            //Point roomSize;
            //Point roomPos;
            // размер комнаты может находиться в промежутке от 3 x 3 тайла до размера листа - 2.
            //roomSize = new Point(random.Next(2, width - 2), random.Next(2, height - 2));
            // располагаем комнату внутри листа, но не помещаем её прямо рядом со стороной листа (иначе комнаты сольются)
            //roomPos = new Point(random.Next(1, width - roomSize.x - 1), random.Next(1, height - roomSize.y - 1));

            //x += random.Next(1, width - roomSize.x - 1);
            //y += random.Next(1, height - roomSize.y - 1);

            //width = random.Next(2, width - 2);
            //height = random.Next(2, height - 2);
            //room = new Rectangle(x + roomPos.x, y + roomPos.y, roomSize.x, roomSize.y);
        }
    }
}
