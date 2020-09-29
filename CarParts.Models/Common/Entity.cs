using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Common
{
    public class Entity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
