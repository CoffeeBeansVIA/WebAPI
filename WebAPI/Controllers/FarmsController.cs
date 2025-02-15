﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Database.DTOs;
using WebAPI.Database.Models;
using WebAPI.Services.Farms;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFarmsService _farmsService;

        public FarmsController(IFarmsService farmsService, IMapper mapper)
        {
            _farmsService = farmsService;
            _mapper = mapper;
        }

        [HttpGet("eui/{EUI}")]
        public async Task<IActionResult> GetFarmByEui(string eui)
        {
            var foundFarm = await _farmsService.GetFarmByEUI(eui);

            if (foundFarm == null)
                return NotFound();

            return Ok(_mapper.Map<FarmDetailDto>(foundFarm));
        }

        [HttpGet("{farmId}")]
        public async Task<IActionResult> GetFarmById(int farmId)
        {
            var foundFarm = await _farmsService.GetFarmByIdAsync(farmId);

            if (foundFarm == null)
                return NotFound();

            return Ok(_mapper.Map<FarmDetailDto>(foundFarm));
        }

        [HttpGet]
        public async Task<ActionResult<IList<FarmDetailDto>>> GetFarms()
        {
            var fetchedFarms = await _farmsService.GetAllFarmsAsync();
            if (fetchedFarms == null)
                return NoContent();

            return Ok(_mapper.Map<IEnumerable<FarmDetailDto>>(fetchedFarms));
        }

        [HttpDelete("{farmId}")]
        public async Task<IActionResult> RemoveFarm(int farmId)
        {
            try
            {
                await _farmsService.RemoveFarmByIdAsync(farmId);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarm(FarmDto farm)
        {
            try
            {
                var createdFarm=await _farmsService.CreateFarmAsync(_mapper.Map<Farm>(farm));
                return Ok(_mapper.Map<FarmDto>(createdFarm));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}