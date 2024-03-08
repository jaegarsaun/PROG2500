using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Cube3D
{
    public class Grouping
    {
        Random random = new Random();
        int num_of_points;
        int threshold_distance;
        List<List<Dot>> sublists = new List<List<Dot>>();

        // Encapsulated field (Property) ... getters and setters
        // MasterList is the original set of points
        public List<Dot> MasterList { get; set; }


        public Grouping(int threshold_distance, int num_of_points)
        {
            this.num_of_points = num_of_points;
            this.threshold_distance = threshold_distance;


            // Create the new list (empty at first)
            MasterList = new List<Dot>();

            for (int i = 0; i < num_of_points; i++)
            {
                int rx = random.Next(-40, 40);
                int ry = random.Next(-40, 40);
                int rz = random.Next(-40, 40);  // add z for 3D
                Dot p = new Dot(rx, ry, rz); // make random point
                MasterList.Add(p); // add point to list
            }

        }

        public List<List<Dot>> applyThreseholdsMakeGroups()
        {
            // workingList is a copy of the MasterList
            List<Dot> workingList = new List<Dot>();
            foreach (Dot p in MasterList)
            {
                workingList.Add(p);
            }

            // the result build into list (a list of group lists)
            List<List<Dot>> lists = new List<List<Dot>>();

            /////////////////////////////////////////////
            while (workingList.Any())
            {
                List<Dot> newlist = GetGroup(workingList, threshold_distance);
                lists.Add(newlist);  // this is a group
                                     // the workingList is shrunk by the points taken to make this group
            }

            return lists; // the final list of groups
        }


        public List<Dot> GetGroup(List<Dot> workingList, int threshold_distance)
        {


            List<Dot> currentList = new List<Dot>();  // will be the new group
            List<Dot> straggleList = new List<Dot>(); // helps fill the group (no stragglers at end of the list)

            straggleList.Add(workingList[0]);   // first entry at least, might be alone in the list

            while (straggleList.Any())
            {

                // algo pulls from straggleList and adds to currentList
                Dot p = straggleList[0];
                straggleList.Remove(p);
                currentList.Add(p);

                foreach (Dot q in workingList)
                {
                    if (p != q)
                    {
                        double distance = Vector3D.Subtract(p.V, q.V).Length; // maybe put into Dot?
                        if (distance < threshold_distance)
                        {
                            if (!currentList.Contains(p))
                            {
                                currentList.Add(p);  // add if not already in current
                            }
                            if (!currentList.Contains(q))
                            {
                                currentList.Add(q);  // add if not already in current
                                straggleList.Add(q); // 
                            }


                        }
                        else
                        {
                            // avoid too much console output
                            Console.WriteLine(".");
                        }
                    }
                }
            }

            // remove these points from workingList
            // eventually all points are removed from workingList
            foreach (Dot q in currentList)
            {
                if (currentList.Contains(q))
                {
                    workingList.Remove(q);
                }
            }

            return currentList;
        }
    }
}
