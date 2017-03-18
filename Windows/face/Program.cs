using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Face;
using Nito.AsyncEx;

namespace face
{
    class Program
    {

        static FaceServiceClient client = new FaceServiceClient("92a4d02d8c0c449e935d7298379f1ee1");

        static int choice = 0;
        static string input = "";
        static string[] names = { "89974018-dfe5-4e72-864b-8ccafd751ad9", "a06aa604-3563-4258-a80c-1757c06beee3", "0bad96d6-5b0a-4510-921f-79e5db317188", "fc67668c-ff5b-40c7-9654-593de7e423c5", "12098173-e8cf-4d34-b21b-f286deabdefe" };
        static Guid[] ids = new Guid[5];

        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                ids[i] = new Guid(names[i]);
            }
            while (choice != 9)
            {
                Console.WriteLine("Enter choice:");
                Console.WriteLine("1.   Add face");
                Console.WriteLine("2.   Train");
                Console.WriteLine("3.   Train status");
                Console.WriteLine("4.   Identify face");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AsyncContext.Run(() => addFace());
                        break;
                    case 2:
                        AsyncContext.Run(() => train());
                        break;
                    case 3:
                        AsyncContext.Run(() => trainStatus());
                        break;
                    case 4:
                        AsyncContext.Run(() => identify());
                        break;
                    default:
                        break;

                }
            }
        }
        static async Task train()
        {
            await client.TrainPersonGroupAsync("rejected");
            Console.WriteLine("Training...");

        }
        static async Task trainStatus()
        {
            TrainingStatus status = await client.GetPersonGroupTrainingStatusAsync("rejected");
            Console.WriteLine(status.Status.ToString());

        }

        static async Task addFace()
        {
            Console.WriteLine("Person:");
            Console.WriteLine("1.   Caleb");
            Console.WriteLine("2.   Adna");
            Console.WriteLine("3.   Jordan");
            Console.WriteLine("4.   Nicole");
            Console.WriteLine("5.   Nat");
            choice = Convert.ToInt32(Console.ReadLine())-1;
            Console.WriteLine("Paste URL of picture:");
            input = Console.ReadLine();
            Console.WriteLine("Loading...");
            Microsoft.ProjectOxford.Face.Contract.Face[] face = await client.DetectAsync(input);
            Microsoft.ProjectOxford.Face.Contract.FaceRectangle faceRect = face[0].FaceRectangle;
            AddPersistedFaceResult result = await client.AddPersonFaceAsync("rejected", ids[choice], input, null, faceRect);
            Console.WriteLine("Face added");

        }

        static async Task identify()
        {

            int i = 0, j=0, k = 0;
            Console.WriteLine("Paste URL of picture:");
            input = Console.ReadLine();
            Console.WriteLine("Loading...");
            Microsoft.ProjectOxford.Face.Contract.Face[] face = await client.DetectAsync(input);
            List<Guid> guids = new List<Guid>();
            foreach (Microsoft.ProjectOxford.Face.Contract.Face element in face)
            {
                guids.Add(element.FaceId);
                i++;
            }
            IdentifyResult[] results = await client.IdentifyAsync("rejected", guids.ToArray(), 1);
            List<Candidate[]> personIds = new List<Candidate[]>();
            foreach(IdentifyResult element in results)
            {
                personIds.Add(element.Candidates);
                j++;
            }
            List<Guid> guidnamelist = new List<Guid>();
            foreach (Candidate[] element in personIds)
            {
                foreach (Candidate candidate in element)
                {
                    guidnamelist.Add(candidate.PersonId);
                }
            }
            List<string> nameIds = new List<string>();
            foreach (Guid element in guidnamelist)
            {
                nameIds.Add(element.ToString());
                k++;
            }
            foreach (string nameid in nameIds)
            {
                switch (nameid)
                {
                    case "89974018-dfe5-4e72-864b-8ccafd751ad9":
                        Console.WriteLine("Caleb");
                        break;
                    case "a06aa604-3563-4258-a80c-1757c06beee3":
                        Console.WriteLine("Adna");
                        break;
                    case "0bad96d6-5b0a-4510-921f-79e5db317188":
                        Console.WriteLine("Jordan");
                        break;
                    case "fc67668c-ff5b-40c7-9654-593de7e423c5":
                        Console.WriteLine("Nicole");
                        break;
                    case "12098173-e8cf-4d34-b21b-f286deabdefe":
                        Console.WriteLine("Nathanael");
                        break;
                    default:
                        Console.WriteLine("Unknown");
                        break;
                }
            }
        }
    }
}
