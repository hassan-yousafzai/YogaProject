using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the room interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IRoomRepository
    {
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        Boolean DeleteRoom(int id);
        List<Room> GetRoomsByLocation();
        List<Room> GetRoomsById(int id);
        Room GetRoomById(int id);
        Room GetRoomByRoomNumber(int id, string roomNumber);
        List<Room> GetRooms();
    }

    /// <summary>
    /// The room Repository class that implements the room interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository()
        {
            _context = new ApplicationDbContext();
        }

        //add a room
        //the room number is converted to title case to be consistent
        public void AddRoom(Room room)
        {
            room.RoomNumber = Helper.Utility.ConvertToTitleCase(room.RoomNumber);
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        //update room, again, the room number is converted to title case
        public void UpdateRoom(Room room)
        {
            room.RoomNumber = Helper.Utility.ConvertToTitleCase(room.RoomNumber);
            var roomInDb = _context.Rooms.Where(r => r.Id == room.Id).Single();

            roomInDb.RoomNumber = room.RoomNumber;
            roomInDb.Capacity = room.Capacity;
            roomInDb.BranchId = room.BranchId;
            _context.SaveChanges();
        }

        //delete a room
        //only delete it if it has no bookings 
        public Boolean DeleteRoom(int id)
        {
            var status = false;
            var roomInDb = _context.Rooms.Where(r => r.Id == id).SingleOrDefault();
            if (roomInDb != null)
            {
                var bookedRoomsInDb = _context.Classes.Where(r => r.RoomId == roomInDb.Id).ToList();

                if (bookedRoomsInDb.Count == 0)
                {
                    _context.Rooms.Remove(roomInDb);
                    _context.SaveChanges();
                    return status = true;
                }
            }
            return status;
        }

        //get list of rooms      
        public List<Room> GetRoomsByLocation()
        {
            return _context.Rooms.ToList();
        }

        //get room by Id
        public Room GetRoomById(int id)
        {
            return _context.Rooms.Where(r => r.Id == id).SingleOrDefault();
        }

        //get list of rooms by id
        public List<Room> GetRoomsById(int id)
        {
            var branchId = GetBranchId(id);
            return _context.Rooms.Where(r => r.BranchId == branchId).ToList();
        }

        //get branch id, by using the room id to query the database
        private int GetBranchId(int id)
        {
            var branchId = _context.Rooms.Where(b => b.Id == id).FirstOrDefault();
            return branchId.BranchId;
        }

        //get room by room number and id
        //roomNumber converted to title case
        public Room GetRoomByRoomNumber(int id, string roomNumber)
        {
            roomNumber = Helper.Utility.ConvertToTitleCase(roomNumber);
            return _context.Rooms.Where(r => r.BranchId == id && r.RoomNumber == roomNumber).FirstOrDefault();
        }

        //get list of rooms
        //also load the branch model with it
        public List<Room> GetRooms()
        {
            var rooms = _context.Rooms.Include(r => r.Branch).ToList();
            return rooms;
        }


    }
}