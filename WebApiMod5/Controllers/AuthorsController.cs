using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMod5.Context;
using WebApiMod5.Entities;
using WebApiMod5.Models;

namespace WebApiMod5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Get api/authors
        //It returns AuthorDT entities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>>Get(){

            var authors = await context.Authors.ToListAsync();
            var authorDTO = mapper.Map<List<AuthorDTO>>(authors);
            return authorDTO;

        }


        //Get api/authors/5
        //It returns AuthorDT entities
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDTO>> Get(int id, string param)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return NotFound();

            }

            var authorDTO = mapper.Map<AuthorDTO>(author);

            return authorDTO;

        }


        //Post api/authors
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthorCreationDTO authorcreationDTO)
        {
            var author = mapper.Map<Author>(authorcreationDTO);
            context.Add(author);
            await context.SaveChangesAsync();
            var authorDTO = mapper.Map<AuthorDTO>(author);
            return new CreatedAtRouteResult("GetAuthor", new { id = author.Id }, authorDTO);
        }


        //Put api/authors
        //Complete Update need all data from user
        //{	"name": "Teresa","birthDate": "1998-09-06T00:00:00"}
    [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AuthorCreationDTO authorUpdating)
        {
            var author = mapper.Map<Author>(authorUpdating);
            author.Id = id;
            context.Entry(author).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        //Parcial Updatings
        //Patch api/Authors/1005
        //[{	"op":"replace",	"path":"/name",	"value": "Teresita"}]
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AuthorCreationDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            Author AuthorinsideDB = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (AuthorinsideDB == null)
            {
                return NotFound();
            }
            //If find a problem here is for the version of  autormapper. You need to change
            var authorDTO = mapper.Map<AuthorCreationDTO>(AuthorinsideDB);

            patchDocument.ApplyTo(authorDTO, ModelState);

            var isvalid = TryValidateModel(AuthorinsideDB);

            if (!isvalid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(authorDTO, AuthorinsideDB);
            await context.SaveChangesAsync();
            return NoContent();
        }


        //No DTO
        ////Parcial Updatings
        ////Patch api/Authors/1005
        ////[{	"op":"replace",	"path":"/name",	"value": "Teresita"}]
        //[HttpPatch("{id}")]
        //public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Author> patchDocument)
        //{
        //    if(patchDocument == null)
        //    {
        //        return BadRequest();
        //    }
        //    var AuthorinsideDB = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        //    if (AuthorinsideDB == null)
        //    {
        //        return NotFound();
        //    }
        //    patchDocument.ApplyTo(AuthorinsideDB, ModelState);

        //    var isvalid = TryValidateModel(AuthorinsideDB);

        //    if (!isvalid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //Delete  api/author/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>>Delete(int id)
        {
            //Only search id and not search all the data of the author
            var authorId = await context.Authors.Select(x=>x.Id).FirstOrDefaultAsync(x => x == id);
            //search all the author data
            //var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);

            //it's int because id is int
            if (authorId == default(int))
            {
                return NotFound();
            }
            //Id is new variable
            context.Remove(new Author { Id = authorId });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }




}

