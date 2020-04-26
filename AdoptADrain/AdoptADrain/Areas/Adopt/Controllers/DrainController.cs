﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdoptADrain.Areas.Adopt.Models;
using AdoptADrain.Areas.Adopt.ViewModels;
using AdoptADrain.DomainModels;
using AdoptADrain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdoptADrain.Areas.Adopt.Controllers
{
 
    [Authorize]
    public class DrainController : Controller
    {
        private readonly IDrainService _drainService;
        private readonly ILogger<DrainController> _logger;
        private readonly IMapper _mapper;

        public DrainController(ILogger<DrainController> logger, IMapper mapper, IDrainService drainService)
        {
            _logger = logger;
            _drainService = drainService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Adopted()
        {
            //Get drains for user
            string userObjectId = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            //User Drains
            List<DrainDTO> userDrains = await _drainService.GetDrainAll(new DrainSearchOptions { AdoptedUserId = userObjectId });

            //Available Drains
            List<DrainDTO> availableDrains = await _drainService.GetDrainAll(new DrainSearchOptions { excludeAdopted = true });

            return View(new UserAdoptedDrainsVM { AdoptedDrains = userDrains, AvailableDrains = availableDrains });
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        public IActionResult Abandon()
        {
            return View();
        }

        public async Task<IActionResult> FlowDirection()
        {
            List<FlowDirectionDTO> flowDirectionList = await _drainService.GetFlowDirectionAll();
            return View(flowDirectionList);
        }

        public async Task<IActionResult> CreateFlowDirection(FlowDirectionDTO flowDirection)
        {
            FlowDirection newFlowDirection = _mapper.Map<FlowDirection>(flowDirection);

            await _drainService.CreateFlowDirection(newFlowDirection);

            return Ok();
        }
    }
}