﻿using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Api.Controller
{
    /// <summary>
    /// Authentication
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _auth;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticateService auth, IMapper mapper)
        {
            this._auth = auth;
            this._mapper = mapper;
        }
        
        /// <summary>
        /// Get JWT Token
        /// </summary>
        /// <param name="request">User login info</param>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="400">If authorization verification is not passed</response>
        /// <response code="422">DTO LoginRequestDTO failed to pass the model validation</response>
        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult RequestToken([FromBody] LoginRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null
            if (!ModelState.IsValid)
            {
                // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var loginRequest = _mapper.Map<LoginRequest>(request);

            var (isAuth, result) = _auth.IsAuthenticated(loginRequest);
            if (isAuth)
            {
                return Ok(result);
            }

            return BadRequest("Invalid Request");
        }
    }
}