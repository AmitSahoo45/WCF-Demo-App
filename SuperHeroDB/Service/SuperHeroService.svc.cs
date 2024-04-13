using SuperHeroDB;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace SuperHeroDB.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SuperHeroService
    {
        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public string DoWork()
        {
            return "This is the SuperHeroDB service!";
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<SuperHero> GetAllHeros(string search = "")
        {
            if (!string.IsNullOrEmpty(search))
                return Data.SuperHeroes.FindAll(hero => hero.FirstName.ToLower().Contains(search.ToLower()) || hero.LastName.ToLower().Contains(search.ToLower()) || hero.HeroName.ToLower().Contains(search.ToLower()) || hero.PlaceOfBirth.ToLower().Contains(search.ToLower()));

            return Data.SuperHeroes;
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetHero/{id}")]
        public SuperHero GetHero(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WebFaultException<string>("Id is required", System.Net.HttpStatusCode.BadRequest);

            var superHero = Data.SuperHeroes.Find(hero => hero.Id == int.Parse(id));

            if (superHero == null)
                throw new WebFaultException<string>("Hero not found", System.Net.HttpStatusCode.NotFound);

            return superHero;
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddHero")]
        public SuperHero AddHero(SuperHero hero)
        {
            if (hero == null)
                throw new WebFaultException<string>("Hero is required", System.Net.HttpStatusCode.BadRequest);

            hero.Id = Data.SuperHeroes.Count + 1;
            Data.SuperHeroes.Add(hero);

            return hero;
        }

        [OperationContract]
        [WebInvoke(Method = "PATCH",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateHero/{id}")]
        public SuperHero UpdateHero(SuperHero hero, string id)
        {
            if (hero == null)
                throw new WebFaultException<string>("Hero is required", System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(id))
                throw new WebFaultException<string>("Hero Id is required", System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(hero.FirstName) || string.IsNullOrWhiteSpace(hero.LastName) || string.IsNullOrWhiteSpace(hero.HeroName) || string.IsNullOrWhiteSpace(hero.PlaceOfBirth) || hero.Combat == 0)
                throw new WebFaultException<string>("Please enter valid data", System.Net.HttpStatusCode.BadRequest);

            var superHero = Data.SuperHeroes.Find(h => h.Id == int.Parse(id));

            if (superHero == null)
                throw new WebFaultException<string>("Hero not found", System.Net.HttpStatusCode.NotFound);

            superHero.FirstName = hero.FirstName;
            superHero.LastName = hero.LastName;
            superHero.HeroName = hero.HeroName;
            superHero.PlaceOfBirth = hero.PlaceOfBirth;
            superHero.Combat = hero.Combat;

            return superHero;
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                       RequestFormat = WebMessageFormat.Json,
                       ResponseFormat = WebMessageFormat.Json,
                       BodyStyle = WebMessageBodyStyle.Bare,
                       UriTemplate = "DeleteHero/{id}")]
        public string DeleteHero(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WebFaultException<string>("Id is required", System.Net.HttpStatusCode.BadRequest);

            var superHero = Data.SuperHeroes.Find(hero => hero.Id == int.Parse(id));

            if (superHero == null)
                throw new WebFaultException<string>("Hero not found", System.Net.HttpStatusCode.NotFound);

            Data.SuperHeroes.Remove(superHero);

            return "Hero deleted successfully";
        }
    }
}