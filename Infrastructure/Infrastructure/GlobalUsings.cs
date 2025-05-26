global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Yu.Domain.Entities;
global using Yu.Application.Abstractions;
global using Yu.Application.Exceptions;
global using Yu.Infrastructure.Concretes;
global using Yu.Application.Extensions;

global using Yu.Application.DTOs;