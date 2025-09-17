namespace Yu.API.Controllers.Admin;

[ApiController, Route("api/admin/[controller]")]
public class OrdersController : BaseAdminApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] GetOrdersFilterRequestDto? filter)
    {
        return Ok(await Mediator.Send(new GetOrdersQuery(filter)));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderDetailsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderDetailsAsync([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetAdminOrderDetailsQuery(id)));
    }

    [HttpPatch("{id}/change-date")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangeOrderDateAsync([FromRoute] int id,
        [FromBody] UpdateOrderDateRequestDto request)
    {
        await Mediator.Send(new UpdateOrderDateCommand(id, request.Date, request.DateType));
        return NoContent();
    }

    [HttpGet("statistics/count")]
    [Authorize]
    [ProducesResponseType(typeof(OrderCountStatisticsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderCountStatisticsAsync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        return Ok(await Mediator.Send(new GetOrderCountStatisticsQuery(startDate, endDate)));
    }

    [HttpGet("statistics/value")]
    [Authorize]
    [ProducesResponseType(typeof(OrderValueStatisticsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderValueStatisticsAsync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        return Ok(await Mediator.Send(new GetOrderValueStatisticsQuery(startDate, endDate)));
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportOrdersAsync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var rows = await Mediator.Send(new GetOrdersExportQuery(startDate, endDate));

        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("Orders");

        int col = 1;
        ws.Cell(1, col++).Value = "ID";
        ws.Cell(1, col++).Value = "Order Number";
        ws.Cell(1, col++).Value = "Created Date";
        ws.Cell(1, col++).Value = "Customer";
        ws.Cell(1, col++).Value = "Phone";
        ws.Cell(1, col++).Value = "Address";
        ws.Cell(1, col++).Value = "Total Price";
        ws.Cell(1, col++).Value = "Status";
        ws.Cell(1, col++).Value = "Sub Status";

        int rowIndex = 2;
        foreach (var r in rows)
        {
            int c = 1;
            ws.Cell(rowIndex, c++).Value = r.Id;
            ws.Cell(rowIndex, c++).Value = r.OrderNumber;
            ws.Cell(rowIndex, c++).Value = r.CreatedDate.ToString("yyyy-MM-dd HH:mm");
            ws.Cell(rowIndex, c++).Value = r.CustomerFullName;
            ws.Cell(rowIndex, c++).Value = r.CustomerPhone;
            ws.Cell(rowIndex, c++).Value = r.Address;
            ws.Cell(rowIndex, c++).Value = r.TotalPrice;
            ws.Cell(rowIndex, c++).Value = r.OrderStatus.ToString();
            ws.Cell(rowIndex, c++).Value = r.SubStatus.ToString();
            rowIndex++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"orders_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx";
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

}