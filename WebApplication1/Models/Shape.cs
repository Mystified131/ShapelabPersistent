using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Shape
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public double Sidelength { get; set; }


        public Shape(string name, double sidelength)
        {
            Name = name;
            Sidelength = sidelength;
        }

        public double Volume(double Sidelength)
        {

            double Vol = Sidelength * Sidelength * Sidelength;

            return Vol;

        }

        public double Surfacearea(double Sidelength)
        {

            double Sar = Sidelength * Sidelength * 6;

            return Sar;

        }

        public double Perimeter(double Sidelength)
        {

            double Per = Sidelength * 4;

            return Per;

        }

        public double Area(double Sidelength)
        {

            double Are = Sidelength * Sidelength;

            return Are;

        }
    }

    public class Cube : Shape
    {

        public Cube(string Name, double Sidelength) : base(Name, Sidelength) { }

    }

    public class Square : Shape
    {


        public Square(string Name, double Sidelength) : base(Name, Sidelength) { }

       
    }

    public class Segment : Shape
    {

        public Segment(string Name, double Sidelength) : base(Name, Sidelength) { }
    }

}
