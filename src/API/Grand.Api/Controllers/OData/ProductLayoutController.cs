﻿using Grand.Api.Queries.Models.Common;
using Grand.Business.Common.Interfaces.Security;
using Grand.Business.Common.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Grand.Api.Controllers.OData
{
    public partial class ProductLayoutController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public ProductLayoutController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [SwaggerOperation(summary: "Get entity from ProductLayout by key", OperationId = "GetProductLayoutById")]
        [HttpGet("{key}")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
                return Forbid();

            var layout = await _mediator.Send(new GetLayoutQuery() { Id = key, LayoutName = typeof(Domain.Catalog.ProductLayout).Name });
            if (!layout.Any())
                return NotFound();

            return Ok(layout.FirstOrDefault());

        }

        [SwaggerOperation(summary: "Get entities from ProductTemplate", OperationId = "GetProductTemplates")]
        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
                return Forbid();

            return Ok(await _mediator.Send(new GetLayoutQuery() { LayoutName = typeof(Domain.Catalog.ProductLayout).Name }));
        }
    }
}
