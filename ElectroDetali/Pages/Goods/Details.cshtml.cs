﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;

namespace ElectroDetali.Pages.Goods
{
    public class DetailsModel : Models.HelperModels.Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DetailsModel(ElectroDetali.Models.ElectroDetaliContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Good Good { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null || _context.Goods == null)
                {
                    return NotFound();
                }

                var good = await _context.Goods.Include(g => g.Reviews).
                    Include(g => g.Category).
                    FirstOrDefaultAsync(m => m.Id == id);
                if (good == null)
                {
                    return NotFound();
                }
                else
                {
                    Good = good;
                }

                ViewData["Review"] = Good.Reviews;
                ViewData["GoodId"] = Good.Id;
                ViewData["Pick"] = _context.PickUpPoints.ToList();

                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товара:\r\n" + ex.Message;
                return Redirect("./Details");
            }
            
        }

        public IActionResult OnPostAddReview(IFormCollection form)
        {
            try
            {
                var id = form["id"];
                var text = form["review"];

                if (Goods.IndexModel.currentUser == null)
                {
                    var user = new Models.User
                    {
                        Name = _httpContextAccessor.HttpContext.Session.Id
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    Goods.IndexModel.currentUser = user;
                }

                var review = new Review
                {
                    Goodid = Convert.ToInt32(id),
                    Value = text
                };
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return Redirect($"/Goods/Details?id={id}");
            }
            catch (Exception ex)
            {
                var id = form["id"];
                StatusMessage = "Ошибка при добавлении отзыва:\r\n" + ex.Message;
                return Redirect($"/Goods/Details?id={id}");
            }
        }

        public IActionResult OnPostCreateBuy(IFormCollection form)
        {
            try
            {
                var id = form["id"];
                var pick = form["selector"];
                var dateAdd = _context.PickUpPoints.First(f => f.Id == Convert.ToInt32(pick));

                if (Goods.IndexModel.currentUser == null)
                {
                    var user = new Models.User
                    {
                        Name = _httpContextAccessor.HttpContext.Session.Id
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    Goods.IndexModel.currentUser = user;
                }

                var buy = new Buy
                {
                    Goodid = Convert.ToInt32(id),
                    Pointid = Convert.ToInt32(pick),
                    Datedelivery = DateTime.Now.Date.AddDays((double)dateAdd.Time),
                    Datecreate = DateTime.Now.Date,
                    Isbasket = true,
                    Userid = Goods.IndexModel.currentUser.Id,
                };
                _context.Buys.Add(buy);
                _context.SaveChanges();
                return Redirect($"/Goods/Details?id={id}");
            }
            catch (Exception ex)
            {
                var id = form["id"];
                StatusMessage = "Ошибка при добавлении в корзину:\r\n" + ex.Message;
                return Redirect($"/Goods/Details?id={id}");
            }
            
        }
    }

}
