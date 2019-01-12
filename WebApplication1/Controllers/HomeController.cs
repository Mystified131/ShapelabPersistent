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

        public IActionResult Catalog()
        {
            CatalogViewModel catalogViewModel = new CatalogViewModel();

            List<Shape> TheList = context.Shapes.ToList();
            catalogViewModel.TheList = TheList;

            return View(catalogViewModel);
        }

        [HttpPost]
        public IActionResult Catalog(CatalogViewModel catalogViewModel)

        {
            if ((ModelState.IsValid) & (catalogViewModel.Sidelength > 0))
            {

                List<Shape> TheList = context.Shapes.ToList();

                if (catalogViewModel.Shapetype == "Cube")
                {

                    Cube Cube = new Cube("Cube", catalogViewModel.Sidelength);

                    context.Shapes.Add(Cube);
                    TheList.Add(Cube);

                }

                if (catalogViewModel.Shapetype == "Square")
                {

                    Square Square = new Square("Square", catalogViewModel.Sidelength);

                    context.Shapes.Add(Square);
                    TheList.Add(Square);

                }

                if (catalogViewModel.Shapetype == "Segment")
                {

                    Segment Segment = new Segment("Segment", catalogViewModel.Sidelength);

                    context.Shapes.Add(Segment);
                    TheList.Add(Segment);

                }

                context.SaveChanges();
                catalogViewModel.TheList = TheList;

                return View(catalogViewModel);


            }


            return Redirect("/Home/Error");

        }


        public IActionResult Result()
        {
            ResultViewModel resultViewModel = new ResultViewModel();

            resultViewModel.Error = "To see results, please visit the 'Calculate' page.";

            return View(resultViewModel);
        }


        [HttpPost]
        public IActionResult Result(ResultViewModel resultViewModel)
        {
            List<Shape> TheList = context.Shapes.ToList();
            if (ModelState.IsValid)
            {
                Shape calcshape = context.Shapes.Single(c => c.ID == resultViewModel.CalcshapeID);

                if (calcshape.Name == "Cube") {


                ViewBag.Volume = calcshape.Volume(calcshape.Sidelength);
                ViewBag.Surfacearea = calcshape.Surfacearea(calcshape.Sidelength);
                ViewBag.Onesidearea = calcshape.Onesidearea(calcshape.Sidelength);

                }

                if (calcshape.Name == "Square")

                { 

                ViewBag.Area = calcshape.Area(calcshape.Sidelength);
                ViewBag.Perimeter = calcshape.Perimeter(calcshape.Sidelength);

                }

                if (calcshape.Name == "Segment")

                {

                ViewBag.Error = "There are no calculations for a segment.";

                }

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

            return Redirect("/Home/Catalog");
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
    public IActionResult Calculate()
    {
        List<Shape> TheList = context.Shapes.ToList();
        if (TheList.Count > 0)
        {
            CalculateViewModel calculateViewModel = new CalculateViewModel();

            calculateViewModel.TheList = TheList;

            return View(calculateViewModel);
        }

        else
        {
            return Redirect("/");
        }
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
                
                return Redirect("/Home/Catalog");
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

        public IActionResult Random()
        {
            
            List<Shape> TheList = context.Shapes.ToList();
            if (TheList.Count > 0) { 

            RandomViewModel randomViewModel = new RandomViewModel();

                int Shapecont = TheList.Count();
                Random random = new Random();
                int Shapeind = random.Next(0, Shapecont);
                Shape Ranshape = TheList[Shapeind];

                randomViewModel.Ranshape = Ranshape;

                return View(randomViewModel);
            }

            else
            {
                return Redirect("/");
            }
        }
    }

}