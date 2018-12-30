using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public static List<Shape> Remlist = new List<Shape>();
        public static List<Shape> Editlist = new List<Shape>();
        public static int editID;
        public static string editname;
        public static double editsidelength;
        public static string Searchstr;
        public static string remname;

        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.context = dbContext;
        }

        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel();

            return View(indexViewModel);
        }

        public IActionResult Error()
        {

            return View();
        }

        public IActionResult Result()
        {
            ResultViewModel resultViewModel = new ResultViewModel();

            resultViewModel.Error = "To add a new shape, please return to the 'Add' page.";

            List<Shape> TheList = context.Shapes.ToList();
            resultViewModel.Shapelist = TheList;

            return View(resultViewModel);
        }

        [HttpPost]
        public IActionResult Result(ResultViewModel resultViewModel)

        {
            if ((ModelState.IsValid) & (resultViewModel.Sidelength > 0))
            {

                List<Shape> TheList = context.Shapes.ToList();

                if (resultViewModel.Shapetype == "Cube")
                {

                    Cube Cube = new Cube("Cube", resultViewModel.Sidelength);

                    resultViewModel.Volume = Cube.Volume(resultViewModel.Sidelength);
                    resultViewModel.Surfacearea = Cube.Surfacearea(resultViewModel.Sidelength);

                    context.Shapes.Add(Cube);
                    TheList.Add(Cube);

                }

                if (resultViewModel.Shapetype == "Square")
                {

                    Square Square = new Square("Square", resultViewModel.Sidelength);

                    resultViewModel.Perimeter = Square.Perimeter(resultViewModel.Sidelength);
                    resultViewModel.Area = Square.Area(resultViewModel.Sidelength);
                    context.Shapes.Add(Square);
                    TheList.Add(Square);

                }

                if (resultViewModel.Shapetype == "Segment")
                {

                    Segment Segment = new Segment("Segment", resultViewModel.Sidelength);
                    context.Shapes.Add(Segment);
                    TheList.Add(Segment);

                }

                context.SaveChanges();
                resultViewModel.Shapelist = TheList;

                return View(resultViewModel);


            }


            return Redirect("/Home/Error");

        }

        [HttpGet]
        public IActionResult Remove()
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0)
            {
                RemoveViewModel removeViewModel = new RemoveViewModel();

                removeViewModel.TheList = TheList;

                return View(removeViewModel);
            }

            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult Remove(RemoveViewModel removeViewModel)

        {
            List<Shape> TheList = context.Shapes.ToList();

            if (ModelState.IsValid)
            {

                TheList.RemoveAll(x => x.ID == removeViewModel.RemshapeID);
                Shape remshape = context.Shapes.Single(c => c.ID == removeViewModel.RemshapeID);
                context.Shapes.Remove(remshape);
                context.SaveChanges();

                Remlist.Clear();

                return Redirect("/Home/Result");
            }

            return Redirect("/Home/Error");

        }

        [HttpGet]
        public IActionResult EditSelect()
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0)
            {
                EditSelectViewModel editSelectViewModel = new EditSelectViewModel();

                editSelectViewModel.TheList = TheList;

                return View(editSelectViewModel);
            }

            else
            {
                return Redirect("/");
            }
        }


        [HttpPost]
        public IActionResult EditSelect(EditSelectViewModel editSelectViewModel)
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (ModelState.IsValid)
            {
                Shape editshape = context.Shapes.Single(c => c.ID == editSelectViewModel.EditshapeID);
                editID = editSelectViewModel.EditshapeID;
                editname = editshape.Name;
                editsidelength = editshape.Sidelength;
                context.SaveChanges();
               

                return Redirect("/Home/EditItem");
            }

            return Redirect("/Home/Error");

        }



        [HttpGet]
        public IActionResult EditItem()
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0)
            {
                EditItemViewModel editItemViewModel = new EditItemViewModel();

                ViewBag.Name = editname;
                ViewBag.Sidelength = editsidelength;

                return View(editItemViewModel);
            }

            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult EditItem(EditItemViewModel editItemViewModel)

        {
            List<Shape> TheList = context.Shapes.ToList();

            if ((ModelState.IsValid) & (editItemViewModel.NewElement2 > 0))

            {
                Shape editshape = context.Shapes.Single(c => c.ID == editID);
                editshape.Sidelength = editItemViewModel.NewElement2;
                context.SaveChanges();
                
                return Redirect("/Home/Result");
            }
            
            return Redirect("/Home/Error");

        }

        [HttpGet]
        public IActionResult SearchSelect()
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0)
            {
                SearchSelectViewModel searchSelectViewModel = new SearchSelectViewModel();

                return View(searchSelectViewModel);
            }

            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult SearchSelect(SearchSelectViewModel searchSelectViewModel)

        {
            if (ModelState.IsValid)

            {
                Searchstr = searchSelectViewModel.Searchstr.ToLower();
                return Redirect("/Home/SearchResult");
            }

            return Redirect("/Home/Error");

        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            List<Shape> TheList = context.Shapes.ToList();

            if (TheList.Count > 0)
            {
                SearchResultViewModel searchResultViewModel = new SearchResultViewModel();
                List<Shape> Anslist = new List<Shape>();

                foreach (Shape item in TheList)
                {
                    string itemname = item.Name.ToLower();

                    if (itemname.Contains(Searchstr))
                    {

                        Anslist.Add(item);

                    }


                }

                if (Anslist.Count == 0)
                {
                    Shape errshape = new Shape("That search returned no results", 0);

                    Anslist.Add(errshape);

                }

                ViewBag.Anslist = Anslist;

                return View(searchResultViewModel);
            }

            else
            {
                return Redirect("/");
            }

        }

        [HttpGet]
        public IActionResult Sort()
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0)
            {
                SortViewModel sortViewModel = new SortViewModel();

                List<Shape> Seglist = new List<Shape>();
                List<Shape> Sqlist = new List<Shape>();
                List<Shape> Cublist = new List<Shape>();

                foreach (Shape item in TheList)
                {
                    if (item.Name == "Segment")
                    {

                        Seglist.Add(item);

                    }

                    if (item.Name == "Square")
                    {

                        Sqlist.Add(item);

                    }

                    if (item.Name == "Cube")
                    {

                        Cublist.Add(item);

                    }


                }

                List<Shape> Seglisto = Seglist.OrderBy(x => x.Sidelength).ToList();
                List<Shape> Sqlisto = Sqlist.OrderBy(x => x.Sidelength).ToList();
                List<Shape> Cublisto = Cublist.OrderBy(x => x.Sidelength).ToList();

                List<Shape> Bridgelist = new List<Shape>();

                foreach (Shape item in Cublisto)
                {

                    Bridgelist.Add(item);

                }

                foreach (Shape item in Seglisto)
                {

                    Bridgelist.Add(item);

                }

                foreach (Shape item in Sqlisto)
                {

                    Bridgelist.Add(item);

                }



                sortViewModel.Sortlist = Bridgelist;

                return View(sortViewModel);
            }

            else
            {
                return Redirect("/");
            }

        }

    }

}