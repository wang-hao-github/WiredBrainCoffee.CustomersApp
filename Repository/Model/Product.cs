using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    [Table("products")]
    public class Product:ModelBase
    {
        [MaxLength(20)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
