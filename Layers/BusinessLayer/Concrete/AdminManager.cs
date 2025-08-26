using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AdminManager : IAdminService
    {
        IAdminDal _admindal;

        public AdminManager(IAdminDal admindal)
        {
            _admindal = admindal;
        }

        public void AdminAdd(Admin admin)
        {
            _admindal.Add(admin);
        }

        public void AdminDelete(Admin admin)
        {
            _admindal.Delete(admin);
        }

        public void AdminUpdate(Admin admin)
        {
            _admindal.Update(admin);
        }

        public Admin GetAdminByID(int id)
        {
           return _admindal.Get(x => x.AdminID == id);
        }

        public Admin GetAdminByInfo(Admin admin)
        {
            var adminFromDb = _admindal.Get(x => x.AdminName == admin.AdminName && x.AdminPassword == admin.AdminPassword);
            return adminFromDb;
        }

        public List<Admin> GetList()
        {
            return _admindal.List();
        }
    }
}
