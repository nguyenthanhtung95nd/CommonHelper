Luxury

It website my project handmade very well.

This project is divided into 2 branches containing code -Brach master : merge code when project complete -Brach font-end : contain code for font-end code, angularJS -Brach develop: contain basecore infrastructure of solution

Have a good day! Tks all.

Guide how to result only Custom Json Formater and allow cross domain when client app call by enable CORS.
Note: To allow cross domain for client app call api

Step 1: Install Microsoft.AspNet.WebApi.Cors package

Install-Package Microsoft.AspNet.WebApi.Cors
Step 2: Include the following 2 line of code Register() menthod

        EnableCorsAttribute cors = new EnableCorsAttribute("*","*","*");
        config.EnableCors(cors);
EnableCors attribute can be applied on a specific controller or controller menthod

a. Create 1 class in WebApiConfig within CustomJsonFormater class name

     public class CustomJsonFormater : JsonMediaTypeFormatter
      {
          public CustomJsonFormater()
          {
              this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
          }
          public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
          {
              base.SetDefaultContentHeaders(type, headers, mediaType);
              headers.ContentType = new MediaTypeHeaderValue("application/json");
          }
       }
b. After you write config.Formatters.Add(new CustomJsonFormater()) just below Register function.

Some CRUD functions are simple when implemented with the API, which is useful for you. Note: using sql server with table contain table is Employee
a. Read all records table Employee

   public HttpResponseMessage Get(string gender="All")
      {
          using (EmployeeDBEntities entities = new EmployeeDBEntities())
          {
              return entities.Employees.ToList();
          }
      }
b. Get by Id

   public HttpResponseMessage Get(int id)
    {
        using (EmployeeDBEntities entities = new EmployeeDBEntities())
        {
            var entity = entities.Employees.FirstOrDefault(x => x.ID == id);
            if (entity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Employee with Id = " + id.ToString() + "\t not found!");
            }
        }
    }
c. Post

public HttpResponseMessage Post([FromBody]Employee employee)
    {
        try
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                entities.Employees.Add(employee);
                entities.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return message;
            }
        }
        catch (Exception ex)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        }
    }
d. Put

	public HttpResponseMessage Put(int id, [FromBody] Employee employee)
				{
						try
						{
								using (EmployeeDBEntities entities = new EmployeeDBEntities())
								{
										var entity = entities.Employees.FirstOrDefault(x => x.ID == id);
										if (entity==null)
										{
												return Request.CreateErrorResponse(HttpStatusCode.NotFound,
														"Employee with Id = " + id.ToString() + "\t not found!");
										}
										else
										{
												entity.FristName = employee.FristName;
												entity.LastName = employee.LastName;
												entity.Salary = employee.Salary;
												entities.SaveChanges();
												return Request.CreateResponse(HttpStatusCode.OK);
										}
								}
						}
						catch (Exception ex)
						{

								return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
						}
				}		
e. Delete by id

public HttpResponseMessage Delete(int id)
    {
        try
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(x => x.ID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with Id = " + id.ToString() + "\t not found!");
                }
                else
                {
                    entities.Employees.Remove(entity);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }
        catch (Exception ex)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        }
    }
