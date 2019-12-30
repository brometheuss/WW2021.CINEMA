# Introduction
In this section we will cover fundamentals of Web Api

ASP .NET Core supports creating RESTful services, also known as web APIs, using C#.
To handle requests, a web API uses controllers. Controllers in a web API are classes that derive from ControllerBase.
This document shows how to use controllers for handling web API requests.

##Web-API

### What is Web-API?

WebAPI is a framework which helps you to build/develop HTTP services.

More about this topic on:
https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.0



## REST

### What is REST?

REST is acronym for **RE**presentational **S**tate **T**ransfer. It is architectural style for distributed hypermedia systems.

### Guiding Principles of REST

*   **Client–server** – By separating the user interface concerns from the data storage concerns, 
    we improve the portability of the user interface across multiple platforms and improve scalability by simplifying the server components.
*   **Stateless** – Each request from client to server must contain all of the information necessary to understand the request,
    and cannot take advantage of any stored context on the server. Session state is therefore kept entirely on the client.
*   **Cacheable** – Cache constraints require that the data within a response to a request be implicitly or
    explicitly labeled as cacheable or non-cacheable. If a response is cacheable, then a client cache is given the right to reuse 
    that response data for later, equivalent requests.
*   **Uniform interface** – By applying the software engineering principle of generality to the component interface, 
    the overall system architecture is simplified and the visibility of interactions is improved. In order to obtain a uniform interface,
    multiple architectural constraints are needed to guide the behavior of components. REST is defined by four interface constraints: 
    identification of resources; manipulation of resources through representations; self-descriptive messages; and 
    hypermedia as the engine of application state.
*   **Layered system** – The layered system style allows an architecture to be composed of hierarchical layers 
    by constraining component behavior such that each component cannot “see” beyond the immediate layer with which they are interacting.
*   **Code on demand (optional)** – REST allows client functionality to be extended by downloading and executing code in the form of applets or scripts.
    This simplifies clients by reducing the number of features required to be pre-implemented.


These principles need to be satisfied if we want an interface to be referred as RESTful.

### What is RESTful?

A **RESTful** API is an application program interface (API) that uses HTTP requests to **GET**, **PUT**, **POST** and **DELETE** data. Based on **REST** 
architecture.

### WebAPI vs REST 

A WEB API could be or not REST compliant. 
Most of them are open source and all of them are an interface to access server resources via an HTTP protocol, which is only a subset of all the REST capabilities.

### HTTP verbs

**HTTP verb GET**

We use **GET**  to retrieve a resource. 

**Example**
```csharp
/// <summary>
/// Gets all current movies
/// </summary>
/// <returns>
/// Microsoft.AspNetCore.Http.StatusCodes.Status200OK
/// </returns>
[HttpGet]
[Route("current")]
public async Task<ActionResult<IEnumerable<Movie>>> GetAsync()
{
    var data = _movieService.GetAllMovies(true);
    return Ok(data);
}
```

**HTTP verb PUT**

We use **PUT** to change the state of or update a resource.

**Example**
```csharp
/// <summary>
/// Updates a movie
/// </summary>
/// <param name="id"></param>
/// <param name="movieModel"></param>
/// <returns>
/// Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
/// </returns>
[HttpPut]
[Route("{id}")]
public async Task<ActionResult> Put(Guid id, [FromBody]MovieModel movieModel)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var movieToUpdate = await _movieService.GetMovieByIdAsync(id);

    movieToUpdate.Title = movieModel.Title;
    movieToUpdate.Current = movieModel.Current;
    movieToUpdate.Year = movieModel.Year;
    movieToUpdate.Rating = movieModel.Rating;

    _movieService.UpdateMovie(movieToUpdate);

    return Accepted("movies//" + movieToUpdate.Id, movieToUpdate);
}
```

**HTTP verb POST**

We use **POST** to create a resource.

**Example**
```csharp
/// <summary>
/// Adds a new movie
/// </summary>
/// <param name="movieModel"></param>
/// <returns>
/// Microsoft.AspNetCore.Http.StatusCodes.Status201Created
/// </returns>
[HttpPost]
public async Task<ActionResult> Post(MovieModel movieModel)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    MovieDomainModel domainModel = new MovieDomainModel
    {
        Current = movieModel.Current,
        Rating = movieModel.Rating,
        Title = movieModel.Title,
        Year = movieModel.Year
    };

    var data = _movieService.AddMovie(domainModel);

    return Created("movies//" + data.Id, data);
}
```

**HTTP verb DELETE**

We use **DELETE** to remove a resource.

**Example**
```csharp
/// <summary>
/// Delete a movie by id
/// </summary>
/// <param name="id"></param>
/// <returns>
/// Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
/// </returns>
[HttpDelete]
[Route("{id}")]
public async Task<ActionResult> Delete(Guid id)
{
    var deletedMovie = _movieService.DeleteMovie(id);

    return Accepted("movies//" + deletedMovie.Id, deletedMovie);
}
```

### PUT vs PATCH

The only similarity between the two is that they can both be used to update resources in a given location.

The main difference between PUT and PATCH requests is witnessed in the way the server processes the enclosed entity to update the resource identified by the Request-URI.
When making a PUT request, the enclosed entity is viewed as the modified version of the resource saved on the original server, and the client is requesting to replace it.
However, with PATCH, the enclosed entity boasts a set of instructions that describe how a resource stored on the original server should be partially modified to create
a new version.

The second difference is when it comes to idempotency. HTTP PUT is said to be idempotent since it always yields the same results every after making several requests.
On the other hand, HTTP PATCH is basically said to be non-idempotent. However, it can be made to be idempotent based on where it is implemented.

### PUT vs POST

The fundamental difference between the **POST** and **PUT** requests is reflected in the different meaning of the Request-URI. 

The POST method is used to request that the origin server accept the entity enclosed in the request as a new subordinate of the resource identified by the 
Request-URI in the Request-Line. It essentially means that POST request-URI should be of a collection URI.
Use POST when you want to add a child resource under resources collection.
POST is NOT idempotent. So if you retry the request N times, you will end up having N resources with N different URIs created on server.

PUT method requests for the enclosed entity be stored under the supplied Request-URI. 
If the Request-URI refers to an already existing resource – an update operation will happen, otherwise create operation should happen 
if Request-URI is a valid resource URI (assuming client is allowed to determine resource identifier).
Use PUT when you want to modify a singular resource which is already a part of resources collection. PUT replaces the resource in its entirety.
PUT method is idempotent. So if you send retry a request multiple times, that should be equivalent to single request modification.



### REST vs HTTP

A lot of people prefer to compare HTTP with REST. **REST and HTTP are not same.**

No, they are not. **HTTP** stands for **H**yperText **T**ransfer **P**rotocol and is a way to transfer files. 
This protocol is used to link pages of hypertext in what we call the world-wide-web. However, there are other transfer protocols available, 
like FTP and gopher, yet they are less popular.

**RE**presentational **S**tate **T**ransfer, or **REST**, is a set of constraints that ensure a scalable, fault-tolerant and easily extendible system. 
The world-wide-web is an example of such system (and the biggest example, one might say). 
REST by itself is not a new invention, but it's the documentation on such systems like the world-wide-web.

One thing that confuses people, is that REST and HTTP seem to be hand-in-hand. 
After all, the world-wide-web itself runs on HTTP, and it makes sense, a RESTful API does the same. 
However, there is nothing in the REST constraints that makes the usage of HTTP as a transfer protocol mandatory. 
It's perfectly possible to use other transfer protocols like SNMP, SMTP and others to use, and your API could still very well be a RESTful API.

In practice, most - if not all - **RESTful APIs** currently use **HTTP* as a transport layer, since the infrastructure, 
servers and client libraries for **HTTP** are widely available already.

### What is Idempotent REST API?

An **IDEMPOTENT** HTTP method is an HTTP method that can be called many times without different outcomes. 
It would not matter if the method is called only once, or ten times over. The result should be the same. 
It essentially means that the result of a successfully performed request is independent of the number of times it is executed.


More about these topics you can find on links: 

*   https://restfulapi.net
*   https://restfulapi.net/rest-put-vs-post/
*   https://restfulapi.net/idempotent-rest-apis/

## Controllers

A controller is used to define and group a set of actions. An action (or action method) is a method on a controller which handles requests.
Controllers logically group similar actions together. This aggregation of actions allows common sets of rules, such as routing, caching and authorization,
to be applied collectively. Requests are mapped to actions through routing.

**Example**
```csharp
[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{      
    private readonly IMovieService _movieService;

    private readonly ILogger<MoviesController> _logger;

    public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
    {
        _logger = logger;
        _movieService = movieService;
    }

   /// <summary>
    /// Gets all current movies
    /// </summary>
    /// <returns>
    /// Microsoft.AspNetCore.Http.StatusCodes.Status200OK
    /// </returns>
    [HttpGet]
    [Route("current")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAsync()
    {
        var data = _movieService.GetAllMovies(true);
        return Ok(data);
    }
}
```

More about controllers you can find:
*   https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.0
*   https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions?view=aspnetcore-3.0

## Routing

Routes are defined in startup code or attributes. Routes describe how URL paths should be matched to actions.
Routes are also used to generate URLs (for links) sent out in responses.

**Example**
```csharp
[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{      
    private readonly IMovieService _movieService;

    private readonly ILogger<MoviesController> _logger;

    public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
    {
        _logger = logger;
        _movieService = movieService;
    }

   /// <summary>
    /// Gets all current movies
    /// </summary>
    /// <returns>
    /// Microsoft.AspNetCore.Http.StatusCodes.Status200OK
    /// </returns>
    [HttpGet]
    [Route("current")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAsync()
    {
        var data = _movieService.GetAllMovies(true);
        return Ok(data);
    }
}
```
This example show us that this action have route.
![picture alt](images/route-postman.png)
This example show us response when we have matched route in postman and in controller.
![picture alt](images/route-matched-controller-action-postman.png)

Paragraph about routing:
https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.0

## Startup class

ASP .NET Core apps use a Startup class, which is named Startup by convention. The Startup class:

*   Optionally includes a ConfigureServices method to configure the app's services. A service is a reusable component that provides app functionality. Services are registered in ConfigureServices and consumed across the app via dependency injection (DI) or ApplicationServices.
*   Includes a Configure method to create the app's request processing pipeline.

ConfigureServices and Configure are called by the ASP .NET Core runtime when the app starts:

**Example of Startup class**
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CinemaContext>(options =>
        {
            options
            .UseSqlServer(Configuration.GetConnectionString("CinemaConnection"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);                 
        });

        services.AddControllers();

        //Repositories
        services.AddTransient<IMoviesRepository, MoviesRepository>();
        services.AddTransient<IProjectionsRepository, ProjectionsRepository>();
        services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();
        services.AddTransient<ICinemasRepository, CinemasRepository>();
        services.AddTransient<ISeatsRepository, SeatsRepository>();

        //Business Logic
        services.AddTransient<IMovieService, MovieService>();
        services.AddTransient<IProjectionService, ProjectionService>();
        services.AddTransient<IAuditoriumService, AuditoriumService>();
        services.AddTransient<ICinemaService, CinemaService>();
        services.AddTransient<ISeatService, SeatService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

Paragraph about Startup class:
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.0
 

## Configuration

App configuration in ASP .NET Core is based on key-value pairs established by configuration providers. Configuration providers read configuration data into key-value pairs from a variety of configuration sources:

*   Azure Key Vault
*   Azure App Configuration
*   Command-line arguments
*   Custom providers (installed or created)
*   Directory files
*   Environment variables
*   In-memory .NET objects
*   Settings files

**Example of appsettings.json**
```json
{
  "ConnectionStrings": {
    "CinemaConnection": "Data Source=AJERINIC-NEW;Initial Catalog=Cinema;Integrated Security=True;Connect Timeout=30;
                            Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*"
}
```
Paragraph about configuration:
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1