using MVCAssessment2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

//  Web API
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configuration Root
IConfigurationRoot configuration; //normal variable
configuration = new ConfigurationBuilder().AddJsonFile("./config.json").Build();

// Add the connection string
builder.Services.AddDbContext<CSIROContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DBConnection");
    options.UseSqlServer(connectionString);
});

// Create a user
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = false;    //add email confirmation
    })
    .AddEntityFrameworkStores<CSIROContext>()
    .AddDefaultTokenProviders();

//APIs for registering session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//pattern: "{controller=Applicant}/{action=DisplayOne}/{id?}");
//pattern: "{controller=Applicant}/{action=Add}/{id?}");
//pattern: "{controller=Applicant}/{action=Edit}/{id?}");
//pattern: "{controller=Applicant}/{action=Delete}/{id?}");

//pattern: "{controller=Account}/{action=Register}/{id?}");
//pattern: "{controller=Account}/{action=Login}/{id?}");

//pattern: "{controller=Admin}/{action=CreateRole}/{id?}");
//pattern: "{controller=Admin}/{action=ManageRole}/{id?}");

//pattern: "{controller=Application}/{action=Successful}/{id?}");

DownloadData();

app.Run();

// Universities Web API by Jovan
async static void DownloadData()
{
    // THE FOLLOWING CODE WILL GET THE DEFAULTDATASETID FROM THE WEB API
    Console.WriteLine("DEBUG OUTPUT: DownloadData() executed.");
    HttpClient client = new HttpClient();
    var response = client.GetAsync("https://api.apify.com/v2/acts/vbartonicek~topuniversities-scraper/runs?token=apify_api_1ikYBA5ymZ1ghvxDVk7Y8dU9sDP6zQ4qXTFB");
    response.Wait();

    string defaultDatasetId = "";

    if (response.IsCompleted)
    {
        Console.WriteLine("DEBUG OUTPUT: response.IsCompleted = true");

        var result = response.Result;
        var dataAsString = "";
        if (result.IsSuccessStatusCode)
        {
            dataAsString = await result.Content.ReadAsStringAsync();
            Console.WriteLine(dataAsString);
        }

        JsonDocument jsonDocument = JsonDocument.Parse(dataAsString);
        JsonElement root = jsonDocument.RootElement;

        if (root.TryGetProperty("data", out JsonElement dataElement))
        {
            Console.WriteLine("DEBUG OUTPUT: root.TryGetProperty() evaluates to true");

            if (dataElement.TryGetProperty("items", out JsonElement itemsElement))
            {
                Console.WriteLine("DEBUG OUTPUT: dataElement.TryGetProperty() evaluates to true");

                itemsElement = itemsElement[0];

                if (itemsElement.TryGetProperty("defaultDatasetId", out JsonElement defaultDatasetId_Element))
                {
                    Console.WriteLine("DEBUG OUTPUT: If itemsElement.TryGetProperty evaluates to true");
                    defaultDatasetId = defaultDatasetId_Element.GetString();
                    Console.WriteLine($"defaultDatasetID = {defaultDatasetId}");
                }
            }


        }

    }// END IF STATEMENT: IF(RESPONSE.ISCOMPLETED())


    // CAN I ACCESS "DEFAULTDATASETID" FROM WITHIN THIS SCOPE?
    Console.WriteLine("DEBUG OUTPUT: defaultDatasetId = " + defaultDatasetId);

    HttpClient client2 = new HttpClient();
    var response2 = client.GetAsync($"https://api.apify.com/v2/datasets/{defaultDatasetId}/items");
    response2.Wait();

    if (response2.IsCompleted)
    {
        Console.WriteLine("DEBUG OUTPUT: response2.IsCompleted = true");
        var result = response2.Result;
        var dataAsString = "";
        if (result.IsSuccessStatusCode)
        {
            dataAsString = await result.Content.ReadAsStringAsync();
            Console.WriteLine(dataAsString);
        }

        //JsonDocument jsonDocument = JsonDocument.Parse(dataAsString);
        //JsonElement root = jsonDocument.RootElement;




        var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Universities>>(dataAsString);

        foreach (var val in list)
        {
            val.universityName = val.Title;
            Console.WriteLine($"The title is: {val.universityName}");
        }

        // NOW I NEED TO GET THE TITLE OF EACH UNIVERSITY INTO THE "UNIVERSITIES" TABLE 
        // IN THE DATABASE.

        string connectionString = "Server=DESKTOP-A7J167C;Database=CSIRO;Integrated Security=True;Encrypt=False";
        SqlConnection conn;
        conn = new SqlConnection(connectionString);
        conn.Open();

        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        String sql = "";

        // Test insertion into database
        //sql = "Insert into AspNetUsers (Id, UserName, Email,EmailConfirmed,PhoneNumberConfirmed, TwoFactorEnabled,LockoutEnabled, AccessFailedCount) values(1,'blah blah','jovvy@jovvy.com','True','True','True','False', 0)";

        command = new SqlCommand(sql, conn);
        //sql = "DELETE FROM Universities";
        SqlCommand deleteCommand = new SqlCommand(sql, conn);
        //deleteCommand.ExecuteNonQuery();

        foreach (var val in list)
        {
            Console.WriteLine($"Attempting to insert {val.universityName} into the database.");

            sql = "INSERT INTO Universities (universityName) VALUES (@Title)";
            SqlCommand insertCommand = new SqlCommand(sql, conn);
            insertCommand.Parameters.AddWithValue("@Title", val.universityName);

            try
            {
                int rowsAffected = insertCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Insertion into the database successful.");
                }
                else
                {
                    Console.WriteLine("Insertion into the database failed.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while executing the query: " + ex.Message);
            }
        }

        conn.Close();

    }

} // end of DownloadData()

