using Blazor.Server.Data;
using Blazor.Server.Extensions;
using Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<EmployeeInfo> repository;
        public EmployeeController(IRepository<EmployeeInfo> repository) =>
            this.repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeInfo>>> GetEmployees(
            [FromQuery] PaginationDTO pagination,
            [FromQuery] String name)
        {
            var queryable = repository.Items;
            if (queryable is null) return NotFound();
            if (!String.IsNullOrEmpty(name)) {
                queryable = queryable.Where(x => x.Name.Contains(name));
            }
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPerPage);

            return Ok(await queryable.Paginate(pagination));
        }

        [HttpGet("{id:int}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeInfo>> GetEmployee([FromRoute] int id)
        {
            var employee = await repository.TakeAsync(id);
            if (employee is null) return NotFound();

            return Ok(employee);
        }

        // PUT: api/employee/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEmployee([FromBody] EmployeeInfo employee, [FromRoute] int id)
        {
            if (id != employee.Id) return BadRequest();
            await repository.SaveAsync(employee);

            return NoContent();
        }

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeInfo employee)
        {
            await repository.SaveAsync(employee);

            return CreatedAtAction(nameof(GetEmployee), // возвращает код состояния HTTP 201,
                new { id = employee.Id }, employee);    // создаёт URI на созданный ресурс
        }

        // DELETE: api/employee/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var deletedEmployee = await repository.DeleteAsync(id);
            if (deletedEmployee is null) return NotFound();

            return NoContent();
        }
    }
}
