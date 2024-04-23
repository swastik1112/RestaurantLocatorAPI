using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantLocatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantsController : ControllerBase
    {   
        private static readonly List<Restaurant> Restaurants = new List<Restaurant>
        {
            new Restaurant { Name = "Restaurant A", Latitude = 40.7128, Longitude = -74.0060 },
            new Restaurant { Name = "Restaurant B", Latitude = 40.7120, Longitude = -74.0090 },
             new Restaurant { Name = "Restaurant C", Latitude = 40.7118, Longitude = -74.0010 },
            new Restaurant { Name = "Restaurant D", Latitude = 40.7110, Longitude = -74.0050 },
             new Restaurant { Name = "Restaurant E", Latitude = 40.7123, Longitude = -74.0040 },
            new Restaurant { Name = "Restaurant F", Latitude = 40.7125, Longitude = -74.0080 }

        };

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetRestaurants(double latitude, double longitude, double miles)
        {
            var userLocation = new GeoLocation(latitude, longitude);

            var nearbyRestaurants = Restaurants.Where(restaurant => userLocation.DistanceTo(new GeoLocation(restaurant.Latitude, restaurant.Longitude)) <= miles).ToList();

            return Ok(nearbyRestaurants);
        }
    }

    public class Restaurant
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GeoLocation
    {
        public GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public double DistanceTo(GeoLocation location)
        {
            var lat1 = Latitude * (Math.PI / 180);
            var lon1 = Longitude * (Math.PI / 180);
            var lat2 = location.Latitude * (Math.PI / 180);
            var lon2 = location.Longitude * (Math.PI / 180);

            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var radiusOfEarthInMiles = 3956.0;
            return radiusOfEarthInMiles * c;
        }
    }
}
