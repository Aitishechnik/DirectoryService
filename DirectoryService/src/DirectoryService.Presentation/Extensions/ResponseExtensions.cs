﻿using DirectoryService.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Errors errors)
        {
            if (errors.Any())
            {
                return new ObjectResult(null)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }

            var distinctErrorTypes = errors
                .Select(e => e.Type)
                .Distinct()
                .ToList();

            int statusCode = distinctErrorTypes.Count > 1
                ? StatusCodes.Status500InternalServerError
                : GetStatusCodeFromErrorType(distinctErrorTypes.First());

            return new ObjectResult(errors)
            {
                StatusCode = statusCode,
            };
        }

        private static int GetStatusCodeFromErrorType(Error.ErrorType errorType) =>
        errorType switch
            {
                Error.ErrorType.VALIDATION => StatusCodes.Status400BadRequest,
                Error.ErrorType.NOT_FOUND => StatusCodes.Status404NotFound,
                Error.ErrorType.CONFLICT => StatusCodes.Status409Conflict,
                Error.ErrorType.FAILURE => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };
    }
}