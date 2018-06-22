using System;
using System.Collections.Generic;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the ClassType interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IClassTypeRepository
    {
        void AddClassType(ClassType classType);
        void UpdateClassType(ClassType classType);
        Boolean DeleteClassType(int id);
        ClassType GetClassTypeByName(string name);
        List<ClassType> GetClassTypes();
        ClassType GetClassType(int id);
        ClassType GetClassTypeById(int id);
    }

    /// <summary>
    /// The ClassType Repository class that implements the ClassType interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class ClassTypeRepository : IClassTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassTypeRepository()
        {
            _context = new ApplicationDbContext();
        }

        //get classtype by id
        public ClassType GetClassTypeById(int id)
        {
            return _context.ClassTypes.SingleOrDefault(c => c.Id == id);
        }

        public ClassType GetClassType(int id)
        {
            return _context.ClassTypes.Where(i => i.Id == id).FirstOrDefault();
        }

        //get list of classtype
        public List<ClassType> GetClassTypes()
        {
            return _context.ClassTypes.ToList();
        }

        //add classtype to classtypes table
        //classTypeName is converted to title case
        public void AddClassType(ClassType classType)
        {
            classType.ClassTypeName = Helper.Utility.ConvertToTitleCase(classType.ClassTypeName);

            _context.ClassTypes.Add(classType);
            _context.SaveChanges();
        }

        //update class type
        public void UpdateClassType(ClassType classType)
        {
            classType.ClassTypeName = Helper.Utility.ConvertToTitleCase(classType.ClassTypeName);
            var classTypeInDb = _context.ClassTypes.Where(c => c.Id == classType.Id).SingleOrDefault();
            if (classTypeInDb != null)
            {
                classTypeInDb.ClassTypeName = classType.ClassTypeName;
                classTypeInDb.Price = classType.Price;

                _context.SaveChanges();
            }
        }

        //delete a classtype
        //it can only be deleted if it has not been used in classes
        public bool DeleteClassType(int id)
        {
            var status = false;
            var classTypeInDb = _context.ClassTypes.Where(c => c.Id == id).SingleOrDefault();

            if (classTypeInDb != null)
            {
                var classTypesInClasses = _context.Classes.Where(c => c.ClassTypeId == classTypeInDb.Id).ToList();

                if (classTypesInClasses.Count == 0)
                {
                    _context.ClassTypes.Remove(classTypeInDb);
                    _context.SaveChanges();
                    return status = true;
                }
            }
            return status;
        }

        //get classtype by name
        //the parameter sent here is converted to title case as the classTypeName 
        //is stored in that format
        public ClassType GetClassTypeByName(string name)
        {
            name = Helper.Utility.ConvertToTitleCase(name);
            return _context.ClassTypes.Where(b => b.ClassTypeName == name).FirstOrDefault();
        }
    }
}