using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeatherReport.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherReport.Controllers
{
    public class WeatherReportsController : Controller
    {
          WeatherReportContext _context;

        public WeatherReportsController(WeatherReportContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        public ActionResult Weather()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Weather(WeatherCityValidaton model)
        {
            if(ModelState.IsValid)
            {
                if(model.City==null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid city name.");
                    return View(model);
                }
                else
                {
                    GetWeather(model.City);
                }
            }
            return View();
        }

        public ActionResult WeatherCity()
        {
            return View();
        }

        // GET: WearherReports
        public async Task<IActionResult> Index(string searchString)
        {
            var names = from m in _context.WearherReport select m;

            if(!String.IsNullOrEmpty(searchString))
            {
                names = names.Where(p => p.City.Contains(searchString));
            }

            return View(await names.ToListAsync());
        }

        #region public methods
        /// <summary>
        /// Method to convert the Jason Data and save in  DB
        /// </summary>
        public JsonResult GetWeather(string city)
        {
            WearherReport weath = new WearherReport();

            var viewData =  Json(weath.getWeatherForcast(city));
            var temperature = JsonConvert.DeserializeObject<CityName>(viewData.Value.ToString())._temp.temp;
            var name = JsonConvert.DeserializeObject<Wrapper>(viewData.Value.ToString()).name;
            weath.Temp = temperature;
            weath.City = name;

            _context.AddRange(weath);
            _context.SaveChanges();

            return viewData;
        }
       
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearherReport = await _context.WearherReport
                .SingleOrDefaultAsync(m => m.ID == id);
            if (wearherReport == null)
            {
                return NotFound();
            }

            return View(wearherReport);
        }

        // GET: WearherReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WearherReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,City,Temp,WeatherTime")] Models.WearherReport wearherReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wearherReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wearherReport);
        }

        // GET: WearherReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearherReport = await _context.WearherReport.SingleOrDefaultAsync(m => m.ID == id);
            if (wearherReport == null)
            {
                return NotFound();
            }
            return View(wearherReport);
        }

        // POST: WearherReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,City,Temp,WeatherTime")] Models.WearherReport wearherReport)
        {
            if (id != wearherReport.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wearherReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WearherReportExists(wearherReport.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wearherReport);
        }

        // GET: WearherReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearherReport = await _context.WearherReport
                .SingleOrDefaultAsync(m => m.ID == id);
            if (wearherReport == null)
            {
                return NotFound();
            }

            return View(wearherReport);
        }

        // POST: WearherReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wearherReport = await _context.WearherReport.SingleOrDefaultAsync(m => m.ID == id);
            _context.WearherReport.Remove(wearherReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WearherReportExists(int id)
        {
            return _context.WearherReport.Any(e => e.ID == id);
        }
    }

    #endregion

    #region 
    class Wrapper
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }
    class CityName
    {
        [JsonProperty("main")]
        public Temperature _temp { get; set; }
    }
    class Temperature
    {
        [JsonProperty("temp")]
        public string temp { get; set; }
    }

    #endregion
}
