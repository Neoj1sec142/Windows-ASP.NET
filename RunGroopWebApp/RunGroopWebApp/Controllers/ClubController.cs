using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            this._clubRepository = clubRepository;
            this._photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVm.Image);
                var club = new Club
                {
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = clubVm.Address.City,
                        State= clubVm.Address.State,
                        Street= clubVm.Address.Street,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVm);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVm = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                Address = club.Address,
                AddressId = club.AddressId,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVm)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVm);
            }
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub == null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVm);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVm.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = photoResult.Url.ToString(),
                    Address = clubVm.Address,
                    AddressId = clubVm.AddressId,
                };
            _clubRepository.Update(club);
            return RedirectToAction("Index");
            }
            else
            {
                return View(clubVm);
            }
        }
    }
}
