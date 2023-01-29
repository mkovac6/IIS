using ErgastApi.Client;
using ErgastApi.Ids;
using ErgastApi.Requests;
using ErgastApi.Responses;

var client = new ErgastClient();

var request = new RaceResultsRequest
{
    Season = Seasons.Current,
    Round = Rounds.Last,
    DriverId = Drivers.MaxVerstappen,

    Limit = 25,
    Offset = 0,
};


RaceResultsResponse response = await client.GetResponseAsync(request);

var race = response.Races.First();

Console.WriteLine("Showing up to date data for driver: " + Drivers.MaxVerstappen);
Console.WriteLine("Race round: " + race.Round);
Console.WriteLine("Name of the race: " + race.RaceName);
Console.WriteLine("Name of the circuit: " + race.Circuit.CircuitName);

var driver = race.Results[0];

Console.WriteLine("Driver code: " + driver.Driver.Code);
Console.WriteLine("Fastest lap number: " + driver.FastestLap.LapNumber);
Console.WriteLine("Driver position at the end of the race: " + driver.Position);