using System;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using DarkSkyApi;
using DarkSkyApi.Models;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Net;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace Friday_s_Project
{
    class Program
    {
            static async Task Main(string[] args)
        {
            string command = "";
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();

            bool x = true;
            synthesizer.SpeakAsync("What would you like me to do, when done type exit.");

            while (x = true)
            {

                

                Console.WriteLine("What would you like me to do, when done type exit.");
                command = Console.ReadLine();





                if (command.Contains("date"))
                {
                    DateTime now = DateTime.Now;
                    synthesizer.SpeakAsync(Convert.ToString(now));
                    Console.WriteLine(Convert.ToString(now));
                }
                else if (command.Contains("weather"))
                {
                    var client = new DarkSkyService("ab03560fc770524915bc4c173486ef02");
                    Forecast result = await client.GetWeatherDataAsync(40.41305, -82.71121);
                    synthesizer.SpeakAsync(Convert.ToString(result.Currently.Summary));
                    synthesizer.SpeakAsync("the temperature today is " + Convert.ToString(result.Currently.Temperature) + "degrees");
                    Console.WriteLine(Convert.ToString(result.Currently.Summary));
                    Console.WriteLine(Convert.ToString(result.Currently.Temperature));

                }
                else if (command == "calculator")
                {

                    double num1 = 0, num2 = 0, answer = 0;
                    string operation = "";
                    bool num1Check = false, num2Check = false;
                    bool operationCheck = false;
                    string input1, input2;

                    //This stores user input for the first number in the variable num1
                    while (num1Check != true)
                    {
                        synthesizer.SpeakAsync("what is your first number");

                        Console.WriteLine("What is your first number");
                        input1 = Console.ReadLine();
                        if (Double.TryParse(input1, out num1))
                        {
                            num1 = Convert.ToDouble(input1);
                            num1Check = true;
                        }
                        else
                        {
                            Console.WriteLine("You did not enter a valid number.");
                        }
                    }


                    //This stores user input for the first number in the variable num1

                    synthesizer.SpeakAsync("what is your second number");
                    Console.WriteLine("What is your second number");
                    input2 = Console.ReadLine();
                    if (Double.TryParse(input2, out num2))
                    {
                        num2 = Convert.ToDouble(input2);
                        num2Check = true;
                    }
                    else
                    {
                        synthesizer.SpeakAsync("you did not enter a valid number");
                        Console.WriteLine("You did not enter a valid number.");
                    }

                    while (operationCheck == false)
                    {
                        synthesizer.SpeakAsync("What operation should I perform?");
                        Console.WriteLine("What operation should I perform ? (+, -, *, /)");
                        operation = Console.ReadLine();


                        if (operation == "+")
                        {
                            answer = num1 + num2;
                            operationCheck = true;
                        }

                        else if (operation == "-")
                        {
                            answer = num1 - num2;
                            operationCheck = true;
                        }
                        else if (operation == "*")
                        {
                            answer = num1 * num2;
                            operationCheck = true;
                        }
                        else if (operation == "/")
                        {
                            if (num2 != 0)
                            {
                                answer = num1 / num2;
                                operationCheck = true;
                            }
                            else
                            {
                                synthesizer.SpeakAsync("Sorry, I can't divide by zero.");
                                Console.WriteLine("Sorry, I can't divide by zero.");
                            }

                        }
                        else
                        {
                            synthesizer.SpeakAsync("You entered an invalid operation.");

                            Console.WriteLine("You entered an invalid operation.");
                        }
                        synthesizer.SpeakAsync(num1 + " " + operation + " " + num2 + " = " + answer);
                        Console.WriteLine(num1 + " " + operation + " " + num2 + " = " + answer);
                        Console.Read();
                    }
                }
                else if (command.Contains("news"))
                {
                    // init with your API key 
                    DateTime now = DateTime.Now;
                    var newsApiClient = new NewsApiClient("11115afda7fa47fcad70c9d20d392718");
                    var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
                    {

                        Q = "abc-news",
                        SortBy = SortBys.PublishedAt,
                        Language = Languages.EN,
                        From = now
                    }) ;
                    if (articlesResponse.Status == Statuses.Ok)
                    {
                        // total results found
                        Console.WriteLine(articlesResponse.Articles);
                        // here's the first 20
                        foreach (var article in articlesResponse.Articles)
                        {
                            Console.WriteLine("---------------------------------------------------------------------------------------");
                            // title
                            Console.WriteLine(article.Title);
                            // author
                            Console.WriteLine(article.Author);
                            // description
                            Console.WriteLine(article.Description);
                        }
                    }
                }
                else if (command == "exit")
                {
                    synthesizer.SpeakAsync("good bye");
                    Environment.Exit(0);
                }
                else
                {
                    string wolframSearch = "http://api.wolframalpha.com/v2/query?appid=GX6RHT-H7VW4LYK4X&input=" + command;
                    string html = string.Empty;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(wolframSearch);

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(html);
                    XmlNodeList xlist = xmldoc.SelectNodes("//plaintext");
                    foreach (XmlNode xn in xlist)
                    {
                        
                        string plaintext = xn.InnerText;
                        Console.WriteLine(plaintext);
                    }
                }
               


            }
            Console.Read();

        }
    }
}

