using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    // GET: api/example
    [HttpGet]
    public IActionResult Get()
    {
        // Return en lista med exempeldata
        var data = new string[] { "Exempel 1", "Exempel 2", "Exempel 3" };
        return Ok(data);
    }

    // GET: api/example/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        // Returnera ett specifikt exempel baserat på id
        var example = $"Exempel {id}";
        return Ok(example);
    }

    // POST: api/example
    [HttpPost]
    public IActionResult Post([FromBody] string value)
    {
        // Skapa en resurs med det mottagna värdet och returnera en bekräftelse
        // I det här fallet antar vi att värdet är en sträng
        // Du kan hantera den inkommande datastrukturen baserat på dina behov
        return Ok($"Skapade en resurs med värdet: {value}");
    }

    // PUT: api/example/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] string value)
    {
        // Uppdatera en resurs med det mottagna värdet och returnera en bekräftelse
        // I det här fallet antar vi att värdet är en sträng
        // Du kan hantera den inkommande datastrukturen baserat på dina behov
        return Ok($"Uppdaterade resursen med id {id} till värdet: {value}");
    }

    // DELETE: api/example/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Ta bort en resurs baserat på id och returnera en bekräftelse
        return Ok($"Tog bort resursen med id {id}");
    }
}
