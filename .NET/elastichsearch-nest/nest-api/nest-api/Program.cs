using Nest;
using nest_api.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest_api
{
    class Program
    {
        private const string _indexName = "cars-for-sale";
        private const string _elasticsearchAddress = "http://localhost:9200";

        static async Task Main(string[] args)
        {
            await Start();
        }

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        private static async Task Start()
        {
            var client = CreateNESTClient();

            byte choice;
            do
            {
                DisplayMenu();
                choice = GetUserInput();

                switch (choice)
                {
                    case 1:
                        await AddCars(client);
                        break;
                    case 2:
                        await AddCar(client);
                        break;
                    case 3:
                        await RemoveCars(client);
                        break;
                    case 4:
                        await SearchByDescription(client);
                        break;
                    case 5:
                        await ListCars(client);
                        break;
                    default:
                        Console.WriteLine("Wrong choice.");
                        break;
                }
            } while (choice != 6);
        }

        /// <summary>
        /// Creates and returns a new instance of the <see cref="ElasticClient"/>.
        /// </summary>
        /// <returns>New instance of the <see cref="ElasticClient"/>.</returns>
        private static ElasticClient CreateNESTClient()
        {
            var uri = new Uri(_elasticsearchAddress);
            var settings = new ConnectionSettings(uri).DefaultIndex(_indexName);

            var client = new ElasticClient(settings);

            return client;
        }

        /// <summary>
        /// Displays the application menu.
        /// </summary>
        private static void DisplayMenu()
        {
            Console.WriteLine("==========NEST playground==========");
            Console.WriteLine("1. Add predefined cars");
            Console.WriteLine("2. Add car");
            Console.WriteLine("3. Remove cars");
            Console.WriteLine("4. Search cars by description");
            Console.WriteLine("5. List cars");
            Console.WriteLine("6. Exit");
            Console.WriteLine("==========NEST playground==========");
        }

        /// <summary>
        /// Reads the user's input.
        /// </summary>
        private static byte GetUserInput()
        {
            var result = Console.ReadLine();

            byte.TryParse(result, out byte parsed);

            return parsed;
        }

        /// <summary>
        /// Searches and displays the documents according to the user's input. The input is matched with the <see cref="Car.Description"/> property.
        /// </summary>
        /// <param name="client">The <see cref="ElasticClient"/> object instance.</param>
        private static async Task SearchByDescription(ElasticClient client)
        {
            Console.WriteLine("Query: ");
            var query = Console.ReadLine();

            var indexResponses = await client.SearchAsync<Car>(
                descriptor =>
                    descriptor
                        .Index(_indexName)
                        .Size(1000)
                        .Query(
                            q => q
                                .Match(
                                    matchDescriptor => matchDescriptor
                                        .Query(query)
                                        .Field(field => field.Description)
                                    )
                            )
                    );

            PrintExceptionsIfExist(indexResponses);

            foreach (var car in indexResponses.Hits)
                Console.WriteLine($"Brand: {car.Source.Brand}, Description: {car.Source.Description}, Score:{car.Score}");
        }

        /// <summary>
        /// Prints all cars from the Elasticsearch index.
        /// </summary>
        /// <param name="client">The <see cref="ElasticClient"/> object instance.</param>
        private static async Task ListCars(ElasticClient client)
        {
            var indexResponse = await client.SearchAsync<Car>(
                 descriptor => descriptor
                     .Index(_indexName)
                     .Size(1000)
                     .Query(q => q.MatchAll())
                );

            PrintExceptionsIfExist(indexResponse);

            foreach (var car in indexResponse.Documents)
                Console.WriteLine($"Brand: {car.Brand}, Description: {car.Description}");
        }

        /// <summary>
        /// Deletes the cars-for-sale index.
        /// </summary>
        /// <param name="client">The <see cref="ElasticClient"/> object instance.</param>
        private static async Task RemoveCars(ElasticClient client)
        {
            //Delete cars-for-sale index
            var indexResponse = await client.Indices.DeleteAsync(_indexName);

            PrintExceptionsIfExist(indexResponse);
        }

        /// <summary>
        /// Adds predefined list of cars to the Elasticsearch index.
        /// </summary>
        /// <param name="client">The <see cref="ElasticClient"/> object instance.</param>
        private static async Task AddCars(ElasticClient client)
        {
            var cars = GenerateCars();

            foreach (var car in cars)
            {
                // Index each document with the _id excatly the same as the Property Id.
                // By default NEST looks for a property called Id and use it as the _id field in a document.
                // Worth to mention that _id is limited to 512 bytes in size. Larger _id values will be rejected.
                var indexResponse = await client.IndexDocumentAsync(car);

                PrintExceptionsIfExist(indexResponse);
            }
        }

        /// <summary>
        /// Add user defined car to the Elasticsearch index.
        /// </summary>
        /// <param name="client">The <see cref="ElasticClient"/> object instance.</param>
        private static async Task AddCar(ElasticClient client)
        {
            Console.WriteLine("Brand: ");
            var brand = Console.ReadLine();

            Console.WriteLine("Description: ");
            var description = Console.ReadLine();

            var car = new Car
            {
                Id = Guid.NewGuid(),
                Brand = brand,
                Description = description
            };

            var indexResponse = await client.IndexDocumentAsync(car);

            PrintExceptionsIfExist(indexResponse);
        }

        private static void PrintExceptionsIfExist(IResponse indexResponse)
        {
            if (indexResponse.OriginalException is not null)
                Console.WriteLine(indexResponse.OriginalException);

            if (indexResponse.ServerError is not null)
                Console.WriteLine(indexResponse.ServerError);
        }

        private static List<Car> GenerateCars()
        {
            var cars = new List<Car> {
                new Car{
                    Id = Guid.NewGuid(),
                    Brand = "Mercedes",
                    Description = "Beautiful Mercedes, almost new. If you are interested call me right now!"
                },
                new Car{
                    Id = Guid.NewGuid(),
                    Brand = "BMW",
                    Description = "Beautiful BMW, newer than the Mercedes (faster as well). If you are interested send me an email!"
                },
                new Car{
                    Id = Guid.NewGuid(),
                    Brand = "Audi",
                    Description = "Magnificent and stylish Audi. This is car for you, trust me! If you are interested just send me a letter right now!"
                }
            };

            return cars;
        }
    }
}
