using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class EditSelectViewModel
    {
        [Required]
        public double EditshapeID { get; set; }
        public List<Shape> TheList { get; set; }
    }
}
