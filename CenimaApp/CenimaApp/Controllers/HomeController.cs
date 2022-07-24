﻿using Microsoft.AspNetCore.Mvc;
using CenimaApp.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace CenimaApp.Controllers
{
    public class HomeController : Controller
    {
        public const string PERSONKEY = "person";
        CenimaDBContext context = new CenimaDBContext();
        Person GetPerson()
        {
            var session = HttpContext.Session;
            string jsonPerson = session.GetString(PERSONKEY);
            if (jsonPerson != null)
            {
                return JsonConvert.DeserializeObject<Person>(jsonPerson);
            }
            return new Person();
        }

        public IActionResult Index(int? id, string? search)
        {
            ViewBag.Person = GetPerson();
            //1. List Category
            List<Genre> genreList = context.Genres.ToList();
            ViewBag.Genres = genreList;

            //2. List Movies
            List<Rate> ratesList = context.Rates.ToList();
            ViewBag.Rates = ratesList;
            List<Movie> moviesList = new List<Movie>();
            if (id == null)
            {
                moviesList = context.Movies.ToList();
            }
            else
            {
                moviesList = context.Movies.Where(m => m.GenreId == id).ToList();
            }
            if (search != null)
            {

                moviesList = context.Movies.Where(m => m.Title.Contains(search.Trim())).ToList();
            }

            return View(moviesList);
        }
        public IActionResult MovieDetails(int id)
        {
            ViewBag.Person = GetPerson();
            //1. List Category
            List<Genre> genreList = context.Genres.ToList();
            ViewBag.Genres = genreList;

            //2. List Movies
            List<Rate> ratesList = context.Rates.ToList();
            Movie movie = context.Movies.Where(m => m.MovieId == id).FirstOrDefault();

            //3. List Rates
            List<Person> personList = context.Persons.ToList();
            List<Rate> rates = context.Rates.Where(r => r.MovieId == id).OrderByDescending(r => r.Time).ToList();
            ViewBag.rates = rates;


           
            Rate rate = context.Rates.Where(r => r.MovieId == id && GetPerson().PersonId == r.PersonId).FirstOrDefault();
            if (rate != null)
            {
                ViewBag.OldRate = rate;
            }
            return View(movie);
        }
        [HttpPost]
        public IActionResult MovieDetails(int MovieID, string comment, float num)
        {
            ViewBag.Person = GetPerson();
           

            //4. Edit Feedback by MovieID
            Rate rate = new Rate();
            if (MovieID != null)
            {
                rate.MovieId = MovieID;
                rate.PersonId = GetPerson().PersonId;
                rate.Comment = comment;
                rate.NumericRating = num;
                rate.Time = System.DateTime.Now;
                context.Rates.Add(rate);
                context.SaveChanges();
                ViewBag.OldRate = rate;
            }

            //1. List Category
            List<Genre> genreList = context.Genres.ToList();
            ViewBag.Genres = genreList;

            //2. List Movies
            List<Rate> ratesList = context.Rates.ToList();
            Movie movie = context.Movies.Where(m => m.MovieId == MovieID).FirstOrDefault();

            //3. List Rates
            List<Person> personList = context.Persons.ToList();
            List<Rate> rates = context.Rates.Where(r => r.MovieId == MovieID).OrderByDescending(r => r.Time).ToList();
            
            ViewBag.rates = rates;

            return View("MovieDetails", movie);
        }
        public IActionResult EditDetails(int MovieID, string comment, float num)
        {
            ViewBag.Person = GetPerson();


            //4. Feedback Movies
            Rate rate = context.Rates.Where(r => r.MovieId == MovieID && r.PersonId == GetPerson().PersonId).FirstOrDefault();

                rate.Comment = comment;
                rate.NumericRating = num;
                context.SaveChanges();
                ViewBag.OldRate = rate;


            //1. List Category
            List<Genre> genreList = context.Genres.ToList();
            ViewBag.Genres = genreList;

            //2. List Movies
            List<Rate> ratesList = context.Rates.ToList();
            Movie movie = context.Movies.Where(m => m.MovieId == MovieID).FirstOrDefault();

            //3. List Rates
            List<Person> personList = context.Persons.ToList();
            List<Rate> rates = context.Rates.Where(r => r.MovieId == MovieID).OrderByDescending(r => r.Time).ToList();

            ViewBag.rates = rates;

            return View("MovieDetails", movie);
        }
    }
}
