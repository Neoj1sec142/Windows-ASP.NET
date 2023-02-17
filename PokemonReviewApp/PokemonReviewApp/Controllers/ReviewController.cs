using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRespository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRespository,
            IReviewerRepository reviewerRepository, 
            IPokemonRepository pokemonRepository, 
            IMapper mapper)
        {
            _reviewRespository = reviewRespository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRespository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRespository.ReviewExists(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRespository.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRespository.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest();
            var reviews = _reviewRespository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title
                .TrimEnd().ToUpper()).FirstOrDefault();
            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!_reviewRespository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully posted");
        }
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);
            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);
            if (!_reviewRespository.ReviewExists(reviewId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(updatedReview);
            if (!_reviewRespository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRespository.ReviewExists(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRespository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRespository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
