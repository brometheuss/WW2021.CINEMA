# Introduction
In this section we will fundamentals of Web Api
https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.0

ASP.NET Core supports creating RESTful services, also known as web APIs, using C#.
To handle requests, a web API uses controllers. Controllers in a web API are classes that derive from ControllerBase.
This article shows how to use controllers for handling web API requests.

## REST

### What is REST?

REST is acronym for Representational State Transfer. It is architectural style for distributed hypermedia systems.

### Guiding Principles of REST

*   **Client–server** – By separating the user interface concerns from the data storage concerns, 
    we improve the portability of the user interface across multiple platforms and improve scalability by simplifying the server components.
*   Stateless – Each request from client to server must contain all of the information necessary to understand the request,
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


These principles need to be satisfied if we an interface to be referred as RESTful.

### What is RESTful?

A **RESTful** API is an application program interface (API) that uses HTTP requests to **GET**, **PUT**, **POST** and **DELETE** data. Based on **REST** 
architecture.

### HTTP verbs

**GET**

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

**PUT**

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

**POST**

We use **POST** to create that resource.

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

**DELETE**

We use **DELETE** to remove resource.

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

More about these topics you can find on link: https://restfulapi.net

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
This example show us that this action have route:
http://localhost:55430/movies/current

Paragraph about routing:
https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.0

## Startup class

Paragraph about Startup class:
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.0
 

## Configuration

appsettings.json