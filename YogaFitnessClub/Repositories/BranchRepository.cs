using System;
using System.Collections.Generic;
using System.Linq;
using YogaFitnessClub.Helper;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the branch interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IBranchRepository
    {
        void AddBranch(Branch branch);
        void UpdateBranch(Branch branch);
        Boolean DeleteBranch(int id);
        List<Branch> GetBranches();
        Branch GetBranchByAddress(string address);
        Branch GetBranchById(int id);
    }

    /// <summary>
    /// The branch Repository class that implements the branch interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository()
        {
            _context = new ApplicationDbContext();
        }

        //gets a branch by id from the branches table
        public Branch GetBranchById(int id)
        {
            return _context.Branches.SingleOrDefault(b => b.Id == id);
        }

        //gets the branch by address from branches table
        public Branch GetBranchByAddress(string address)
        {
            return _context.Branches.Where(b => b.Address == address).SingleOrDefault();
        }

        //adds a branch to the branches table
        //the location name and address is converted to title case
        //the postcode is converted to upper case
        public void AddBranch(Branch branch)
        {
            branch.LocationName = Utility.ConvertToTitleCase(branch.LocationName);
            branch.Address = Utility.ConvertToTitleCase(branch.Address);
            branch.Postcode = branch.Postcode.ToUpper();

            _context.Branches.Add(branch);
            _context.SaveChanges();
        }

        //gets all the branches from the branches table
        public List<Branch> GetBranches()
        {
            return _context.Branches.ToList();
        }

        //updates a branch
        public void UpdateBranch(Branch branch)
        {
            var branchInDb = _context.Branches.Where(b => b.Id == branch.Id).Single();

            branchInDb.LocationName = branch.LocationName;
            branchInDb.Address = branch.Address;
            branchInDb.MobileNumber = branch.MobileNumber;
            branchInDb.Email = branch.Email;

            _context.SaveChanges();
        }

        //deletes a branch
        //it can only be deleted if the branch does not have any rooms 
        public Boolean DeleteBranch(int id)
        {
            var status = false;
            var branchInDb = _context.Branches.Where(b => b.Id == id).SingleOrDefault();

            if (branchInDb != null)
            {
                var branchRoomsInDb = _context.Rooms.Where(r => r.BranchId == branchInDb.Id).ToList();

                if (branchRoomsInDb.Count == 0)
                {
                    _context.Branches.Remove(branchInDb);
                    _context.SaveChanges();
                    return status = true;
                }
            }
            return status;
        }
    }
}