var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();

app.MapPost("/create-drafts", async (HttpRequest req) =>
{
    try
    {
        var form = await req.ReadFormAsync();

        var toList = form["to"].ToString()
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var subject = form["subject"].ToString();
        var body = form["body"].ToString();

        string? tempFile = null;
        if (form.Files.Count > 0)
        {
            var file = form.Files[0];
            tempFile = Path.Combine(Path.GetTempPath(), file.FileName);

            using var fs = File.Create(tempFile);
            await file.CopyToAsync(fs);
        }

        var helper = new OutlookHelper();

        foreach (var to in toList)
            helper.CreateDraft(to, subject, body, tempFile);

        return Results.Json(new { status = "drafts_created", count = toList.Length });
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "error", count = 0, message = ex.Message });
    }
});

app.Run("http://localhost:5001");