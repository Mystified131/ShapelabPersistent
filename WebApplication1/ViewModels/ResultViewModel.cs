using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ResultViewModel
    {
        [Required]
        public string Shapetype { get; set; }
        [Required]
        public double Sidelength { get; set; }
        public double Volume { get; set; }
        public double Surfacearea { get; set; }
        public double Perimeter { get; set; }
        public double Area { get; set; }
        public List<Shape> Shapelist { get; set; }
        public String Error { get; set; }
    }
}
