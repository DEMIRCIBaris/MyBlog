using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Entities.Abstract
{
    public abstract class EntityBase
    {

        public virtual int Id { get; set; }

        //Diğer tablolarda da olabilecek ama değerleri değişebilecek bir değişken oluşturduk
        //override CreatedDate=new Date(2021) şeklinde
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual bool IsActive { get; set; } = true;
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string Note { get; set; }
    }
}
