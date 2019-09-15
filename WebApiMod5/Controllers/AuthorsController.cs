using AutoMapper;
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
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Author author)
        {

            context.Entry(author).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetAuthor", new { id = author.Id }, author);
        }

    }




}

