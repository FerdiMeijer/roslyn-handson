using System;
using System.Collections.Generic;

namespace SourceGenerator.Library
{
    public static class RandomUserFactory
    {
        public static RandomUser Create()
        {
            var user = new RandomUser
            {
                Name = Faker.Name.FirstName(),
                Address = $"{Faker.Address.StreetName()} {Faker.Number.RandomNumber(400)}",
                Username = Faker.User.Username(),
                Email = Faker.User.Email(),
                BirthDay = Faker.Date.Birthday(),
                Hobbies = Faker.Lorem.Paragraphs(Faker.Number.RandomNumber(5))
            };

            return user;
        }
    }

    public class RandomUser
    {
        public string Address { get; internal set; }
        public DateTime BirthDay { get; internal set; }
        public string Email { get; internal set; }
        public List<string> Hobbies { get; internal set; }
        public dynamic Name { get; set; }
        public string Username { get; internal set; }
    }
}