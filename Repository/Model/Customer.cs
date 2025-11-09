using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class Customer : ModelBase
    {
        /// <summary>
        /// 姓
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string FirstName { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        [MaxLength(10)]
        public string? LastName { get; set; }
        /// <summary>
        /// 是否开发者
        /// </summary>
        public bool IsDeveloper { get; set; } = false;
    }
}
