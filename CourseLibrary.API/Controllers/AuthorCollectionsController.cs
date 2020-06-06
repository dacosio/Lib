using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICourseLibraryRepository _repository;

        public AuthorCollectionsController(ICourseLibraryRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
            {
                _repository.AddAuthor(author);
            }

            _repository.Save();

            return Ok();

        }


    }
}
