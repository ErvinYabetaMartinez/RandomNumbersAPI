using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RandomNumbersAPI.Models;
//hola
namespace RandomNumbersAPI.Controllers
{
    [Route("random")]
    [ApiController]
    public class RandomNumberController : ControllerBase
    {
        private readonly Random _random = new Random();

        [HttpGet("number")]
        public IActionResult GetRandomNumber([FromQuery] int? min, [FromQuery] int? max)
        {
            if (min.HasValue && max.HasValue)
            {
                if (min > max)
                {
                    return BadRequest("El parámetro 'min' no puede ser mayor que 'max'.");
                }

                int value = _random.Next(min.Value, max.Value + 1);
                return Ok(value);
            }

            int randomValue = _random.Next();
            return Ok(randomValue);
        }

        [HttpGet("decimal")]
        public IActionResult GetRandomDecimal()
        {
            double value = _random.NextDouble(); // genera número entre 0.0 y 1.0
            return Ok(value);
        }

        [HttpGet("string")]
        public IActionResult GetRandomString([FromQuery] int length = 8)
        {
            if (length < 1 || length > 1024)
            {
                return BadRequest("La longitud debe estar entre 1 y 1024.");
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            return Ok(result);
        }

        [HttpPost("custom")]
        public IActionResult GetCustomRandom([FromBody] CustomRandomRequest request)
        {
            if (string.IsNullOrEmpty(request.Type))
                return BadRequest("El campo 'type' es obligatorio.");

            switch (request.Type.ToLower())
            {
                case "number":
                    if (!request.Min.HasValue || !request.Max.HasValue)
                        return BadRequest("Para 'number' se requieren 'min' y 'max'.");

                    if (request.Min > request.Max)
                        return BadRequest("'min' no puede ser mayor que 'max'.");

                    int num = _random.Next(request.Min.Value, request.Max.Value + 1);
                    return Ok(new { result = num });

                case "decimal":
                    int decimals = request.Decimals ?? 2;
                    if (decimals < 0 || decimals > 10)
                        return BadRequest("El número de decimales debe estar entre 0 y 10.");

                    double value = Math.Round(_random.NextDouble(), decimals);
                    return Ok(new { result = value });

                case "string":
                    int length = request.Length ?? 8;
                    if (length < 1 || length > 1024)
                        return BadRequest("La longitud debe estar entre 1 y 1024.");

                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var str = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[_random.Next(s.Length)]).ToArray());

                    return Ok(new { result = str });

                default:
                    return BadRequest("El tipo debe ser 'number', 'decimal' o 'string'.");
            }
        }
    }
}
