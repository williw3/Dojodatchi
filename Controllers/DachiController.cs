using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace dojodachi.Controllers
{
    public class DachiController : Controller
    {

        [HttpGet]
        [Route("")]

        public IActionResult index()
        {
            //retrieve data from session:
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");
            int? meals = HttpContext.Session.GetInt32("meals");
            int? energy = HttpContext.Session.GetInt32("energy");
            if (fullness == null && happiness == null && meals == null && energy == null)
            {   
                //if no session data, initialize
                HttpContext.Session.SetInt32("fullness", 20);
                HttpContext.Session.SetInt32("happiness", 20);
                HttpContext.Session.SetInt32("meals", 3);
                HttpContext.Session.SetInt32("energy", 50);
            }
            
            //show session to index via viewbag
            ViewBag.fullness = HttpContext.Session.GetInt32("fullness");
            ViewBag.happiness = HttpContext.Session.GetInt32("happiness");
            ViewBag.meals = HttpContext.Session.GetInt32("meals");
            ViewBag.energy = HttpContext.Session.GetInt32("energy");
            

            //Check if winner or loser
            if (fullness > 99 && happiness > 99 && energy > 99)
            {
               TempData["action"] = "Congratulations you dojodatchi is all grown up!! GAME OVER";
            }
            if (fullness < 1 || happiness < 1)
            {
                TempData["action"] = "Your dojodatchi died sad and alone you monster!!";
            }

            ViewBag.action = TempData["action"];
            return View();
        }

        [HttpGet]
        [Route("feed")]

        public IActionResult feed()
        {
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? meals = HttpContext.Session.GetInt32("meals");

            if (meals < 1)
            {
                TempData["action"] = "You have no meals left to feed your datchi, GAME OVER";
                return RedirectToAction("index");
            }
            Random rand = new Random();
            int picky = rand.Next(0,4);
            int sated = rand.Next(5,11);
            Console.WriteLine(picky);
            Console.WriteLine(sated);
            if (picky == 2)
                {
                    TempData["action"] = "Your datchi didn't like that meal";
                }
            else
                {
                    fullness += sated;
                    TempData["action"] = $"Your datchi ate {sated}";
                }

            meals -= 1;
            HttpContext.Session.SetInt32("fullness", (int)fullness);
            HttpContext.Session.SetInt32("meals", (int)meals);
            Console.WriteLine("We in this bitch$$$$$$");
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("play")]
        public IActionResult play()
        {
            int? energy = HttpContext.Session.GetInt32("energy");
            int? happiness = HttpContext.Session.GetInt32("happiness");

            Random rand = new Random();
            int happy = rand.Next(5,11);
            int sad = rand.Next(0,4);
            if (sad == 2)
            {
                TempData["action"] = $"Your datchi does not want to play";
            }
            else 
            {
                happiness += happy;
                TempData["action"] = $"Your datchi gained {happy} happiness";
            }
            
            energy -= 5;
            HttpContext.Session.SetInt32("energy", (int)energy);
            HttpContext.Session.SetInt32("happiness", (int)happiness);
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("work")]
        public IActionResult work()
        {
            int? energy = HttpContext.Session.GetInt32("energy");
            int? meals = HttpContext.Session.GetInt32("meals");

            Random rand = new Random();
            int grub = rand.Next(0,3);
            meals += grub;
            energy -= 5;
            TempData["action"] = $"Your datchi earned {grub} meals";
            HttpContext.Session.SetInt32("meals", (int)meals);
            HttpContext.Session.SetInt32("energy", (int)energy);
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("sleep")]
        public IActionResult sleep()
        {
            int? energy = HttpContext.Session.GetInt32("energy");
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");

            energy += 15;
            fullness -= 5;
            happiness -= 5;

            TempData["action"] = $"Your datchi had a good snooze";
            HttpContext.Session.SetInt32("fullness", (int)fullness);
            HttpContext.Session.SetInt32("energy", (int)energy);
            HttpContext.Session.SetInt32("happiness", (int)happiness);
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("reset")]
        public IActionResult reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
    }
}