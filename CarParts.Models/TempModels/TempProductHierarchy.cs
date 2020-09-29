using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.TempModels
{
    public class TempProductHierarchy
    {
        public bool Model { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Banner { get; set; }
        public List<TempProductHierarchy> TempProductHierarchyList { get; set; }
    }
}
