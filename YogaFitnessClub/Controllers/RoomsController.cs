using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a rooms controller that handles everything about rooms e.g all the CRUD operations 
    /// This whole controller is only restricted to admin but some methods allowed to tutor
    /// This controller utilises the RoomRepository to complete all its tasks
    /// </summary>
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBranchRepository _branchRepository;

        public RoomsController()
        {
            _roomRepository = new RoomRepository();
            _branchRepository = new BranchRepository();
        }

        //renders the index view of the rooms 
        //sends list of rooms to the view
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var listOfRooms = _roomRepository.GetRooms();
            return View(listOfRooms);
        }

        //shows the room form
        //sends the roomviewmodel to the room form view
        [Authorize(Roles = "Admin")]
        public ActionResult RoomForm()
        {
            var branches = _branchRepository.GetBranches();

            var viewModel = new RoomViewModel
            {
                Branches = branches
            };
            return View(viewModel);
        }

        //saves or edits a room 
        //all the validation has been considered 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveRoom(Room room)
        {
            room.RoomNumber = Regex.Replace(room.RoomNumber, @"\s+", " ");//remove multiple whitespaces 
            if (room.Id == 0)
            {
                var checkRoom = CheckIfRoomExist(room);

                if (checkRoom != null)
                {
                    ViewData["Message"] = "Room already exist";
                    var branches = _branchRepository.GetBranches();
                    var viewModel = new RoomViewModel
                    {
                        Branches = branches,
                        NotEmptyModel = true
                    };
                    return View("RoomForm", viewModel);
                }
                else
                {
                    _roomRepository.AddRoom(room);
                    return RedirectToAction("Index", "Rooms");
                }
            }
            else
            {
                var roomInDb = _roomRepository.GetRoomById(room.Id);

                if (roomInDb.RoomNumber == room.RoomNumber
                    && roomInDb.Capacity == room.Capacity
                    && roomInDb.BranchId == room.BranchId)
                {
                    ViewData["Message"] = "You did not update any fields!";
                    var branches = _branchRepository.GetBranches();
                    var viewModel = new RoomViewModel
                    {
                        Branches = branches,
                        NotEmptyModel = true
                    };

                    return View("RoomForm", viewModel);
                }

                var checkRoomInDb = _roomRepository.GetRoomByRoomNumber(room.BranchId, room.RoomNumber);

                if (checkRoomInDb == null)
                {
                    _roomRepository.UpdateRoom(room);
                    return RedirectToAction("Index", "Rooms");
                }
                else
                {
                    ViewData["Message"] = "Another room with the same name exist in this branch!";
                    var branches = _branchRepository.GetBranches();
                    var viewModel = new RoomViewModel
                    {
                        Branches = branches,
                        NotEmptyModel = true
                    };
                    return View("RoomForm", viewModel);
                }
            }
        }

        //returnds the room forms to edit a room by using the room id
        [Authorize(Roles = "Admin")]
        public ActionResult EditRoom(int id)
        {
            var roomInDb = _roomRepository.GetRoomById(id);
            var viewModel = new RoomViewModel
            {
                Branches = _branchRepository.GetBranches(),
                Room = roomInDb
            };

            if (roomInDb != null)
                return View("RoomForm", viewModel);
            else
                return RedirectToAction("Index", "Rooms");
        }

        //deletes a room or if it has been used i.e. customers have bookings booked in a room
        //then that room cannot be deleted
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var status = _roomRepository.DeleteRoom(id);
            if (status == true)
                return RedirectToAction("Index", "Rooms");
            else
            {
                ViewData["Message"] = "This room cannot be deleted as it has existing bookings!";
                var listOfRooms = _roomRepository.GetRooms();
                return View("Index", listOfRooms);
            }
        }

        //checks if a room exist
        private Room CheckIfRoomExist(Room room)
        {
            return _roomRepository.GetRoomByRoomNumber(room.BranchId, room.RoomNumber);
        }

        //returns a rooms 
        //sends it back in json formart
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult FetchRoomById(int id)
        {
            var branchId = _roomRepository.GetRoomsById(id);
            return new JsonResult { Data = branchId, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //returns all rooms by a branch id
        //sends it back in json formart
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult FetchRoomsByLocation(int id)
        {
            var rooms = _roomRepository.GetRoomsByLocation()
                .Where(r => r.BranchId == id);
            return new JsonResult { Data = rooms, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //returns a room to check its capacity
        //sends it back in json formart
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult FetchRoomForCapacity(int id)
        {
            var rooms = _roomRepository.GetRoomById(id);
            return new JsonResult { Data = rooms, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

    }
}