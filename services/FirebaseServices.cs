using Firebase.Database;
using Firebase.Database.Query;


namespace FoodDeliveryAdminWebsite.services
{
   
           public class FirebaseServices
        {
            private readonly FirebaseClient _firebase;
            private readonly IConfiguration _configuration;

            public FirebaseServices(IConfiguration configuration)
            {
                _configuration = configuration;
                _firebase = new FirebaseClient(
                    _configuration["Firebase:BasePath"],
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(_configuration["Firebase:Secret"])
                    });
            }

            public FirebaseClient GetClient() => _firebase;
        }
   
}
