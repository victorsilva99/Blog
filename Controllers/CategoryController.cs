﻿using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")] // localhost:4565/v1/categories
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context
                                       .Categories
                                       .ToListAsync();

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor")); 
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id,
                                                      [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model,
                                                   [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug,
                };

                await context.Categories.AddAsync(category);

                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possivel incluir a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsysnc([FromRoute] int id,
                                                   [FromBody] EditorCategoryViewModel model,
                                                   [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possivel atualizar a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsysc([FromRoute] int id,
                                                     [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontraado"));

                context.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possivel excluir a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500,new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
    }
}
