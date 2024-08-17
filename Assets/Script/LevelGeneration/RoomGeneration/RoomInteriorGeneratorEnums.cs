using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomInteriorGeneratorTag
{
    public enum Surface
    {
        Emptiness,
        Floor,
        Wall,
        Door,
        StaticObject,
        TriggerObject,
        StorageObject,

        UniversalObject
    }

    public enum Interactive
    {
        Emptiness,
        StaticObject,
        NonStaticObject,
        PickUp,

        PassiveEnemy,
        ActiveEnemy
    }

    public enum RoomShape
    {
        Rectangle,

    }

    public enum Direction
    {
        Top,
        Left,
        Right,
        Bottom
    }
}
