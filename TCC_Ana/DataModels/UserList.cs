using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    public class UserList
    {
        [Key]
        public string UserId { get; set; }

    }

}