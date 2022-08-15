using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeBot.Entities;
using Microsoft.Data.SqlClient;

namespace CoffeeBot.Repository
{
    public class CoffeeContentRepository
    {
        private const string CONNECTIONSTRING = "Persist Security Info=True;Initial Catalog=Coffee_DEMO;Data Source=.; Application Name=Coffee_DEMO;MultipleActiveResultSets=True;Integrated Security=True";
    
        public void CreatePerson()
        {
            var person = new Person
            {
                Name = "John Doe",
                Age = 54,
                CreatedDateUtc = DateTime.UtcNow
            };
            using (var connection = new SqlConnection(CONNECTIONSTRING))
            {
                var id = connection.Insert(person);
            }
        }
    }
}
