using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    public class ServiceResponse
    {
        public int ranking;
        public List<UserScore> scores;
    }


    [Serializable]
    public class UserScore
    {

        public string name { get; set; }
        public double score { get; set; }

    }
}
