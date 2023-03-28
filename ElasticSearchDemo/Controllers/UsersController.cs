using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElasticSearchDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IElasticClient elasticClient;

        public UsersController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet("{name}")]
        public async Task<User?> Get(string name)
        {
            var response = await elasticClient.SearchAsync<User>(s => s
            //.Index("users")
            .Query(q => q.Term(t=> t.Name,name) ||
                    q.Match(m => m.Field(f=> f.Name).Query(name)))); 
            return response.Documents.FirstOrDefault();
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<string> Post([FromBody] User value)
        {
            var response = await elasticClient.IndexAsync<User>(new IndexRequest<User>(value));
            // var response = await elasticClient.IndexAsync<User>(value, x=> x.Index("users"));
            return response.Id.ToString();
        }
    }
}
