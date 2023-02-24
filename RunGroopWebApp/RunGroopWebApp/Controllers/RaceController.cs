using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            
            this._raceRepository = raceRepository;
            this._photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVm.Image);
                var race = new Race
                {
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = raceVm.Address.City,
                        State = raceVm.Address.State,
                        Street = raceVm.Address.Street,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVm);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVm = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                Address = race.Address,
                AddressId = race.AddressId,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVm);
            }
            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (userRace == null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVm);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVm.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = photoResult.Url.ToString(),
                    Address = raceVm.Address,
                    AddressId = raceVm.AddressId,
                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVm);
            }
        }
    }
}
