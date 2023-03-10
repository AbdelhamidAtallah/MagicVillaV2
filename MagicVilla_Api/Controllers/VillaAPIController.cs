using AutoMapper;
using MagicVilla_Api.Data;
using MagicVilla_Api.Logging;
using MagicVilla_Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_Api.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    
    public class VillaAPIController : ControllerBase
    {
        /*private readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }*/

        //private readonly ILogging _logger;
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

      
        public VillaAPIController(ApplicationDbContext db , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>>  GetVillas()
        {
            //_logger.Log("getting all villa ","");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}",Name ="GetVilla")]
        //[ProducesResponseType(200,Type=typeof(VillaDTO))]
        //[ProducesResponseType(404)] //notfound
        //[ProducesResponseType(400)] //badrequest
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {

            if(id == 0)
            {
                //_logger.Log("get villa error!" + id, "error");
                return BadRequest();
            }
            var villa=await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>>  CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

           

            if(villaCreateDTO == null)
            {
                return BadRequest(villaCreateDTO);
            }



        
            Villa model = _mapper.Map<Villa>(villaCreateDTO);
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if(id == 0) {
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa ==null)
            {
                return NotFound();
            }


            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();


            return NoContent();
            
        }


        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateVilla([FromBody] VillaUpdateDTO villaUpdateDTO,int id)
        {
            if (villaUpdateDTO == null || id != villaUpdateDTO.Id) {
                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(villaUpdateDTO);
            
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();


            return NoContent();
            
        }

        [HttpPatch("{id:int}", Name = "UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id,JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id ==0 )
            {
                return BadRequest();
            }
            var villa =  await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            VillaUpdateDTO villaToPatch = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaToPatch,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa villaToDb = _mapper.Map<Villa>(villaToPatch);

            _db.Villas.Update(villaToDb);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
