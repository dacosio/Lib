using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CourseLibrary.API.Controllers.Home
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private ICourseLibraryRepository _repository;
        private IMapper _mapper;
        public AuthorsController(ICourseLibraryRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet]
        //[HttpHead]
        //public IActionResult GetAuthors([FromQuery]string mainCategory, [FromQuery] string searchQuery)
        //{
        //    var authors = _repository.GetAuthors(mainCategory, searchQuery);
        //    return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        //}


        [HttpGet]
        [HttpHead]
        public IActionResult GetAuthors([FromQuery] AuthorResourceParameters authorsResourceParameter)
        {
            var authors = _repository.GetAuthors(authorsResourceParameter);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{id}", Name = "GetAuthor")] 
        public IActionResult GetAuthor(Guid id)
        {
            var author = _repository.GetAuthor(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authorEntity = _mapper.Map<Author>(author);
            _repository.AddAuthor(authorEntity);
            _repository.Save();

            var authorEntityToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = authorEntityToReturn.Id }, authorEntityToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(Guid id, [FromBody] Author author)
        {

            if (!ModelState.IsValid || !_repository.AuthorExists(id))
            {
                return BadRequest();
            }

            var model = _repository.GetAuthor(id);

            model.FirstName = author.FirstName;
            model.LastName = author.LastName;
            model.MainCategory = author.MainCategory;
            model.DateOfBirth = author.DateOfBirth;

            return CreatedAtRoute("GetAuthor", new { model.Id }, model);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            if (!_repository.AuthorExists(id))
            {
                return NotFound("Author not Found");
            }

            var author = _repository.GetAuthor(id);

            _repository.DeleteAuthor(author);

            return NoContent();
        }


    }
}