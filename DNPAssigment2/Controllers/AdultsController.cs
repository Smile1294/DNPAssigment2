using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DNPAssigment1.Models;
using DNPAssigment1.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace DNPAssigment2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdultsController : ControllerBase
    {
        private IAdultServices Iadult;

        public AdultsController(IAdultServices iadult)
        {
            this.Iadult = iadult;
        }
        
        
        [HttpGet ("{ID}")]
        public async Task<ActionResult<Adult>> GetAdult(int ID)
        {
            try
            {
                var d = await Iadult.getAdult(ID);
                return Ok(d);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpDelete ("{id}")]
        public async Task DeleteAsync([FromRoute] int id)
        {
            try
            {
                await Iadult.RemoveAdult(id);
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        [HttpPatch]
        public async Task EditAsync([FromBody]Adult adult)
        {
            try
            {
                await Iadult.UpdateAdults(adult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<Adult>>> GetAdults()
        {
            try
            {
                var adults = await Iadult.LoadAdultsAsync();
                return Ok(JsonSerializer.Serialize(adults));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Adult>> AddAdult([FromBody] Adult adult)
        {
            Console.WriteLine(ModelState);
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine(adult);
                await Iadult.AddAdult(adult);
                return Created($"/{adult.Id}", adult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}