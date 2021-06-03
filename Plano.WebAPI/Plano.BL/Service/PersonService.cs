using Plano.Data.Interface;
using Plano.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plano.BL.Repository
{
    public class PersonService
    {
        private readonly IRepository<Person> _person;

        public PersonService(IRepository<Person> perosn)
        {
            _person = perosn;
        }
        
        //GET All Person Details 
        public IEnumerable<Person> GetAllPersons()
        {
            try
            {
                return _person.GetAll().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Get Get Person By UserId
        public Person GetPersonByUserId(int UserId)
        {
            return _person.GetAll().Where(x => x.Id == UserId).FirstOrDefault();
        }
        //Add Person
        public async Task<Person> AddPerson(Person Person)
        {
            return await _person.Create(Person);
        }
    }
}
