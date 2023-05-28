using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class RoomManager : BaseGameObject
{
    [field:SerializeField]
    public Room ActiveRoom { get; private set; }
    
    public List<Room> Rooms { get; private set; }

    public override void Init()
    {
        base.Init();
        Rooms = new List<Room>();
    }

    public void AddRoom(Room room)
    {
        Rooms.Add(room);
        if (ActiveRoom == null && room.Active) ActiveRoom = room;
    }

    public void MarkAsActive(Room currentRoom)
    {
        ActiveRoom = currentRoom;
        foreach (var room in Rooms.Where(room => room.Active && room != currentRoom))
        {
            room.DeactivateRoom();
        }
    }
}
